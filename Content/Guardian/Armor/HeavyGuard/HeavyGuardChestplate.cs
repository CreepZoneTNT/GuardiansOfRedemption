using OrchidMod;
using OrchidMod.Content.Guardian;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Content.Guardian.Armor.HeavyGuard
{
	[AutoloadEquip(EquipType.Body)]
	public class HeavyGuardChestplate : OrchidModGuardianEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 22;
			Item.value = Item.sellPrice(0, 3, 0, 0);
			Item.rare = ItemRarityID.Orange;
			Item.defense = 7;
		}

		public override void UpdateEquip(Player player)
		{
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
			modPlayer.GuardianGuardMax++;
		}

	}
}
