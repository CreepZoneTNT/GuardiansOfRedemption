using OrchidMod.Content.Guardian;
using Redemption.Items.Materials.PreHM;
using Redemption.Items.Placeable.Tiles;
using Terraria;
using Terraria.ID;

namespace GuardiansOfRedemption.Items.Weapons.Shields;

public class PureIronShield : OrchidModGuardianShield
{
    public override void SafeSetDefaults()
    {
        Item.value = Item.sellPrice(0, 1);
        Item.width = 34;
        Item.height = 36;
        Item.knockBack = 6f;
        Item.damage = 100;
        Item.rare = ItemRarityID.Orange;
        Item.useTime = 30;
        distance = 60f;
        slamDistance = 85f;
        blockDuration = 120;
        shouldFlip = true; 
    }

    public override void AddRecipes()
    {
        CreateRecipe()
        .AddIngredient<PureIronAlloy>(12)
        .AddIngredient<ElderWood>(4)
        .AddTile(TileID.Anvils)
        .Register();
    }
}