using GuardiansOfRedemption.Buffs;
using OrchidMod;
using OrchidMod.Content.Guardian;
using Redemption;
using Redemption.BaseExtension;
using Redemption.Globals;
using Redemption.Rarities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace GuardiansOfRedemption.Items.Armor.HeavyGuard
{
	[AutoloadEquip(EquipType.Body)]
	public class GuardianHeavyGuardChest : OrchidModEquippable
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

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient<Archcloth>, 3);
      recipe.AddIngredient<GraveSteelAlloy>, 25);
			recipe.AddTile(TileID.Sawmill);
			recipe.Register();
		}
	}
}
