using OrchidMod.Content.Guardian;
using OrchidMod.Content.Guardian.Misc;
using Redemption.Items.Materials.PostML;
using Terraria;
using Terraria.ID;

namespace GuardiansOfRedemption.Items.Accessories;

public class GuardianMutagen : OrchidModGuardianEquipable
{
    
    public override void SafeSetDefaults()
    {
        Item.width = 28;
        Item.height = 36;
        Item.value = Item.sellPrice(0, 12);
        Item.rare = ItemRarityID.Purple;
        Item.accessory = true;
    }
    
    
    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetDamage<GuardianDamageClass>() += 0.15f;
        player.GetCritChance<GuardianDamageClass>() += 10;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<HorizonFragment>(10)
            .AddIngredient<EmptyMutagen>()
            .AddIngredient(ItemID.DestroyerEmblem)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
    }
}