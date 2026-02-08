using System;
using System.Numerics;
using FullSerializer.Internal;
using GuardiansOfRedemption.General.Global;
using OrchidMod;
using OrchidMod.Content.Guardian;
using OrchidMod.Content.Guardian.Accessories;
using Redemption.BaseExtension;
using Redemption.Buffs.Debuffs;
using Redemption.Items.Materials.PostML;
using Redemption.Items.Placeable.Tiles;
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
        MoRGuardianPlayer addonPlayer = player.GetModPlayer<MoRGuardianPlayer>();
        if (player == Main.LocalPlayer)
        {
            addonPlayer.GuardianCosmosChain = true;
        
            float chainDistance = Math.Clamp((Main.MouseWorld - player.Center).Length(), 16f, 960f);
            modPlayer.GuardianChain = chainDistance;
            modPlayer.GuardianChainTexture = Texture + "_Chain";
            
            if (player.HeldItem.ModItem is OrchidModGuardianHammer) player.GetAttackSpeed(DamageClass.Melee) *= 0.25f;
            
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