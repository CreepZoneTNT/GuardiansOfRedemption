using System;
using GuardiansOfRedemption.Buffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod;
using OrchidMod.Content.Guardian;
using Redemption;
using Redemption.BaseExtension;
using Redemption.Buffs.NPCBuffs;
using Redemption.Dusts;
using Redemption.Globals;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using OrchidMod.Utilities;

namespace GuardiansOfRedemption.Items.Guardian.Weapons.Warhammers;

public class ZephosWarhammer : OrchidModGuardianHammer
{
    public override void SafeSetDefaults()
    {
        Item.width = 46;
        Item.height = 44;
        Item.value = Item.sellPrice(0, 35);
        Item.rare = ItemRarityID.Orange;
        Item.UseSound = SoundID.Item1;
        Item.knockBack = 10f;
        Item.shootSpeed = 12f;
        Item.damage = 100;
        Item.useTime = 45;
        Range = 60;
        SlamStacks = 2;
        BlockDuration = 210;
        Item.Redemption().CanSwordClash = true;
    }

    public override void ExtraAI(Player player, OrchidGuardian guardian, Projectile projectile)
    {
        if (projectile.ModProjectile is GuardianHammerAnchor anchor)
        {
            if (projectile.timeLeft < 598 && anchor.range > 0 && !anchor.WeakThrow && anchor.BlockDuration == 0)
            {

                projectile.direction = projectile.spriteDirection = projectile.velocity.X < 0 ? -1 : 1;
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver4 * projectile.spriteDirection;
            }

        //     if (projectile.ai[1] < 0) {
        //         float SwingOffset = projectile.ai[1] <= -30 ? -EaseFunction.EaseQuinticIn.Ease(2 + projectile.ai[1] / 30f) : EaseFunction.EaseCircularOut.Ease(1 + projectile.ai[1] / 30f);
        //         Vector2 arm = player.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, MathHelper.Pi - (guardian.GuardianItemCharge * 0.006f) * projectile.spriteDirection);
        //         player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathHelper.Pi + (guardian.GuardianItemCharge * 0.006f + SwingOffset * (3f + guardian.GuardianItemCharge * 0.006f)) * projectile.spriteDirection);
        //         Vector2 armPosition = player.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, MathHelper.Pi - (guardian.GuardianItemCharge * 0.006f + SwingOffset * (3f + guardian.GuardianItemCharge * 0.006f)) * projectile.spriteDirection) - (new Vector2(player.Center.X, player.Center.Y) - new Vector2(player.Center.X, player.Center.Y).Floor());
        //         projectile.Center = armPosition - new Vector2((anchor.hitboxOffset * 2 + 0.3f * guardian.GuardianItemCharge + (float)Math.Sin(MathHelper.Pi / 210f * guardian.GuardianItemCharge) * 10f) * player.direction * 0.4f + (armPosition.X - arm.X) * (2.5f + anchor.hitboxOffset * 0.07f), (armPosition.Y - arm.Y) * -(1.1f + anchor.hitboxOffset * 0.03f) + (210f - guardian.GuardianItemCharge) * 0.075f);

        //         if (guardian.GuardianChain > 0f && projectile.ai[1] < -20)
        //         {
        //             Vector2 chainDirection = Vector2.Normalize(projectile.Center - armPosition);
        //             float chainOffset = guardian.GuardianChain;
        //             if (projectile.ai[1] < -52) chainOffset = (chainOffset / 8f) * (projectile.ai[1] + 60);
        //             if (projectile.ai[1] > -35) chainOffset += (chainOffset / 15f) * (-projectile.ai[1] - 35);

        //             projectile.Center += chainDirection * chainOffset;
        //         }

        //         float toAdd = 30f / Item.useTime * SwingSpeed * player.GetTotalAttackSpeed(DamageClass.Melee);
        //         if (projectile.ai[1] < -40) projectile.ai[1] += toAdd * 1.5f;
        //         else
        //         {
        //             projectile.ai[1] += toAdd * 0.66f;
        //             projectile.friendly = false;
        //             projectile.netUpdate = true;
        //         }
        //     }
        }
    }

    public override void WarhammerModifyHitNPC(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, ref NPC.HitModifiers modifiers, bool FullyCharged, bool Melee, bool Block, bool firstHit)
    {
        modifiers.ArmorPenetration += 10;
        if (FullyCharged)
        {
            if (Melee && projectile.Center.Y < target.Top.Y) modifiers.SetCrit();
            if (NPCLists.SkeletonHumanoid.Contains(target.type)) modifiers.FinalDamage *= 1.5f;
        } 
    }

    public override void OnThrowHitFirst(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, float knockback, bool crit, bool Weak)
    {
        if (!Weak) player.AddBuff(ModContent.BuffType<ZephosWarhammerBuff>(), 600);
    }

    public override void OnThrowHit(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, float knockback, bool crit, bool Weak)
    {
        if (!Weak && projectile.Center.Y < target.Top.Y) target.AddBuff(ModContent.BuffType<BrokenArmorDebuff>(), NPCLists.SkeletonHumanoid.Contains(target.type) ? 120 : 40);
                    
    }

    public override void OnMeleeHitFirst(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, float knockback, bool crit, bool FullyCharged)
    {
        if (FullyCharged && projectile.Center.Y < target.Top.Y)
            RedeDraw.SpawnExplosion(target.Center, Color.White, shakeAmount: 0.0f, scale: 0.25f, noDust: false, tex: "Redemption/Textures/Shockwave2");
    }

    public override void OnMeleeHit(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, float knockback, bool crit, bool FullyCharged)
    {
        if (FullyCharged && projectile.Center.Y < target.Top.Y)
        {
            target.AddBuff(ModContent.BuffType<BrokenArmorDebuff>(), NPCLists.SkeletonHumanoid.Contains(target.type) ? 120 : 40);
            Dust.NewDustDirect(target.Top - new Vector2(8,8), 8, 8, ModContent.DustType<GlowDust>(), Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
            SoundEngine.PlaySound(CustomSounds.MaskBreak with {PitchRange = (-0.6f, 0.4f), Volume = 1.5f}, target.Top);
        }
    }

    // public override bool PreDrawHammer(Player player, OrchidGuardian guardian, Projectile projectile, SpriteBatch spriteBatch, ref Color lightColor, ref Texture2D hammerTexture, ref Rectangle drawRectangle)
    // {
    //     if (projectile.ModProjectile is GuardianHammerAnchor anchor)
    //     {
    //         float rotationBonus = 0f;

    //         SpriteEffects effect;
    //         if (projectile.spriteDirection == 1)
    //         {
    //             effect = SpriteEffects.FlipHorizontally;
    //             rotationBonus += MathHelper.PiOver2;
    //         }
    //         else
    //         {
    //             effect = SpriteEffects.None;
    //             rotationBonus -= MathHelper.PiOver2;
    //         }

    //         Vector2 posproj = projectile.Center;
    //         if (player.gravDir == -1)
    //         {
    //             if (projectile.ai[1] <= 0)
    //             {
    //                 posproj.Y = (player.Bottom.Floor() + player.position.Floor()).Y - posproj.Y;
    //             }
    //             if (effect == SpriteEffects.None) effect = SpriteEffects.FlipHorizontally;
    //             else effect = SpriteEffects.None;
    //         }

    //         var color = Lighting.GetColor((int)(projectile.Center.X / 16f), (int)(projectile.Center.Y / 16f), Color.White);
    //         var position = posproj - Main.screenPosition + Vector2.UnitY * player.gfxOffY;

    //         if (projectile.ai[1] == 0)
    //         {
    //             rotationBonus += guardian.GuardianItemCharge * 0.0065f * player.gravDir * projectile.spriteDirection;
    //         }

    //         if (projectile.ai[1] < 0)
    //         {
    //             float SwingOffset = projectile.ai[1] <= -30 ? -EaseFunction.EaseQuinticIn.Ease(2 + projectile.ai[1] / 30f) : EaseFunction.EaseCircularOut.Ease(1 + projectile.ai[1] / 30f);
    //             rotationBonus += (guardian.GuardianItemCharge * 0.0065f + SwingOffset * (3.5f + guardian.GuardianItemCharge * 0.006f)) * player.gravDir * projectile.spriteDirection;
    //         }

    //         if (BlockDuration != 0)
    //         {
    //             spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
    //             spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

    //             for (int i = 0; i < anchor.OldPosition.Count; i++)
    //             {
    //                 Vector2 drawPositionTrail = anchor.OldPosition[i] - Main.screenPosition + Vector2.UnitY * player.gfxOffY;
    //                 spriteBatch.Draw(hammerTexture, drawPositionTrail, drawRectangle, lightColor * 0.04f * (i + 1), anchor.OldRotation[i], drawRectangle.Size() * 0.5f, projectile.scale, effect, 0f);
    //             }

    //             spriteBatch.End();
    //             spriteBatch.Begin(spriteBatchSnapshot);
    //         }
    //         else if (guardian.GuardianChain > 0f && guardian.GuardianChainTexture != null)
    //         { // I want to consume a shoebox
    //             Texture2D chainTexture = ModContent.Request<Texture2D>(guardian.GuardianChainTexture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
    //             Vector2 chainDirection = Vector2.Normalize(projectile.Center - player.Center);
    //             float chainOffset = guardian.GuardianChain;
    //             if (projectile.ai[1] < -52) chainOffset = (chainOffset / 8f) * (projectile.ai[1] + 60);
    //             if (projectile.ai[1] > -35) chainOffset += (chainOffset / 15f) * (-projectile.ai[1] - 35);

    //             while (chainOffset > 0f)
    //             {
    //                 Vector2 chainPos = position - chainDirection * (chainOffset + hammerTexture.Height * 0.3f);
    //                 chainOffset -= chainTexture.Height * 0.66f;
    //                 spriteBatch.Draw(chainTexture, chainPos, null, color, 0f, chainTexture.Size() * 0.5f, 1f, effect, 0f);
    //             }
    //         }

    //         if (projectile.ai[1] != 0)
    //         {
    //             for (int i = 0; i < anchor.OldPosition.Count; i++)
    //             {
    //                 color = Lighting.GetColor((int)(anchor.OldPosition[i].X / 16f), (int)(anchor.OldPosition[i].Y / 16f), Color.White) * (anchor.WeakThrow ? (0.35f * i) - 0.65f : (0.15f * i));
    //                 position = anchor.OldPosition[i] - Main.screenPosition + Vector2.UnitY * player.gfxOffY;

    //                 spriteBatch.Draw(hammerTexture, position, drawRectangle, color, anchor.OldRotation[i] + rotationBonus, drawRectangle.Size() * 0.5f, projectile.scale, effect, 0f);
    //             }
    //         }

    //         spriteBatch.Draw(hammerTexture, position, drawRectangle, color, projectile.rotation + rotationBonus, drawRectangle.Size() * 0.5f, projectile.scale, effect, 0f);

    //     }
    

    //     return false;
    // }
}