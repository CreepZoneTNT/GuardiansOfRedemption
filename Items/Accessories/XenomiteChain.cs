using GuardiansOfRedemption.General.Global;
using OrchidMod;
using OrchidMod.Content.Guardian;
using OrchidMod.Content.Guardian.Accessories;
using Redemption.Items.Materials.HM;
using Terraria;
using Terraria.ID;

namespace GuardiansOfRedemption.Items.Accessories;

public class XenomiteChain : OrchidModGuardianEquipable
{
    
    public override void SafeSetDefaults()
    {
        Item.width = 32;
        Item.height = 36;
        Item.value = Item.sellPrice(0, 6);
        Item.rare = ItemRarityID.Lime;
        Item.accessory = true;
    }
    
    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
        MoRGuardianPlayer addonPlayer = player.GetModPlayer<MoRGuardianPlayer>();
        if (player == Main.LocalPlayer)
        {
            modPlayer.GuardianChain = 128;
            modPlayer.GuardianChainTexture = Texture + "_Chain";
         
            addonPlayer.GuardianXenomiteChain = true;   
        }
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<MoltenChain>()
            .AddIngredient<Xenomite>(15)
            .AddIngredient<ToxicBile>(10)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }
}