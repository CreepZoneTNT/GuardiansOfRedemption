using OrchidMod;
using OrchidMod.Content.Guardian;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Items.Weapons.Quarterstaves
{
	public class ThornQuarterstaff : OrchidModGuardianQuarterstaff {
		public override void SafeSetDefaults()
		{
			Item.width = 46;
			Item.height = 46;
			Item.value = Item.sellPrice(0, 0, 8, 80);
			Item.rare = ItemRarityID.Green;
			Item.useTime = 35;
			ParryDuration = 50;
			Item.knockBack = 4f;
			Item.damage = 70;
			GuardStacks = 1;
			SwingSpeed = 0.8f;
			CounterSpeed = 0.8f;
			JabSpeed = 0.8f;
		}
	}
}
