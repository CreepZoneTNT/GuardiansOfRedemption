using System;
using System.Collections.Generic;
using System.Linq;
using GuardiansOfRedemption.Projectiles.Warhammers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod;
using OrchidMod.Content.Guardian;
using OrchidMod.Utilities;
using Redemption;
using Redemption.BaseExtension;
using Redemption.Buffs.NPCBuffs;
using Redemption.Dusts;
using Redemption.Globals;
using Redemption.Projectiles.Melee;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace GuardiansOfRedemption.Items.Weapons.Warhammers;

public class CyberWarhammer : OrchidModGuardianHammer
{
    

    public override void SetStaticDefaults()
    { 
        ElementID.ItemThunder[Type] = true;
    }

    public override void SafeSetDefaults()
    {
        Item.width = 36;
        Item.height = 36;
        Item.value = Item.sellPrice(0, 10);
        Item.rare = ItemRarityID.LightPurple;
        Item.UseSound = SoundID.DD2_MonkStaffSwing;
        Item.knockBack = 8f;
        Item.shootSpeed = 15f;
        Item.damage = 174;
        Item.useTime = 20;
        Range = 60;
        SwingChargeGain = 0.8f;
        ReturnSpeed = 1.5f;
        BlockDuration = 180;
        Penetrate = true;
        TileCollide = false;
    }

    public override void ExtraAI(Player player, OrchidGuardian guardian, Projectile projectile)
    {
        projectile.light = 0.5f;
        if (projectile.ModProjectile is GuardianHammerAnchor anchor)
        {
            if (projectile.timeLeft < 598 && anchor.range % 10 == 0)
            {
                Projectile hologram = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.position, Vector2.Zero, ModContent.ProjectileType<CyberWarhammer_HologramProj>(), guardian.GetGuardianDamage(projectile.damage * 0.25f), projectile.knockBack, projectile.owner, projectile.rotation, projectile.direction, (anchor.WeakThrow ? 0.25f : 1.2f * projectile.velocity.Length() / 30f) * (projectile.velocity.X > 0 ? 1 : -1));
                hologram.scale = projectile.scale;
                hologram.rotation = projectile.rotation;
            }
        }
    }

    public override void OnThrow(Player player, OrchidGuardian guardian, Projectile projectile, bool Weak)
    {
        SoundEngine.PlaySound(SoundID.Item15, projectile.Center);
        if (!Weak) projectile.extraUpdates = 1;
    }

    public override void OnSwing(Player player, OrchidGuardian guardian, Projectile projectile, bool FullyCharged)
    {
        SoundEngine.PlaySound(SoundID.Item15, projectile.Center);
        Vector2 direction = Vector2.Normalize(Main.MouseWorld - projectile.Center);
        if (guardian.GuardianItemCharge - 45f >= 0) {
            Projectile hologram = Projectile.NewProjectileDirect(
                projectile.GetSource_FromThis(), 
                projectile.Center + player.velocity, 
                direction * Item.shootSpeed * (!FullyCharged ? 0.5f : 1f), 
                ModContent.ProjectileType<CyberWarhammer_HologramProj>(), 
                guardian.GetGuardianDamage(projectile.damage * 0.5f * (FullyCharged ? 2f : 1)), 
                projectile.knockBack, 
                projectile.owner, 
                projectile.rotation, 
                projectile.direction, 
                0.6f
            );
            hologram.scale = projectile.scale;
            hologram.rotation = projectile.rotation;
            if (FullyCharged) ((CyberWarhammer_HologramProj)hologram.ModProjectile).ShouldDrawTrail = true;
            guardian.GuardianItemCharge -= 45f;
        }
    }

    public override void OnMeleeHit(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, float knockback, bool crit, bool FullyCharged)
    {
        target.AddBuff(ModContent.BuffType<ElectrifiedDebuff>(), 180);
    }

    public override bool PreDrawHammer(Player player, OrchidGuardian guardian, Projectile projectile, SpriteBatch spriteBatch, ref Color lightColor, ref Texture2D hammerTexture, ref Rectangle drawRectangle)
    {
        if (projectile.timeLeft < 598)
        {
            GuardianHammerAnchor anchor = projectile.ModProjectile as GuardianHammerAnchor;
            Texture2D trailTexture = ModContent.Request<Texture2D>("GuardiansOfRedemption/Projectiles/Warhammers/CyberWarhammer_HologramProj").Value;

            if (anchor != null)
            {
                Main.spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
                
                for (int index = 0; index < anchor.OldPosition.Count; ++index)
                {
                    Vector2 position = anchor.OldPosition[index] - Main.screenPosition  + Vector2.UnitY * projectile.gfxOffY;
                    Color newColor = lightColor * ((float)(anchor.OldPosition.Count - index) / anchor.OldPosition.Count);
                    spriteBatch.Draw(trailTexture, position, null, projectile.GetAlpha(newColor), projectile.rotation, trailTexture.Size() * 0.5f, projectile.scale, projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
                }
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(spriteBatchSnapshot);
            }
        }
        return true;
    }
}