using GuardiansOfRedemption.Projectiles.Gauntlets;
using Microsoft.Xna.Framework;
using OrchidMod;
using OrchidMod.Content.Guardian;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Items.Weapons.Gauntlets;

public class ChickenGauntlet : OrchidModGuardianGauntlet
{
    public override void SafeSetDefaults()
    {
        Item.width = 26;
        Item.height = 28;
        Item.knockBack = 5f;
        Item.damage = 100;
        Item.value = Item.sellPrice(0, 0, 20);
        Item.rare = ItemRarityID.Blue;
        Item.useTime = 20;
        StrikeVelocity = 16f;
        ParryDuration = 90;
    }

    public override Color GetColor(bool offHand)
    {
        return new Color(202, 163, 95);
    }

    public override bool OnPunch(Player player, OrchidGuardian guardian, Projectile projectile, bool offHandGauntlet, bool manuallyFullyCharged, ref bool charged, ref int damage)
    {
        for (int i = 0; i < 3; i++) Dust.NewDustDirect(projectile.Center, projectile.width, projectile.height, DustID.Hay);
        return true;
    }

    public override void OnHit(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, NPC.HitInfo hit, bool charged)
    {
        if (charged && target.life <= 0 && Main.rand.NextBool(15))
        {
            SoundEngine.PlaySound(SoundID.Item129);
            Vector2 position = target.Center + Vector2.UnitY * target.height * 0.5f;
            Vector2 velocity = Vector2.UnitY.RotatedByRandom(MathHelper.Pi/3) * -20;
            for (int i = 0; i < 5; i++) Dust.NewDustPerfect(position, DustID.BeachShell, velocity.RotatedByRandom(MathHelper.Pi/12) * 0.5f);
            Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), position, velocity, ModContent.ProjectileType<ChickenGauntlet_EggProj>(), 0, 0, projectile.owner);
        }
    }
}