using System;
using GuardiansOfRedemption.Projectiles.Gauntlets;
using Microsoft.Xna.Framework;
using OrchidMod;
using OrchidMod.Content.General.Dusts;
using OrchidMod.Content.Guardian;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Items.Weapons.Gauntlets;

public class LivingGauntlet : OrchidModGuardianGauntlet
{
    public override void SafeSetDefaults()
    {
        Item.width = 28;
        Item.height = 30;
        Item.knockBack = 5f;
        Item.damage = 100;
        Item.value = Item.sellPrice(0, 0, 20);
        Item.rare = ItemRarityID.Blue;
        Item.useTime = 15;
        Item.shootSpeed = 7f;
        StrikeVelocity = 16f;
        ParryDuration = 90;
    }

    public override Color GetColor(bool offHand)
    {
        return new Color(202, 163, 95);
    }

    public override bool OnPunch(Player player, OrchidGuardian guardian, Projectile projectile, bool offHandGauntlet, bool manuallyFullyCharged, ref bool charged, ref int damage)
    {
        Vector2 direction = Vector2.Normalize(Main.MouseWorld - projectile.Center);
        for (int i = 0; i < 3 + (manuallyFullyCharged ? 2 : 0); i++)
        {
            Dust leaf = Dust.NewDustPerfect(projectile.Center, ModContent.DustType<LeafDust>(), direction.RotatedByRandom(MathHelper.Pi/5) * i / 4f);
            leaf.scale = 2f;

        }
        if (manuallyFullyCharged)
        {
            SoundEngine.PlaySound(SoundID.Item42, player.Center);
            Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.Center, direction * Item.shootSpeed, ModContent.ProjectileType<LivingGauntlet_SeedProj>(), guardian.GetGuardianDamage(Item.damage * 0.5f), projectile.knockBack);
        }
        return true;
    }

}