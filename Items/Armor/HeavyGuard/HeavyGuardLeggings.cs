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
	public class HeavyGuardLeggings : OrchidModEquippable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 26;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 3, 0, 0);
			Item.rare = ItemRarityID.Orange;
			Item.defense = 6;
		}

		public override void UpdateEquip(Player player)
		{
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
      modPlayer.GuardianGuardRecharge += 0.4f;
			modPlayer.GuardianSlamRecharge += 0.4f;
      player.moveSpeed += 0.1f;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient<Archcloth>, 3);
      recipe.AddIngredient<GraveSteelAlloy>, 20);
			recipe.AddTile(TileID.Sawmill);
			recipe.Register();
		}
	}
}
