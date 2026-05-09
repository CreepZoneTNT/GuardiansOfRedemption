using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using OrchidMod.Content.Guardian;
using Redemption;
using Redemption.BaseExtension;
using Redemption.Buffs.NPCBuffs;
using GuardiansOfRedemption.General;
using GuardiansOfRedemption.Projectiles.Quarterstaves;
using OrchidMod;
using static Terraria.Player;
using OrchidMod.Common.ModObjects;
using Redemption.Globals;
using GuardiansOfRedemption.Buffs;

namespace GuardiansOfRedemption.Items.Guardian.Weapons.Quarterstaves;
public class DaerelQuarterstaff : OrchidModGuardianQuarterstaff {

    public Vector2 tip;

    public int ComboHit;

    public Vector2 LungeVelocity;

    public enum JabPhases {
        SwingUp,
        SwingDown,
        Jab
    }
    public JabPhases JabPhase;

    public enum SwingPhase {
        Lunge,
        Swing
    }

    public override void SafeSetDefaults() {
        Item.width = 52;
        Item.height = 58;
        Item.value = Item.sellPrice(0, 15);
        Item.rare = ItemRarityID.Yellow;
        Item.useTime = 20;
        Item.knockBack = 4f;
        Item.damage = 216;
        SlamStacks = 2;
        GuardStacks = 2;
        ParryDuration = 200;
        JabChargeGain = 3f;

        Item.Redemption().TechnicallySlash = true;
        
        LungeVelocity = Vector2.Zero;
        ComboHit = 0;
    }

    // Movement speed code borrowed from NanitesGauntlet.cs 
    public override void SafeHoldItem(Player player)
    {
        if (player.mount.Type == MountID.None)
        {
            Vector2 intendedVelocity = player.velocity * 0.05f;
				Vector2 addedVelocity = Vector2.Zero;

				for (int i = 0; i < 10; i++)
					addedVelocity += Collision.TileCollision(player.position + addedVelocity, intendedVelocity, player.width, player.height, false, false, (int)player.gravDir);

				if (addedVelocity.Length() > 0.1f) player.position += addedVelocity;
        }
    }

    public override void ExtraAIQuarterstaff(Player player, OrchidGuardian guardian, Projectile projectile) {

        tip = (projectile.ModProjectile as GuardianQuarterstaffAnchor).GetQuarterstaffTip(0.25f);

        if (ComboHit > 0) guardian.GuardianItemCharge = 60f * ComboHit;
        else {
            guardian.GuardianItemCharge -= 90f / Item.useTime * player.GetTotalAttackSpeed(DamageClass.Melee);
            if (guardian.GuardianItemCharge <= 0) guardian.GuardianItemCharge = 0;
        }

        if (Main.player[projectile.owner].mount.Type == MountID.None) Main.player[projectile.owner].armorEffectDrawShadow = true;

        if (--projectile.localAI[0] < 0) {
            projectile.localAI[0] = 0;
            JabPhase = JabPhases.Jab;
            ComboHit = 0;
            
        }

        if (guardian.GuardianShowDebugVisuals) {
            for (int i = 0; i < 40; i++) {
				Vector2 pos = Main.rand.NextVector2RectangleEdge(tip, new(42, 42));
				Dust dust = Dust.NewDustPerfect(pos, DustID.Torch, Vector2.Zero);
				dust.noGravity = true;
            }
        }
    }

    public override void OnAttack(Player player, OrchidGuardian guardian, Projectile projectile, bool jabAttack, bool counterAttack)
    {
        if (projectile.ModProjectile is GuardianQuarterstaffAnchor anchor) {
            OrchidPlayer orchidPlayer = player.GetModPlayer<OrchidPlayer>();
            if (counterAttack) {}
            else {
                if (jabAttack) { 
                    if (JabPhase == JabPhases.SwingUp && ComboHit == 1) JabPhase = JabPhases.SwingDown;
                    else if (JabPhase == JabPhases.SwingDown && ComboHit == 2) 
                    {
                        JabPhase = JabPhases.Jab;

                        player.ClearBuff(ModContent.BuffType<DaerelQuarterstaffBuff>());

                        orchidPlayer.ForcedVelocityVector = Vector2.UnitX.RotatedBy((Main.MouseWorld - player.Center).ToRotation()) * 10f;
                        orchidPlayer.ForcedVelocityTimer = 15;
                        orchidPlayer.ForcedVelocityUpkeep = 0.5f;
                        LungeVelocity = orchidPlayer.ForcedVelocityVector;

                        orchidPlayer.PlayerImmunity = 15;
                        player.immuneTime = 15;
                        player.immune = true;
                    }
                    else if (JabPhase == JabPhases.Jab && ComboHit == 3) JabPhase = JabPhases.SwingUp;
                    else {
                        JabPhase = JabPhases.SwingUp;
                        ComboHit = 0;
                        // Placeholder sound for flubbing combo
                        // SoundEngine.PlaySound(CustomSounds.Doot, tip);
                    }

                    projectile.Center = player.MountedCenter.Floor() + new Vector2(12f * player.direction, 0f);
                    projectile.rotation = MathHelper.PiOver4 * player.direction - MathHelper.PiOver4;
                    anchor.OldPosition.Clear();
                    anchor.OldRotation.Clear();

                    projectile.localAI[0] = 90;

                }
                else {
                    guardian.GuardianItemCharge = float.Epsilon;
                    ComboHit = 0;

                    orchidPlayer.ForcedVelocityVector = Vector2.UnitX.RotatedBy((Main.MouseWorld - player.Center).ToRotation()) * 12f;
                    orchidPlayer.ForcedVelocityTimer = 20;
                    orchidPlayer.ForcedVelocityUpkeep = 0.5f;
                    LungeVelocity = orchidPlayer.ForcedVelocityVector;

                    int immune = player.longInvince ? 30 : 15;
                    orchidPlayer.PlayerImmunity = immune;
                    player.immuneTime = immune;
                    player.immune = true;
                }
            }
            
        }
    }

    public override void OnHitFirst(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, NPC.HitInfo hit, bool jabAttack, bool counterAttack)
    {
        if (counterAttack) {
            player.AddBuff(ModContent.BuffType<DaerelQuarterstaffBuff>(), 600);
        }
        else {
            if (jabAttack) {
                // Set swing 
                if (ComboHit == 0 && JabPhase == JabPhases.SwingUp) ComboHit = 1;
                else if (ComboHit == 1 && JabPhase == JabPhases.SwingDown) ComboHit = 2;
                else if (ComboHit == 2 && JabPhase == JabPhases.Jab) ComboHit = 3;
                // Default value, if 
                else ComboHit = 0;

                projectile.localAI[0] = JabPhase == JabPhases.Jab ? 180 : 90;
            } 
        }
        
    }

    // 3-hit combo mechanic; code borrowed from GuardianQuarterstaffAnchor.cs
    public override bool PreJabAI(Player player, OrchidGuardian guardian, Projectile anchor)
    {
        switch(JabPhase) {
            case JabPhases.SwingUp or JabPhases.SwingDown: {
                int swingDir = JabPhase == JabPhases.SwingDown ? -1: 1;

                if (player.HasBuff<DaerelQuarterstaffBuff>()) anchor.ai[0] += 0.5f * JabSpeed * player.GetTotalAttackSpeed(DamageClass.Melee);

                anchor.rotation = anchor.ai[1] - MathHelper.PiOver4 + (float)Math.Cos(0.102f * (-anchor.ai[0] - 9)) * 1.9f * -player.direction * swingDir + MathHelper.Pi;
                anchor.Center = player.MountedCenter.Floor() + Vector2.UnitY.RotatedBy(anchor.ai[1] + (float)Math.Cos(0.102f * (-anchor.ai[0] - 9)) * 1.8f * -player.direction * swingDir) * 24f;
                player.SetCompositeArmFront(true, CompositeArmStretchAmount.Full, MathHelper.PiOver4 * player.direction + anchor.ai[1] + 0.1f - (float)Math.Cos(0.102f * (-anchor.ai[0] - 9)) * -player.direction * swingDir);
                player.SetCompositeArmBack(true, CompositeArmStretchAmount.Full, anchor.ai[1] - 0.1f + (float)Math.Cos(0.102f * (-anchor.ai[0]- 9)) * 0.2f * -player.direction * swingDir);

                if (anchor.ai[0] >= 0) anchor.ai[0] = guardian.GuardianItemCharge > 0 ? 1f : 0f;
                break;
            }
            case JabPhases.Jab: {
                if (-anchor.ai[0] <= 30)
					{
						anchor.friendly = false;
						anchor.rotation = anchor.ai[1] - MathHelper.PiOver4 + (float)Math.Sin(0.1046f * (30 + anchor.ai[0])) * 0.4f * -player.direction + MathHelper.Pi;
						anchor.Center = player.MountedCenter.Floor() + Vector2.UnitY.RotatedBy(anchor.ai[1]) * (38f - (float)Math.Sin(0.0523f * (30 + anchor.ai[0])) * 24f);
						anchor.position.Y -= (float)Math.Sin(0.0523f * (30 + anchor.ai[0])) * 2f;
						player.SetCompositeArmFront(true, CompositeArmStretchAmount.Quarter, MathHelper.PiOver4 * player.direction + anchor.ai[1] + 0.1f + (float)Math.Sin(0.1046f * (30 - + anchor.ai[0])) * 0.3f * player.direction);
						player.SetCompositeArmBack(true, CompositeArmStretchAmount.Full, anchor.ai[1] - 0.1f + (float)Math.Sin(0.1046f * (30 + anchor.ai[0])) * 0.2f * player.direction);
					}
					else
					{
						anchor.Center = player.MountedCenter.Floor() + Vector2.UnitY.RotatedBy(anchor.ai[1]) * 3.8f * (40 + anchor.ai[0]);
						anchor.rotation = anchor.ai[1] - MathHelper.PiOver4 + MathHelper.Pi;
						player.SetCompositeArmFront(true, CompositeArmStretchAmount.None, MathHelper.PiOver4 * player.direction + anchor.ai[1] + 0.1f);
						player.SetCompositeArmBack(true, CompositeArmStretchAmount.ThreeQuarters, anchor.ai[1] - 0.1f);
					}
                    break;
            }          
        }
        return false;
    }

    // public override bool PreSwingAI(Player player, OrchidGuardian guardian, Projectile anchor) {
    //     if (anchor.ai[0] <= 30) {
            
    //     }
    //     else {
    //         anchor.Center = player.MountedCenter.Floor() + Vector2.UnitY.RotatedBy(anchor.ai[1]) * 3.8f * (40 + anchor.ai[0]);
    //         anchor.rotation = anchor.ai[1] - MathHelper.PiOver4 + MathHelper.Pi;
    //         player.SetCompositeArmFront(true, CompositeArmStretchAmount.None, MathHelper.PiOver4 * player.direction + anchor.ai[1] + 0.1f);
    //         player.SetCompositeArmBack(true, CompositeArmStretchAmount.ThreeQuarters, anchor.ai[1] - 0.1f);
    //     }
    //     return false;
    // }

    // Sweet spot mechanic
    public override void QuarterstaffModifyHitNPC(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, ref NPC.HitModifiers modifiers, bool jabAttack, bool counterAttack, bool firstHit)
    {
        if (Collision.CheckAABBvAABBCollision(target.Center, target.Hitbox.Size(), tip, new(42, 42))) {
            modifiers.FinalDamage *= 1.5f;
            target.AddBuff(ModContent.BuffType<DisarmedDebuff>(), 180);			
            RedeDraw.SpawnExplosion(RedeHelper.CenterPoint(projectile.Center, target.Center), Color.White, shakeAmount: 0, scale: 1f, noDust: true, rot: Main.rand.NextFloat(MathHelper.PiOver4, 3 * MathHelper.PiOver4), tex: "Redemption/Textures/SwordClash");
            SoundEngine.PlaySound(CustomSounds.SwordClash, tip);
        }
    }

}