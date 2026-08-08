using System;
using System.Collections.Generic;
using GuardiansOfRedemption.Content.Other.Materials;
using GuardiansOfRedemption.Content.Other.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Redemption;
using Redemption.Dusts;
using Redemption.Globals;
using Redemption.Items.Critters;
using Redemption.Items.Weapons.PreHM.Ranged;
using Redemption.NPCs.FowlMorning;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Content.Other.Weapons.Ranged;

public class BasanRifle : ModItem
{
    public override void SetStaticDefaults()
    {
        ElementID.ItemFire[Type] = true;
        ElementID.ItemShadow[Type] = true;
        ElementID.ItemExplosive[Type] = true;
    }

    public override void SetDefaults()
    {
        Item.width = 48;
        Item.height = 16;
        Item.damage = 88;
        Item.DamageType = DamageClass.Ranged;
        Item.useTime = 68;
        Item.useAnimation = 68;
        Item.shoot = ProjectileID.Bullet;
        Item.shootSpeed = 20f;
        Item.useAmmo = AmmoID.Bullet;
        Item.holdStyle = ItemHoldStyleID.HoldFront;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.value = Item.sellPrice(0, 1);
        Item.rare = ItemRarityID.Orange;
        Item.UseSound = CustomSounds.HLShotgun1 with {PitchVariance = 0.5f};
        Item.noMelee = true;
    }

    public void HoldPosition(Player player, Rectangle heldItemFrame, Player.CompositeArmStretchAmount front, Player.CompositeArmStretchAmount back)
    {
        
        player.ChangeDir((Main.MouseWorld.X > player.Center.X).ToDirectionInt());
        float rotation = player.DirectionTo(Main.MouseWorld).ToRotation();
        rotation -= MathHelper.PiOver2 * player.gravDir;

        float animation = 1 - player.itemTime / (float)player.itemTimeMax;
        if (animation <= 0.25f) rotation += MathF.Max(MathF.Cos(animation * MathHelper.TwoPi), 0) * MathHelper.PiOver4 * -player.direction;
        
        player.SetCompositeArmFront(true, front, rotation);
        player.SetCompositeArmBack(true, back, rotation);
        
        Vector2 armPosition = player.GetBackHandPosition(back, rotation);
        armPosition -= (new Vector2(heldItemFrame.Width * player.direction, heldItemFrame.Height) * 0.5f).RotatedBy(rotation + MathHelper.PiOver2 * player.direction);
        player.itemLocation = armPosition - (player.Center - player.Center.Floor());
        // player.itemLocation += Vector2.UnitY * player.gfxOffY;
        player.itemRotation = rotation + MathHelper.PiOver2 * player.direction;
    }
        
    public override void HoldStyle(Player player, Rectangle heldItemFrame)
    {
        HoldPosition(player, heldItemFrame, Player.CompositeArmStretchAmount.ThreeQuarters, Player.CompositeArmStretchAmount.Full);
    }

    public override void UseStyle(Player player, Rectangle heldItemFrame)
    {
        HoldPosition(player, heldItemFrame, Player.CompositeArmStretchAmount.Quarter, Player.CompositeArmStretchAmount.ThreeQuarters);
    }

    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
        float rotation = player.DirectionTo(Main.MouseWorld).ToRotation();
        rotation -= MathHelper.PiOver2 * player.gravDir;
        
        Vector2 armPosition = player.GetBackHandPosition(Player.CompositeArmStretchAmount.ThreeQuarters, rotation);
        armPosition += (new Vector2(32 * player.direction, -3.5f) * Item.scale).RotatedBy(rotation + MathHelper.PiOver2 * player.direction);
        position = armPosition + Vector2.UnitY * player.gfxOffY - (player.Center - player.Center.Floor());
        
        for (int i = 0; i < 5; i++)
        {
            Dust dust = Dust.NewDustPerfect(position, ModContent.DustType<GlowDust>(), velocity.RotatedByRandom(0.236f) * Main.rand.NextFloat(0.1f, 0.4f), 240, new Color(217, 84, 155, 0), Main.rand.NextFloat(0.125f, 0.625f));
            dust.noGravity = true;
        }
        for (int i = 0; i < 7; i++)
        {
            Dust dust = Dust.NewDustPerfect(position, Main.rand.NextBool() ? ModContent.DustType<ChickenFeatherDust4>() : ModContent.DustType<ChickenFeatherDust2>(), velocity.RotatedByRandom(0.236f) * Main.rand.NextFloat(0.1f, 0.4f));
            dust.noGravity = true;
        }

        if (type is ProjectileID.Bullet or ProjectileID.SilverBullet)
        {
            velocity /= 1.5f;
            type = ModContent.ProjectileType<BasanRifleProjectile>();
        }
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<BasanMaterial>(7)
            .AddIngredient(ItemID.HellstoneBar, 6)
            .AddTile(TileID.Anvils)
            .Register();
    }
}