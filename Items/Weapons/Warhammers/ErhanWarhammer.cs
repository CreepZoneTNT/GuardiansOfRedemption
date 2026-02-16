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
    
    private float DrawTimer;

    public override void SetStaticDefaults()
    { 
        ElementID.ItemHoly[Type] = true;
        ElementID.ItemExplosive[Type] = true;
    }

    public override void SafeSetDefaults()
    {
        Item.width = 32;
        Item.height = 32;
        Item.value = Item.sellPrice(0, 0, 54);
        Item.rare = ItemRarityID.Blue;
        Item.UseSound = SoundID.DD2_MonkStaffSwing;
        Item.knockBack = 8f;
        Item.shootSpeed = 2f;
        Item.damage = 150;
        Item.useTime = 40;
        Range = 120;
        SwingChargeGain = 0f;
        ReturnSpeed = 0.9f;
        BlockDuration = 210;
        HasHolyLight = false;
        HolyLightStacks = 0;
    }

    public override bool ThrowAI(Player player, OrchidGuardian guardian, Projectile projectile, bool Weak)
    {
        if (!Weak)
        {
        
            if (guardian.UseSlam(1, true) || guardian.GuardianInfiniteResources)
            {
                HasHolyLight = true;
                HolyLightTimer = 900;
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
                guardian.UseSlam();
            }
            else
            {
                SoundEngine.PlaySound(SoundID.Item16, player.Center);
                CombatText.NewText(player.getRect(), Color.Red, "Not enough slams" + (Main.rand.NextBool(100) ? ", you idiot!" : "!"));
            }
        }
        projectile.Kill();
        return false;
    }

    public override void ExtraAI(Player player, OrchidGuardian guardian, Projectile projectile)
    {
        if (HasHolyLight)
        {
            Item.useTime = 15 + (15 * HolyLightStacks);
            projectile.damage = guardian.GetGuardianDamage(Item.damage * (1 + (0.2f * HolyLightStacks)));
        }
        else
        {
            Item.useTime = 40;
            projectile.damage = Item.damage;
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
            
            
        
            guardian.GuardianItemCharge = 0f;
        
            HasHolyLight = false;
            HolyLightStacks = 0;
            HolyLightTimer = 0; 
        
            projectile.Kill();
        }
        
    }

    // public override void OnThrow(Player player, OrchidGuardian guardian, Projectile projectile, bool Weak)
    // {
    //     base.OnThrow(player, guardian, projectile, Weak);
    // }

    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        base.ModifyTooltips(tooltips);
        
        int index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("Knockback"));
        if (index != -1)
        {
            tooltips[index + 3].Text = Language.GetTextValue("Mods.GuardiansOfRedemption.Items.ErhanWarhammer.ChargeToGlow");
            tooltips[index + 3].OverrideColor = new Color(175, 255, 175);
        }
    }

    public override bool PreDrawHammer(Player player, OrchidGuardian guardian, Projectile projectile, SpriteBatch spriteBatch, ref Color lightColor, ref Texture2D hammerTexture, ref Rectangle drawRectangle)
    {
        SpriteEffects effects = projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
        var position = projectile.Center - Main.screenPosition + Vector2.UnitY * player.gfxOffY;
        if (HasHolyLight)
        {
            float auraOpacity = HolyLightStacks < 3 ? HolyLightStacks * 0.4f + 0.2f * (float)Math.Sin(MathHelper.Pi / 60f * HolyLightTimer) : 1f;
            
            float rotationBonus = 0f;
            rotationBonus += projectile.spriteDirection == 1 ? MathHelper.PiOver2 : -MathHelper.PiOver2;
            if (projectile.ai[1] == 0)
                rotationBonus += guardian.GuardianItemCharge * 0.0065f * player.gravDir * projectile.spriteDirection;
            else if (projectile.ai[1] < 0)
                rotationBonus += (guardian.GuardianItemCharge * 0.0065f + (float)Math.Sin(MathHelper.Pi / 60f * projectile.ai[1]) * (3.5f + guardian.GuardianItemCharge * 0.006f)) * player.gravDir * projectile.spriteDirection;

            Main.spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            float scale = projectile.scale * (0.9f + HolyLightStacks * 0.1f);
            RedeDraw.DrawTreasureBagEffect(spriteBatch, hammerTexture, ref DrawTimer, position, drawRectangle, Color.Goldenrod * auraOpacity, projectile.rotation + rotationBonus - (projectile.spriteDirection == 1 ? MathHelper.PiOver2 : -MathHelper.PiOver2), hammerTexture.Size() * 0.5f, scale, projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
            if (HolyLightStacks == 3)
            {
                float rot = scale * (1f + (float) Math.Abs(Math.Sin(Main.GlobalTimeWrappedHourly * 4.5)) * 0.1f);
                RedeDraw.DrawGodrays(spriteBatch, position, Color.Goldenrod, 40f * rot * projectile.Opacity, 8 * rot * projectile.Opacity, 8);
            }
            
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(spriteBatchSnapshot);
            spriteBatch.Draw(hammerTexture, position, null, default, projectile.rotation + rotationBonus, drawRectangle.Size() * 0.5f, projectile.scale, effects, 0f);
        }
        return true;
    }
}
