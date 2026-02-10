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
	[AutoloadEquip(EquipType.Head)]
	public class HeavyGuardHelmet : OrchidModGuardianEquipable
	{
		public static LocalizedText SetBonusText { get; private set; }

		public override void SetStaticDefaults()
		{
			SetBonusText = this.GetLocalization("SetBonus");
		}

		public override void SafeSetDefaults()
		{
			Item.width = 24;
			Item.height = 26;
			Item.value = Item.sellPrice(0, 3, 0, 0);
			Item.rare = ItemRarityID.Orange;
			Item.defense = 6;
		}

		public override void UpdateEquip(Player player)
		{
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
			modPlayer.GuardianGuardMax++;
			player.aggro += 250;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ItemType<HeavyGuardChestplate>() && legs.type == ItemType<HeavyGuardLeggings>();
		}

		public override void UpdateArmorSet(Player player)
		{
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
			player.setBonus = SetBonusText.Value;
			modPlayer.GuardianHeavyGuard = true;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(<Archcloth>, 3);
      recipe.AddIngredient(<GraveSteelAlloy>, 15);
			recipe.AddTile(TileID.Anvil);
			recipe.Register();
		}
	}
}
