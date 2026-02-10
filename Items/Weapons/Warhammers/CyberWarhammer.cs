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
using Redemption.Buffs.NPCBuffs;
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
        Item.shootSpeed = 16f;
        Item.damage = 300;
        Item.useTime = 20;
        Range = 80;
        SwingChargeGain = 0.8f;
        ReturnSpeed = 1.5f;
        BlockDuration = 180;
    }

    

    public override void ExtraAI(Player player, OrchidGuardian guardian, Projectile projectile)
    {
        
    }

    public override void OnMeleeHit(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, float knockback, bool crit, bool FullyCharged)
    {
        target.AddBuff(ModContent.BuffType<ElectrifiedDebuff>(), 180);
    }

    

    public override bool PreDrawHammer(Player player, OrchidGuardian guardian, Projectile projectile, SpriteBatch spriteBatch, ref Color lightColor, ref Texture2D hammerTexture, ref Rectangle drawRectangle)
    {
        SpriteEffects effects = projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
        var position = projectile.Center - Main.screenPosition + Vector2.UnitY * player.gfxOffY;
        return true;
    }
}