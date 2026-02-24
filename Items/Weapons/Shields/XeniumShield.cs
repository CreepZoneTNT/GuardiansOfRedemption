using OrchidMod.Content.Guardian;
using Redemption.Items.Materials.HM;
using Redemption.Items.Materials.PostML;
using Redemption.Tiles.Furniture.Lab;
using Terraria;
using Terraria.ID;

namespace GuardiansOfRedemption.Items.Weapons.Shields;

public class XeniumShield : OrchidModGuardianShield
{
    public override void SafeSetDefaults()
    {
        Item.value = Item.sellPrice(0, 15);
        Item.width = 34;
        Item.height = 40;
        Item.knockBack = 5f;
        Item.damage = 500;
        Item.rare = ItemRarityID.Purple;
        Item.useTime = 40;
        Item.shootSpeed = 10f;
        distance = 50f;
        slamDistance = 150f;
        blockDuration = 420;
        shouldFlip = true; 
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