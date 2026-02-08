using GuardiansOfRedemption.General.Global;
using OrchidMod;
using OrchidMod.Content.Guardian;
using Redemption.Items.Materials.HM;
using Redemption.Items.Materials.PostML;
using Redemption.Items.Weapons.HM.Magic;
using Redemption.Items.Weapons.PostML.Ranged;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Items.Accessories;

public class OmegaChain : OrchidModGuardianEquipable
{
     
    public override void SafeSetDefaults()
    {
        Item.width = 44;
        Item.height = 36;
        Item.value = Item.sellPrice(0, 15);
        Item.rare = ItemRarityID.Red;
        Item.accessory = true;
    }
    
    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
        MoRGuardianPlayer addonPlayer = player.GetModPlayer<MoRGuardianPlayer>();
        if (player == Main.LocalPlayer)
        {
            modPlayer.GuardianChain = 192;
            modPlayer.GuardianChainTexture = Texture + "_Chain";
         
            addonPlayer.GuardianOmegaChain = true;
            
            if (player.HeldItem.ModItem is OrchidModGuardianHammer) player.GetAttackSpeed(DamageClass.Melee) *= 0.5f;
        }
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<XenomiteChain>()
            .AddIngredient<RoboBrain>()
            .AddIngredient<OmegaPowerCell>(2)
            .AddIngredient<CorruptedXenomite>(8)
            .AddIngredient<CarbonMyofibre>(6)
            .AddIngredient<Plating>(2)
            .AddIngredient<Capacitor>()
            .AddIngredient<AIChip>()
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }
}