using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using GuardiansOfRedemption.Projectiles.Accessories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod;
using OrchidMod.Content.Guardian;
using OrchidMod.Content.Guardian.Weapons.Warhammers;
using Redemption.Buffs.Debuffs;
using Redemption.Buffs.NPCBuffs;
using Redemption.Dusts;
using Redemption.Dusts.Tiles;
using Redemption.Globals;
using Redemption.Items.Weapons.PostML.Melee;
using Redemption.Particles;
using Redemption.Projectiles.Magic;
using Redemption.Projectiles.Ranged;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace GuardiansOfRedemption.General.Global;

public class GlobalProjectiles : GlobalProjectile
{
    public override bool InstancePerEntity => true;

   

    public override void OnSpawn(Projectile projectile, IEntitySource source)
    {
        if (projectile.type == ProjectileID.BladeOfGrass && source is EntitySource_Parent { Entity: Projectile parentProjectile } && parentProjectile.type == ModContent.ProjectileType<GuardianHammerAnchor>())
        {
            projectile.GetGlobalProjectile<ElementalProjectile>().OverrideElement[ElementID.Poison] = 1;
            // CombatText.NewText(projectile.getRect(), Color.Green, "Test!");
        }
    }

    public override void AI(Projectile projectile)
    {
        if (projectile.owner == Main.myPlayer)
        {
            Player owner = Main.player[projectile.owner];
            
            OrchidGuardian guardian = owner.GetModPlayer<OrchidGuardian>();
            MoRGuardianPlayer addonGuardian = owner.GetModPlayer<MoRGuardianPlayer>();
            if (projectile.ModProjectile is GuardianShieldAnchor or GuardianHammerAnchor or GuardianQuarterstaffAnchor)
            {
                if (owner.magmaStone && Main.rand.NextBool(3))
                {
                    Dust magmaStoneDust = Dust.NewDustDirect(projectile.Center, projectile.width, projectile.height, DustID.Torch, projectile.velocity.X * 0.4f, projectile.velocity.Y * 0.4f, Scale: 2.5f);
                    magmaStoneDust.noGravity = true;
                }
                if (projectile.ModProjectile is GuardianHammerAnchor anchor)
                {
                    if (guardian.GuardianChain > 0f)
                    {
                        Vector2 armPosition = owner.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, MathHelper.Pi - (guardian.GuardianItemCharge * 0.006f + (float)Math.Sin(MathHelper.Pi / 60f * projectile.ai[1]) * (3f + guardian.GuardianItemCharge * 0.006f)) * projectile.spriteDirection) - (new Vector2(owner.Center.X, owner.Center.Y) - new Vector2(owner.Center.X, owner.Center.Y).Floor());
                        if (projectile.ai[1] < -35)
                        {
                            if (addonGuardian.GuardianCosmosChain)
                            { 
                                Dust cloudDust = Dust.NewDustDirect(projectile.Center, projectile.width * 2, projectile.height * 2, ModContent.DustType<GlowDust>(), Alpha: 80, newColor: Color.Orchid);
                                cloudDust.noGravity = true;
                                
                                if (Main.rand.NextBool(3))
                                    RedeParticleManager.CreateRainbowParticle(projectile.Center + projectile.velocity, RedeHelper.Spread(1f), Main.rand.NextFloat(0.4f, 0.6f), projectile.Opacity * 0.5f, Main.rand.Next(20, 40));
                                HitObjectsAlongChain(owner, projectile, armPosition, 0.75f * anchor.HammerItem.SwingDamage, cutTiles: true);
                                if (projectile.timeLeft % 8 == 0) Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<CosmosChain_StarProj>(), guardian.GetGuardianDamage(projectile.damage * 0.1f), projectile.knockBack);
                                projectile.extraUpdates = 3;
                            }
                            else if (addonGuardian.GuardianOmegaChain)
                            {
                                Dust cloudDust = Dust.NewDustDirect(projectile.Center, projectile.width * 2, projectile.height * 2, DustID.RedTorch, Scale: 2f);
                                cloudDust.noGravity = true;
                                HitObjectsAlongChain(owner, projectile, armPosition, 0.5f * anchor.HammerItem.SwingDamage, ModContent.BuffType<ElectrifiedDebuff>(), 3, cutTiles: true);
                                SpawnProjectilesAlongChain(owner, projectile, armPosition, ModContent.ProjectileType<OmegaChain_SparkProj>(), Vector2.Zero, 0.1f * anchor.HammerItem.SwingSpeed, 20, 64);           
                                projectile.extraUpdates = 1;
                            }
                            else if (addonGuardian.GuardianXenomiteChain)
                            {
                                Dust cloudDust = Dust.NewDustDirect(projectile.Center, projectile.width * 2, projectile.height * 2, ModContent.DustType<DustSteam>(), Scale: 0.25f);
                                cloudDust.noGravity = true;
                                if (Main.rand.NextBool(3))
                                {
                                    Dust starDust = Dust.NewDustDirect(projectile.Center, projectile.width, projectile.height, ModContent.DustType<XenoemiaDust>(), projectile.velocity.X * 0.4f, projectile.velocity.Y * 0.4f, Scale: 2.5f);
                                    starDust.noGravity = true;
                                }
                                
                                HitObjectsAlongChain(owner, projectile, armPosition, 0.25f * anchor.HammerItem.SwingDamage, ModContent.BuffType<GreenRashesDebuff>(), 5, cutTiles: true);
                                
                                
                            }
                        }
                        else projectile.extraUpdates = anchor.HammerItem is ThoriumThorsHammerWarhammer ? 1 : 0;
                    } 
                } 
            }
            
        }
    }

    public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
    {
        if (projectile.owner == Main.myPlayer)
        {
            Player owner = Main.player[projectile.owner];
            MoRGuardianPlayer modGuardian = owner.GetModPlayer<MoRGuardianPlayer>();
            if (projectile.ModProjectile is GuardianHammerAnchor)
            {
                if (modGuardian.GuardianOmegaChain)
                {
                    target.AddBuff(ModContent.BuffType<ElectrifiedDebuff>(), Main.rand.Next(1,6) * 60);
                }
                else if (modGuardian.GuardianXenomiteChain)
                {
                    target.AddBuff(ModContent.BuffType<GreenRashesDebuff>(), 300);
                    if (Main.rand.NextBool(5)) target.AddBuff(ModContent.BuffType<GlowingPustulesDebuff>(), 180);
                }
                else if (owner.magmaStone)
                {
                    switch (Main.rand.Next(8))
                    {
                        case 7 or 6:
                            target.AddBuff(BuffID.OnFire3, 360);
                            break;
                        case 5 or 4 or 3:
                            target.AddBuff(BuffID.OnFire3, 240);
                            break;
                        default:
                            target.AddBuff(BuffID.OnFire3, 120);
                            break;
                    }
                }
                    
            } 
            
        }
        
    }

    public override void ModifyHitPlayer(Projectile projectile, Player target, ref Player.HurtModifiers modifiers)
    {
        if (projectile.ModProjectile is GuardianHammerAnchor)
        {
            if (projectile.owner == Main.myPlayer && Main.player[projectile.owner].magmaStone)
            {
                
                if (Main.rand.Next(7) == 0)
                    target.AddBuff(BuffID.OnFire3, 360, quiet: false);
                else if (Main.rand.Next(3) == 0)
                    target.AddBuff(BuffID.OnFire3, 120, quiet: false);
                else
                    target.AddBuff(BuffID.OnFire3, 60, quiet: false);
            }
        }
    }

    private static void HitObjectsAlongChain(Player owner, Projectile projectile, Vector2 source, float tangleMultiplier, int debuffID = 0, float debuffSeconds = 0f, bool cutTiles = false)
    {
        OrchidGuardian guardian = owner.GetModPlayer<OrchidGuardian>();
        
        foreach (var npc in Main.npc)
        {
            if ((npc.CanBeChasedBy() || npc.immortal) && npc.immune[owner.whoAmI] == 0 && Collision.CheckAABBvLineCollision(npc.position, npc.Hitbox.Size(), source, projectile.Center))
            {
                npc.SimpleStrikeNPC(guardian.GetGuardianDamage(projectile.damage * tangleMultiplier), -npc.direction, projectile.HeldItemCrit(), projectile.knockBack, projectile.DamageType);
                npc.immune[owner.whoAmI] = 10;
                if (debuffID != 0) npc.AddBuff(debuffID, (int)(60f * debuffSeconds));
            }
        }
        if (cutTiles)
        {
            foreach(var point in GetPointsOfChain(owner, projectile, source))
            {
                Tile tileAtPoint = Framing.GetTileSafely(point);
                if (tileAtPoint.HasTile && Main.tileCut[tileAtPoint.TileType])
                {
                    Vector2 pointCoords = point / 16f; 
                    WorldGen.KillTile((int)pointCoords.X, (int)pointCoords.Y);
                    if (Main.netMode == NetmodeID.MultiplayerClient) NetMessage.SendData(MessageID.TileManipulation, number: 0, number2: (int)pointCoords.X, number3: (int)pointCoords.Y);
                }
            } 
        }
    }
    
    private static void SpawnProjectilesAlongChain(Player owner, Projectile projectile, Vector2 source, int projectileID, Vector2 velocity, float damageMultiplier = 1f, int spawnChance = 1, float minimumDistance = 0f)
    {
        List<Vector2> chainPoints = GetPointsOfChain(owner, projectile, source);
        OrchidGuardian guardian = owner.GetModPlayer<OrchidGuardian>();
        foreach (var point in chainPoints)
            if (Main.rand.NextBool(spawnChance) && (minimumDistance > 0f && point.DistanceSQ(owner.position) >= minimumDistance * minimumDistance)) Projectile.NewProjectileDirect(projectile.GetSource_FromAI(), point, velocity, projectileID, guardian.GetGuardianDamage(projectile.damage * damageMultiplier), projectile.knockBack, projectile.owner);
    }
    
    // Code for getting the position of chain links borrowed from Orchid's GuardianHammerAnchor.OrchidPreDraw method    
    public static List<Vector2> GetPointsOfChain(Player owner, Projectile projectile, Vector2 source)
    {
        List<Vector2> points = [];
        OrchidGuardian guardian = owner.GetModPlayer<OrchidGuardian>();
        if (projectile.ModProjectile is GuardianHammerAnchor anchor)
        {
            if (guardian.GuardianChain > 0f && guardian.GuardianChainTexture != null)
            {
                Texture2D chainTexture = ModContent.Request<Texture2D>(guardian.GuardianChainTexture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                Vector2 chainDirection = Vector2.Normalize(projectile.Center - source);
                float chainOffset = guardian.GuardianChain;
                if (projectile.ai[1] < -52) chainOffset = (chainOffset / 8f) * (projectile.ai[1] + 60);
                if (projectile.ai[1] > -35) chainOffset += (chainOffset / 15f) * (-projectile.ai[1] - 35);

                while (chainOffset > 0f)
                {
                    Vector2 chainPos = projectile.Center - chainDirection * (chainOffset + anchor.HammerTexture.Height * 0.3f);
                    chainOffset -= chainTexture.Height * 0.66f;
                    points.Add(chainPos);
                }
            }
        }
        return points;
    }
}