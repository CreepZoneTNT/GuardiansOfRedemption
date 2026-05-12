using System;
using GuardiansOfRedemption.General.Global;
using OrchidMod;
using OrchidMod.Content.Guardian;
using Redemption.Items.Materials.PostML;
using Redemption.Rarities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Content.Guardian.Accessories;

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
        RedemptionGuardian addonPlayer = player.GetModPlayer<RedemptionGuardian>();
        if (player == Main.LocalPlayer)
        {
            addonPlayer.GuardianCosmosChain = true;
        
            float chainDistance = Math.Clamp((Main.MouseWorld - player.Center).Length(), 16f, 640f);
            modPlayer.GuardianChain = chainDistance;
            modPlayer.GuardianChainTexture = Texture + "_Chain";
            
            if (player.HeldItem.ModItem is OrchidModGuardianHammer) player.GetAttackSpeed(DamageClass.Melee) /= 4f;
            
        }
    }

    public override void AddRecipes()
    {
        CreateRecipe()
        .AddIngredient<OmegaChain>()
        .AddIngredient<LifeFragment>(7)
        .AddTile(TileID.LunarCraftingStation)
        .Register();
    }
}