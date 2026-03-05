using GuardiansOfRedemption.General;
using GuardiansOfRedemption.Projectiles.Shields;
using Microsoft.Xna.Framework;
using OrchidMod;
using OrchidMod.Content.Guardian;
using Redemption.Globals;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Items.Weapons.Shields;

public class EaglecrestShield : OrchidModGuardianShield
{

    public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(ElementID.ThunderS);
    public override void SetStaticDefaults()
    { 
        ElementID.ProjectilesInheritElements[Type] = false;
        ElementID.ItemEarth[Type] = true;
    }
    public override void SafeSetDefaults()
    {
        Item.value = Item.sellPrice(0, 1);
        Item.width = 32;
        Item.height = 42;
        Item.knockBack = 6f;
        Item.damage = 90;
        Item.rare = ItemRarityID.Orange;
        Item.useTime = 20;
        Item.shootSpeed = 10f;
        distance = 45f;
        slamDistance = 60f;
        blockDuration = 120;
        shouldFlip = true; 
    }

    public override void Slam(Player player, Projectile shield)
    {
        OrchidGuardian guardian = player.Guardian();
        player.RedemptionGuardian().EaglecrestShieldTarget = null;
        player.RedemptionGuardian().EaglecrestShieldHitCount = 0;
        SoundEngine.PlaySound(SoundID.NPCDeath43);
        for (int i = -2; i < 3; i++)
        {
            Projectile.NewProjectile(shield.GetSource_FromAI(), shield.Center, Vector2.UnitX.RotatedBy(shield.rotation + MathHelper.Pi + Main.rand.NextFloat(-MathHelper.Pi/24, MathHelper.Pi/24)) * Item.shootSpeed * (1 + 0.025f * i), ModContent.ProjectileType<EaglecrestShield_PebblesProj>(), guardian.GetGuardianDamage(shield.damage * 0.15f), Item.knockBack, shield.owner);
            Dust.NewDustDirect(shield.Center, shield.width, shield.height, DustID.Stone);
        }
    }
}