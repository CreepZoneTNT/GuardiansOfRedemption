using System;
using GuardiansOfRedemption.General.Global;
using OrchidMod;
using OrchidMod.Content.Guardian;
using OrchidMod.Content.Guardian.Accessories;
using Terraria;
using Terraria.ID;

namespace GuardiansOfRedemption.Items.Accessories;

public class MoltenChain : OrchidModGuardianEquipable
{
    public override void SafeSetDefaults()
    {
        Item.width = 20;
        Item.height = 30;
        Item.value = Item.sellPrice(0, 4, 50);
        Item.rare = ItemRarityID.LightRed;
        Item.accessory = true;
    }
    
    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
        if (player == Main.LocalPlayer)
        {
            player.magmaStone = true;
        
            modPlayer.GuardianChain = 64;
            modPlayer.GuardianChainTexture = Texture + "_Chain";
            
        }
    }

    public override void AddRecipes()
    {
        CreateRecipe()
        .AddIngredient<HeavyChain>()
        .AddIngredient(ItemID.MagmaStone)
        .AddTile(TileID.TinkerersWorkbench)
        .Register();
    }
}