using OrchidMod;
using OrchidMod.Content.Guardian;
using Terraria;
using Terraria.ID;

namespace GuardiansOfRedemption.Content.Guardian.Weapons.Gauntlets
{
	public class DaerelGauntlet : OrchidModGuardianGauntlet
	{
		public override void SafeSetDefaults()
		{
			Item.width = 32;
			Item.height = 30;
			Item.knockBack = 5f;
			Item.damage = 90;
			Item.value = Item.sellPrice(0, 35);
			Item.rare = ItemRarityID.Orange;
			Item.useTime = 6;
			StrikeVelocity = 15f;
			ParryDuration = 60;
			PunchSpeed = 2f;
		}

		public override void OnParryGauntlet(Player player, OrchidGuardian guardian, Entity aggressor, Projectile anchor)
		{
			player.AddBuff(BuffID.Swiftness, 600);	
		}

        public override void ExtraAIGauntlet(Player player, OrchidGuardian guardian, Projectile anchor, bool offHandGauntlet)
        {
            base.ExtraAIGauntlet(player, guardian, anchor, offHandGauntlet);
        }
	}
}
