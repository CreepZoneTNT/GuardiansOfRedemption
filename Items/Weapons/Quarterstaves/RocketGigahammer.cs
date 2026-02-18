using OrchidMod;
using OrchidMod.Content.Guardian;
using Redemption.Items.Materials.HM;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Items.Weapons.Quarterstaves
{
	public class RocketGigahammer : OrchidModGuardianQuarterstaff {
		
		public override void SafeSetDefaults()
		{
			Item.width = 148;
			Item.height = 118;
			Item.value = Item.sellPrice(0, 8);
			Item.rare = ItemRarityID.Yellow;
			Item.useTime = 54;
			ParryDuration = 120;
			Item.knockBack = 12f;
			Item.damage = 515;
			GuardStacks = 3;
			SlamStacks = 1;
			SwingSpeed = 0.5f;
			JabSpeed = 0.8f;
			JabStyle = 1;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient<OmegaPowerCell>()
			.AddIngredient<CorruptedXenomite>(8)
			.AddIngredient<CarbonMyofibre>(6)
			.AddIngredient<Plating>(6)
			.AddTile(TileID.MythrilAnvil)
			.Register();
		}
	}
}
