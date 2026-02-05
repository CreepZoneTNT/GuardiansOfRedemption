using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace GuardiansOfRedemption.Items.Weapons.Gauntlets
{
	public class KeeperGauntlet : OrchidModGuardianGauntlet
	{
		public override void SafeSetDefaults()
		{
			Item.width = 44;
			Item.height = 50;
			Item.knockBack = 4f;
			Item.damage = 120;
			Item.value = Item.sellPrice(0, 1);
			Item.rare = ItemRarityID.Blue;
			Item.useTime = 20;
			StrikeVelocity = 16f;
			ParryDuration = 60;
			PunchSpeed = 1.3f;
		}

		public override Color GetColor(bool offHand)
		{
			return new Color(117, 51, 66);
		}
	}
}
