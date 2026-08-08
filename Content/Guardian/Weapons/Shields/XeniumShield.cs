using GuardiansOfRedemption.General;
using GuardiansOfRedemption.Content.Guardian.Projectiles.Shields;
using Microsoft.Xna.Framework;
using OrchidMod;
using OrchidMod.Content.Guardian;
using OrchidMod.Utilities;
using Redemption.Items.Materials.HM;
using Redemption.Items.Materials.PostML;
using Redemption.Tiles.Furniture.Lab;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Content.Guardian.Weapons.Shields;

public class XeniumShield : OrchidModGuardianShield
{
    public override void SafeSetDefaults()
    {
        Item.value = Item.sellPrice(0, 15);
        Item.width = 34;
        Item.height = 40;
        Item.knockBack = 5f;
        Item.damage = 700;
        Item.rare = ItemRarityID.Purple;
        Item.useTime = 40;
        Item.shootSpeed = 5f;
        distance = 50f;
        slamDistance = 50f;
        blockDuration = 300;
        shouldFlip = true; 
    }
    public override void Slam(Player player, Projectile shield, bool WeakSlam)
    {
        if (shield.owner == Main.myPlayer && shield.ModProjectile is GuardianShieldAnchor anchor && !WeakSlam)
        {
            OrchidGuardian guardian = player.Guardian();

            Vector2 dir = Vector2.Normalize(Main.MouseWorld - player.Center) * Item.shootSpeed;
            Projectile newProjectile = Projectile.NewProjectileDirect(Item.GetSource_FromThis(), shield.Center, dir, ModContent.ProjectileType<XeniumShield_WaveProj>(), guardian.GetGuardianDamage(shield.damage), Item.knockBack, player.whoAmI);
            newProjectile.CritChance = (int)(player.GetCritChance<GuardianDamageClass>() + player.GetCritChance<GenericDamageClass>() + Item.crit);
            (newProjectile.ModProjectile as XeniumShield_WaveProj).Strong = true;
        }
    }

    public override void AddRecipes()
    {
        CreateRecipe()
        .AddIngredient<XeniumAlloy>(12)
        .AddIngredient<Capacitor>()
        .AddIngredient<CarbonMyofibre>(4)
        .AddTile<XeniumRefineryTile>()
        .Register();
    }
}