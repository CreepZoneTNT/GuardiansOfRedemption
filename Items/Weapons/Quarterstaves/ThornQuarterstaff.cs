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
			Item.width = 42;
			Item.height = 42;
			Item.value = Item.sellPrice(0, 0, 8, 80);
			Item.rare = ItemRarityID.Green;
			Item.useTime = 18;
			ParryDuration = 30;
			Item.knockBack = 4f;
			Item.damage = 42;
			GuardStacks = 1;
			SwingSpeed = 1.3f;
			CounterSpeed = 1.3f;
			JabSpeed = 1.3f;
		}
	}
}
