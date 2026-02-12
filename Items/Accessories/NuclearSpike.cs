using GuardiansOfRedemption.General.Global;
using Terraria;
using Terraria.ID;
using OrchidMod;
using OrchidMod.Content.Guardian;
using OrchidMod.Content.Guardian.Accessories;
using Redemption;
using Redemption.BaseExtension;
using Redemption.Globals;
using Redemption.Items.Materials.PostML;
using Redemption.Rarities;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Items.Accessories
{
	public class NuclearSpike : OrchidModGuardianEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 20, 0, 0);
			Item.rare = ModContent.RarityType<TurquoiseRarity>();
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
			MoRGuardianPlayer addonPlayer = player.GetModPlayer<MoRGuardianPlayer>();
			if (modPlayer.GuardianSpikeDamage < 2.5f)
				modPlayer.GuardianSpikeDamage = 2.5f;
			modPlayer.GuardianSharpRebuttalBlock = true;
			addonPlayer.GuardianSpikeNuclear = true;
			player.GetCritChance<GuardianDamageClass>() += 10;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient<TempleSpike>()
			.AddIngredient<Plutonium>(8)
			.AddTile(TileID.TinkerersWorkbench)
			.Register();
		}
	}
}
