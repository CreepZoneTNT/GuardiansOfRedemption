using GuardiansOfRedemption.General;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod;
using OrchidMod.Common;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.Guardian;
using Redemption;
using Redemption.BaseExtension;
using Redemption.Dusts;
using Redemption.Globals;
using ReLogic.Content;
using System;
using OrchidMod.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Content.Guardian.Weapons.Gauntlets;

public class LaboratoryGauntlet : OrchidModGuardianGauntlet
{

    public float BonusCharge;
    public bool SuperCharged;
    public bool UberCharged;
    
    Texture2D textureGauntletSubOn = ModContent.Request<Texture2D>("GuardiansOfRedemption/General/UI/GauntletSubOn", AssetRequestMode.ImmediateLoad).Value;
    Texture2D textureGauntletSubOff = ModContent.Request<Texture2D>("GuardiansOfRedemption/General/UI/GauntletSubOff", AssetRequestMode.ImmediateLoad).Value;
    Texture2D textureGauntletSubReady = ModContent.Request<Texture2D>("GuardiansOfRedemption/General/UI/GauntletSubReady", AssetRequestMode.ImmediateLoad).Value;
    
    public override void SafeSetDefaults()
    {
        Item.width = 44;
        Item.height = 24;
        Item.knockBack = 8f;
        Item.damage = 660;
        Item.value = Item.sellPrice(0, 7, 50);
        Item.rare = ItemRarityID.Purple;
        Item.useTime = 45;
        StrikeVelocity = 24f;
        ParryDuration = 180;
        
    }

    public override Color GetColor(bool offHand)
    {
        return new Color(46, 178, 164);
    }
    
    

    public override void ExtraAIGauntlet(Player player, OrchidGuardian guardian, Projectile anchor, bool offHandGauntlet)
    {
        if (anchor.owner == Main.myPlayer && anchor.owner == player.whoAmI)
        {
            GuardianGauntletAnchor gauntlet = anchor.ModProjectile as GuardianGauntletAnchor;
            if (guardian.GuardianItemCharge >= 180f)
            {
                BonusCharge += 30f / Item.useTime * (player.GetTotalAttackSpeed(DamageClass.Melee) * 2f - 1f);
                
                // if ((int)BonusCharge % 10 == 0) CombatText.NewText(player.getRect(), Color.White, (int)BonusCharge);
                
                if (BonusCharge > 360f)
                {
                    if (!UberCharged)
                    {
                        if (ModContent.GetInstance<OrchidClientConfig>().GuardianAltChargeSounds) SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot, player.Center);
                        else SoundEngine.PlaySound(SoundID.MaxMana with { Pitch = 0.04f }, player.Center);
                        UberCharged = true;
                        for (int i = 0; i < 5; i++) Dust.NewDustDirect(player.MountedCenter - Vector2.UnitY * player.height * 0.5f, 24, 24, DustID.AncientLight, Main.rand.NextFloat(-1f, 1f), -1f);
                    }
                    BonusCharge = 360f;
                }
                else if (BonusCharge > 180f)
                {
                    if (!SuperCharged)
                    {
                        if (ModContent.GetInstance<OrchidClientConfig>().GuardianAltChargeSounds) SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot, player.Center);
                        else SoundEngine.PlaySound(SoundID.MaxMana with { Pitch = 0.02f }, player.Center);
                        SuperCharged = true;
                    }
                } 
                
                
                if (Main.rand.NextBool(4))
                {
                    Dust glow = Dust.NewDustPerfect(anchor.Center, ModContent.DustType<GlowDust>(), Vector2.Zero, 90, Color.LightSkyBlue, 0.2f);
                    glow.noGravity = true;
                }
            }
            else
            {
                // guardian.SlamCostUI = 0;
                BonusCharge = 0;
            }
        }
    }

    public override bool OnPunch(Player player, OrchidGuardian guardian, Projectile projectile, bool offHandGauntlet, bool manuallyFullyCharged, ref bool charged, ref int damage)
    {
        charged = ((GuardianGauntletAnchor)projectile.ModProjectile).Ding;
        if (manuallyFullyCharged)
        {
            projectile.penetrate = 1;
            if (UberCharged || SuperCharged)
            {
                damage = guardian.GetGuardianDamage(Item.damage * (UberCharged ? 6f : 3f));
             
                Vector2 velocity = Vector2.UnitX.RotatedBy((Main.MouseWorld - player.Center).ToRotation()) * 20f;
                int projectileType = ModContent.ProjectileType<LaboratoryGauntletProjectile>();
                Projectile newProjectile = Projectile.NewProjectileDirect(Item.GetSource_FromAI(), projectile.Center, velocity, projectileType, damage, UberCharged ? 10f : 6f, projectile.owner);
                newProjectile.CritChance = (int)(player.GetCritChance<GuardianDamageClass>() + player.GetCritChance<GenericDamageClass>() + Item.crit);
                ((LaboratoryGauntletProjectile)newProjectile.ModProjectile).Strong = UberCharged;
                if (UberCharged) newProjectile.timeLeft = 22;
                
                SoundEngine.PlaySound(CustomSounds.Swoosh1, projectile.Center);
                
                if (UberCharged) CombatText.NewText(player.getRect(), Color.RoyalBlue, "UberCharged!");
                else CombatText.NewText(player.getRect(), Color.DodgerBlue, "SuperCharged!");
                
                BonusCharge = 0f;
                SuperCharged = false;
                UberCharged = false;
                
                return false;
            }
            
            CombatText.NewText(player.getRect(), Color.SkyBlue, "Normal!");
        }
        
        BonusCharge = 0f;
        SuperCharged = false;
        UberCharged = false;
        return true;
    }

    public override void GauntletModifyHitNPC(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, ref NPC.HitModifiers modifiers, bool charged)
    {
        if (charged && !SuperCharged && !UberCharged)
        {
            SoundEngine.PlaySound(SoundID.Item94);
            RedeDraw.SpawnRing(projectile.Center, new Color(0, 191, 255), 0.1f, 0.8f);
            target.immune[player.whoAmI] = 10;
            RedeHelper.NPCRadiusDamage(80, projectile, player.Guardian().GetGuardianDamage(Item.damage * 0.8f), 4f);
            
            projectile.Kill();
        }
    }

    public override void GauntletPostDrawUI(SpriteBatch spriteBatch, Player player, ref Color lightColor, Projectile main, Projectile alt)
    {
        // Trying to draw the extra indicators next to the charge icon, so I'm borrowing the base draw code from Orchid (is that okay Verveine?)
        
        Texture2D textureGauntletOn = ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/GauntletOn", AssetRequestMode.ImmediateLoad).Value;
        
        OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
        if (player == Main.LocalPlayer && !player.dead && modPlayer.GuardianDisplayUI > 0)
        {
            bool minHoldTimer = modPlayer.ChargeHoldTimer > ModContent.GetInstance<OrchidClientConfig>().GuardianMinHoldTimer;
            bool maxHoldTimer = modPlayer.ChargeHoldTimer > ModContent.GetInstance<OrchidClientConfig>().GuardianMaxHoldTimer;
            
            bool drawAtCursor = ModContent.GetInstance<OrchidClientConfig>().GuardianChargeCursor;
    
            SpriteEffects effect = player.gravDir > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;
            
            Texture2D chargeTextureOn = null;
            Texture2D chargeTextureOff = null;
            Texture2D chargeTextureReady = null;
            
            
            if (player.HeldItem.ModItem is OrchidModGuardianGauntlet gauntlet && (maxHoldTimer || (minHoldTimer && modPlayer.GuardianItemCharge > (70 * player.GetTotalAttackSpeed(DamageClass.Melee) - (player.HeldItem.useTime * gauntlet.ChargeSpeedMultiplier)) / 2.5f)))
            {
                chargeTextureOn = textureGauntletSubOn;
                chargeTextureOff = textureGauntletSubOff;
                chargeTextureReady = textureGauntletSubReady;
            }
            
            if (chargeTextureOn != null)
            {
                
                int offSet = textureGauntletSubOn.Height + 3;
                
                if (drawAtCursor)
                {
                    
                    // spriteBatch.End();
                    // spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);
                    
                    if (player.gravDir < 0) return;
                    for (int i = 0; i < 2; i++)
                    {
                    
                        int val = chargeTextureOn.Height;
                        if (BonusCharge % 180f < 180f)
                        {
                            float charge = BonusCharge % 180f;
                            while (charge < 180f)
                            {
                                charge += 7.5f;
                                val--;
                            }
                        }
    
                        Rectangle rectangle = chargeTextureOn.Bounds;
                        rectangle.Height = val;
                        rectangle.Y = chargeTextureOn.Height - val;
                        
                        Vector2 drawpos = Main.MouseScreen + new Vector2(18 + textureGauntletSubOn.Width, 18 - offSet + (textureGauntletSubOn.Height + 2) * i) + textureGauntletOn.Size() * 0.5f;
                        // drawpos = Vector2.Transform(drawpos, Main.UIScaleMatrix);
                        
                        if ((int)Math.Floor(BonusCharge / 180f) - 1 >= i)
                        {
                            spriteBatch.Draw(chargeTextureReady, drawpos - new Vector2(2, 2), null, Color.White * 0.8f, 0f, Vector2.Zero, Main.UIScale, effect, 0f);
                        }
                        spriteBatch.Draw(chargeTextureOff, drawpos, null, Color.White, 0f, Vector2.Zero, Main.UIScale, effect, 0f);
                        
                        if ((int)Math.Floor(BonusCharge / 180f) - 1 >= i)
                            spriteBatch.Draw(chargeTextureOn, drawpos, null, Color.White, 0f, Vector2.Zero, Main.UIScale, effect, 0f);
                        else
                        {
                            if ((int)Math.Floor(BonusCharge / 180f) != i) return;
                            drawpos.Y += chargeTextureOn.Height - val;
                            spriteBatch.Draw(chargeTextureOn, drawpos, rectangle, Color.White, 0f, Vector2.Zero, Main.UIScale, effect, 0f);
                        }
                    }
                    
                    // spriteBatch.End();
                    // spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
                }
                else
                {
                    Vector2 position = (player.position + new Vector2(player.width * 0.5f, player.height + player.gfxOffY + 12)).Floor();
                    if (player.gravDir < 0) position.Y -= 81;
                    
                    for (int i = 0; i < 2; i ++)
                    {
                        int val = chargeTextureOn.Height;
                        if (BonusCharge % 180f < 180f)
                        {
                            float charge = BonusCharge % 180f;
                            while (charge < 180f)
                            {
                                charge += 7.5f;
                                val--;
                            }
                        }
    
                        Rectangle rectangle = chargeTextureOn.Bounds;
                        rectangle.Height = val;
                        rectangle.Y = chargeTextureOn.Height - val;
                        
                        Vector2 drawpos = new Vector2(position.X - 9 + textureGauntletSubOn.Width, position.Y - 94 * player.gravDir - offSet + (textureGauntletSubOn.Height + 4) * (player.gravDir < 0 ? i : 1 - i)) + textureGauntletOn.Size() * 0.5f - Main.screenPosition;
                        Vector2 gravOffSet = Vector2.UnitY * (player.gravDir - 1);
                        
                        
                        if ((int)Math.Floor(BonusCharge / 180f) - 1 >= i)
                        {
                            spriteBatch.Draw(chargeTextureReady, drawpos - new Vector2(2, 2) + gravOffSet * 5f * Main.GameViewMatrix.Zoom.Y, null, Color.White * 0.8f, 0f, Vector2.Zero, 1f, effect, 0f);
                        }
                        spriteBatch.Draw(chargeTextureOff, drawpos, null, Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
                        
                        // if (player.gravDir < 0) drawpos.Y -= chargeTextureOn.Height - rectangle.Height;
                        
                        if ((int)Math.Floor(BonusCharge / 180f) - 1 >= i)
                            spriteBatch.Draw(chargeTextureOn, drawpos + gravOffSet * 3f * Main.GameViewMatrix.Zoom.Y * player.gravDir, null, Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
                        else
                        {
                            if ((int)Math.Floor(BonusCharge / 180f) != i) return;
                            drawpos.Y += chargeTextureOn.Height - val;
                            spriteBatch.Draw(chargeTextureOn, drawpos, rectangle, Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
                        }
                    }
                }
            }
        }
    }
    
    public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
    {
        scale *= 1.2f;
        return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
    }
}

public class LaboratoryGauntletProjectile : OrchidModGuardianProjectile
{

    public override string Texture => "Redemption/Empty";
    public override void SetStaticDefaults()
    { 
        ElementID.ProjExplosive[Type] = true;
        ElementID.ProjThunder[Type] = true;
    }

    public override void SafeSetDefaults()
    {
        Projectile.width = 80;
        Projectile.height = 80;
        Projectile.friendly = true;
        Projectile.timeLeft = 17;
        Projectile.tileCollide = false;
        Projectile.stopsDealingDamageAfterPenetrateHits = true;
    }

    public override void OnSpawn(IEntitySource source) => Owner.GetModPlayer<OrchidPlayer>().PlayerImmunity = 5 * (Owner.longInvince ? 2 : 1);

    public override void AI()
    {
        Projectile.Center = Owner.Center;
        
        Owner.armorEffectDrawShadow = true;
        
        if (Main.rand.NextBool(Strong ? 3 : 6))
        {
            Dust dust = Dust.NewDustDirect(Projectile.Center, 24, 24, DustID.Electric);
            dust.noGravity = true;
        }
        if (Main.rand.NextBool(8)) SoundEngine.PlaySound(SoundID.NPCHit53 with {Volume = 0.2f}, Projectile.Center);
        
        OrchidPlayer orchidPlayer = Owner.GetModPlayer<OrchidPlayer>();
        orchidPlayer.ForcedVelocityVector = Projectile.velocity;
        orchidPlayer.ForcedVelocityTimer = 2;
        orchidPlayer.ForcedVelocityUpkeep = 0.6f;
        
        Owner.position += Collision.TileCollision(Owner.position, Projectile.velocity * 0.1f, Owner.width, Owner.height, true, true, (int)Owner.gravDir);
    }

    public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidGuardian guardian)
    {
        SoundEngine.PlaySound(SoundID.Item94);
        SoundEngine.PlaySound(SoundID.Item14 with {Volume = 1.2f}, Projectile.Center);
        
        RedeDraw.SpawnRing(Projectile.Center, new Color(0, 191, 255), Strong ? 0.25f : 0.2f, 0.85f, 4f);
        RedeDraw.SpawnRing(Projectile.Center, new Color(0, 191, 255), Strong ? 0.25f : 0.2f);
        
        for (int i = 0; i < 10; i++) Dust.NewDustPerfect(Projectile.Center, Main.rand.NextBool() ? ModContent.DustType<DustSpark2>() : ModContent.DustType<EnergySphereDust>(), Vector2.UnitX.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi) * (Strong ? 9f : 7f)), 30, Color.DodgerBlue);
        
        player.RedemptionScreen().ScreenShakeIntensity = Strong ? 6f : 4f;
        Owner.GetModPlayer<OrchidPlayer>().PlayerImmunity = 10 * (player.longInvince ? 2 : 1);
        
        target.immune[player.whoAmI] = 10;
        RedeHelper.NPCRadiusDamage(Strong ? 144 : 112, Projectile, player.Guardian().GetGuardianDamage(Projectile.damage * 0.5f), Strong ? 12f : 8f);
        
        Projectile.Kill();
    }

    public override void OnKill(int timeLeft)
    {
        Owner.armorEffectDrawShadow = false;
    }
}