using System.Numerics;
using FullSerializer.Internal;
using OrchidMod;
using OrchidMod.Content.Guardian;
using OrchidMod.Content.Guardian.Accessories;
using Redemption.Items.Materials.PostML;
using Redemption.Items.Weapons.PostML.Melee;
using Redemption.Rarities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Items.Accessories;

public class CosmosChain : OrchidModGuardianEquipable
{
    public override void SafeSetDefaults()
    {
        Item.width = 20;
        Item.height = 30;
        Item.value = Item.sellPrice(0, 20);
        Item.rare = ModContent.RarityType<CosmicRarity>();
        Item.accessory = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
        if (player == Main.LocalPlayer)
        {
            float chainDistance = (Main.MouseWorld - player.Center).Length();
            modPlayer.GuardianChain = chainDistance;
            modPlayer.GuardianChainTexture = Texture + "_Chain";
            
            if (player.HeldItem.ModItem is OrchidModGuardianHammer) player.GetModPlayer<OrchidGuardian>().GuardianMeleeSpeed *= 0.5f;
        }
    }

    public override void AddRecipes()
    {
        CreateRecipe()
        .AddIngredient<HeavyChain>()
        .AddIngredient<LifeFragment>(7)
        .AddTile(TileID.LunarCraftingStation)
        .Register();
    }
}