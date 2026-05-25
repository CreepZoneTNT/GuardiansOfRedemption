using GuardiansOfRedemption.General.Global;
using OrchidMod;
using OrchidMod.Content.Guardian;
using Terraria;
using Terraria.ID;

namespace GuardiansOfRedemption.Content.Guardian.Accessories;

public class ProtectiveAmulet : OrchidModGuardianEquipable
{

    public override void SafeSetDefaults()
    {
        Item.width = 24;
        Item.height = 32;
        Item.value = Item.sellPrice(0, 0, 25, 0);
        Item.rare = ItemRarityID.Blue;
        Item.accessory = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
        RedemptionGuardian addonPlayer = player.GetModPlayer<RedemptionGuardian>();
        modPlayer.GuardianBlockDuration += 0.10f;
        modPlayer.GuardianParryDuration += 0.10f;
        addonPlayer.GuardianProtectiveAmulet = true;
    }
}