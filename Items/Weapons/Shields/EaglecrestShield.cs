using GuardiansOfRedemption.Projectiles.Shields;
using Microsoft.Xna.Framework;
using OrchidMod;
using OrchidMod.Content.Guardian;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Items.Weapons.Shields;

public class EaglecrestShield : OrchidModGuardianShield
{
    public override void SafeSetDefaults()
    {
        Item.value = Item.sellPrice(0, 1);
        Item.width = 32;
        Item.height = 42;
        Item.knockBack = 6f;
        Item.damage = 60;
        Item.rare = ItemRarityID.Orange;
        Item.useTime = 20;
        Item.shootSpeed = 8f;
        distance = 45f;
        slamDistance = 75f;
        blockDuration = 120;
        shouldFlip = true; 
    }

    public override void Slam(Player player, Projectile shield)
    {
        OrchidGuardian guardian = player.GetModPlayer<OrchidGuardian>();
        SoundEngine.PlaySound(SoundID.NPCDeath43);
        for (int i = -2; i < 3; i++)
        {
            Projectile.NewProjectile(shield.GetSource_FromAI(), shield.Center, Vector2.UnitX.RotatedBy(shield.rotation + MathHelper.Pi + Main.rand.NextFloat(-MathHelper.Pi/24, MathHelper.Pi/24)) * Item.shootSpeed * (1 + 0.1f * i), ModContent.ProjectileType<EaglecrestShieldPebblesProj>(), guardian.GetGuardianDamage(shield.damage * 0.15f), Item.knockBack, shield.owner);
        }
    }
}