using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod;
using OrchidMod.Content.Guardian;
using OrchidMod.Utilities;
using Redemption;
using Redemption.BaseExtension;
using Redemption.Dusts;
using Redemption.Globals;
using Redemption.Projectiles.Melee;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace GuardiansOfRedemption.Items.Weapons.Warhammers;

public class ErhanWarhammer : OrchidModGuardianHammer
{

    public bool HasHolyLight = false;
    public int HolyLightStacks = 0;
    public int HolyLightTimer = 0;
    
    public bool NeedsMouseRelease = false;
    
    private float DrawTimer;
    
    public override void SafeSetDefaults()
    {
        Item.width = 36;
        Item.height = 36;
        Item.value = Item.sellPrice(0, 0, 54);
        Item.rare = ItemRarityID.Blue;
        Item.UseSound = SoundID.DD2_MonkStaffSwing;
        Item.knockBack = 8f;
        Item.shootSpeed = 2f;
        Item.damage = 150;
        Item.useTime = 30;
        Range = 120;
        SwingSpeed = 1.2f;
        SwingChargeGain = 0f;
        ReturnSpeed = 0.9f;
        BlockDuration = 210;
        HasHolyLight = false;
        HolyLightStacks = 0;
        NeedsMouseRelease = false;
    }

    public override bool ThrowAI(Player player, OrchidGuardian guardian, Projectile projectile, bool Weak)
    {
        if (!Weak)
        {
            HasHolyLight = true;
            HolyLightTimer = 300 + (HolyLightStacks * 60);
            HolyLightStacks++;
            if (HolyLightStacks > 3) HolyLightStacks = 3;
            
            CombatText.NewText(player.getRect(), Color.Goldenrod, HolyLightStacks, HolyLightStacks == 3);
            
            RedeDraw.SpawnRing(projectile.Center, new Color(255, 255, 120), 0.2f);
            SoundEngine.PlaySound(CustomSounds.NebSound2 with {Pitch = 0.1f}, player.position);
            if (HolyLightStacks == 3)
            {
                SoundEngine.PlaySound(CustomSounds.Choir with {Pitch = 0.4f}, player.position);
                RedeDraw.SpawnRing(projectile.Center, new Color(255, 255, 120), 0.2f, 0.85f, 4f);
            }
        }
        projectile.Kill();
        return false;
    }

    public override void ExtraAI(Player player, OrchidGuardian guardian, Projectile projectile)
    {
        if (NeedsMouseRelease && Main.mouseLeft && Main.mouseLeftRelease) NeedsMouseRelease = false;
        if (!NeedsMouseRelease)
        {
            if (HasHolyLight)
            {
                Item.useTime = 15 + (15 * HolyLightStacks); 
                if (Main.rand.NextBool((int)Math.Pow(2, 3 - HolyLightStacks))) Dust.NewDustDirect(projectile.Center, projectile.width, projectile.height, DustID.DesertTorch);
            }
    
            if (HolyLightTimer > 0) HolyLightTimer--;
            if (HolyLightTimer <= 0)
            {
                if (HasHolyLight)
                {
                    RedeDraw.SpawnRing(projectile.Center, new Color(255, 255, 120), 0.2f, 0.85f, 4f);
                    RedeDraw.SpawnRing(projectile.Center, new Color(255, 255, 120), 0.2f);
                    SoundEngine.PlaySound(CustomSounds.NebSound1 with { Pitch = 0.1f }, player.Center);
                }
                HolyLightTimer = 0;
                HolyLightStacks = 0;
                HasHolyLight = false;
            }
        }
        else
        {
            guardian.GuardianItemCharge = 0;
            projectile.ai[0] = 0;
            projectile.ai[1] = 0;
        }
        
    }

    public override void OnMeleeHit(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, float knockback, bool crit, bool FullyCharged)
    {
        if (HasHolyLight)
        {
            target.AddBuff(BuffID.OnFire, 180);
        }
    }

    public override void OnMeleeHitFirst(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, float knockback, bool crit, bool FullyCharged)
    {
        if (HasHolyLight)
        {
            if (HolyLightStacks == 3)
            {
                player.RedemptionScreen().ScreenShakeIntensity = 6f;
                RedeDraw.SpawnExplosion(target.Center, Color.White, shakeAmount: 0.0f, scale: 1f, noDust: true, tex: "Redemption/Textures/HolyGlow2");
            }
            else
            {
                RedeDraw.SpawnRing(target.Center, new Color(255, 255, 120), 0.2f, 0.85f, 4f);
                RedeDraw.SpawnRing(target.Center, new Color(255, 255, 120), 0.2f);
            }
            SoundEngine.PlaySound(HolyLightStacks == 3 ? CustomSounds.HeavyMagic1 : CustomSounds.Saint1);
            
            guardian.UseSlam(HolyLightStacks);
        
            guardian.GuardianItemCharge = 0f;
        
            HasHolyLight = false;
            HolyLightStacks = 0;
            HolyLightTimer = 0; 
        
            NeedsMouseRelease = true;
            projectile.Kill();
        }
        
    }

    // public override void OnThrow(Player player, OrchidGuardian guardian, Projectile projectile, bool Weak)
    // {
    //     base.OnThrow(player, guardian, projectile, Weak);
    // }

    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        foreach (var ttip in tooltips)
        {
            if (ttip.Name.Equals("Swing") && ttip.Mod.Equals("OrchidMod"))
            {
                ttip.Text = Language.GetTextValue("Mods.GuardiansOfRedemption.Items.ErhanWarhammer.SwingToGlow");
                ttip.OverrideColor = new Color(175, 255, 175);
            }
        }
    }

    public override bool PreDrawHammer(Player player, OrchidGuardian guardian, Projectile projectile, SpriteBatch spriteBatch, ref Color lightColor, ref Texture2D hammerTexture, ref Rectangle drawRectangle)
    {
        SpriteEffects effects = projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
        var position = projectile.Center - Main.screenPosition + Vector2.UnitY * player.gfxOffY;
        if (HasHolyLight)
        {
            float rotationBonus = 0f;
            rotationBonus += projectile.spriteDirection == 1 ? MathHelper.PiOver2 : -MathHelper.PiOver2;
            if (projectile.ai[1] == 0)
                rotationBonus += guardian.GuardianItemCharge * 0.0065f * player.gravDir * projectile.spriteDirection;
            else if (projectile.ai[1] < 0)
                rotationBonus += (guardian.GuardianItemCharge * 0.0065f + (float)Math.Sin(MathHelper.Pi / 60f * projectile.ai[1]) * (3.5f + guardian.GuardianItemCharge * 0.006f)) * player.gravDir * projectile.spriteDirection;

            Main.spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            RedeDraw.DrawTreasureBagEffect(spriteBatch, hammerTexture, ref DrawTimer, position, drawRectangle, Color.Goldenrod, projectile.rotation + rotationBonus - (projectile.spriteDirection == 1 ? MathHelper.PiOver2 : -MathHelper.PiOver2), hammerTexture.Size() * 0.5f, projectile.scale, projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
            
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(spriteBatchSnapshot);
            spriteBatch.Draw(hammerTexture, position, null, default, projectile.rotation + rotationBonus, drawRectangle.Size() * 0.5f, projectile.scale, effects, 0f);
        }
        return true;
    }
}