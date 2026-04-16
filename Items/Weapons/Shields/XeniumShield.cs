using GuardiansOfRedemption.General;
using GuardiansOfRedemption.Projectiles.Shields;
using Microsoft.Xna.Framework;
using OrchidMod;
using OrchidMod.Content.Guardian;
using OrchidMod.Content.Guardian.Projectiles.Shields;
using Redemption.Items.Materials.HM;
using Redemption.Items.Materials.PostML;
using Redemption.Tiles.Furniture.Lab;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Items.Weapons.Shields;

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
        blockDuration = 420;
        shouldFlip = true; 
    }
    public override void Slam(Player player, Projectile shield)
    {
        if (IsLocalPlayer(player))
        {
            Projectile anchor = GetAnchor(player).Projectile;
            int type = ModContent.ProjectileType<XeniumShield_Projectile>();
            Vector2 dir = Vector2.Normalize(Main.MouseWorld - player.Center) * 5f;
            Projectile newProjectile = Projectile.NewProjectileDirect(Item.GetSource_FromThis(), anchor.Center, dir, type, (int)(shield.damage), Item.knockBack, player.whoAmI);
            newProjectile.CritChance = (int)(player.GetCritChance<GuardianDamageClass>() + player.GetCritChance<GenericDamageClass>() + Item.crit);
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