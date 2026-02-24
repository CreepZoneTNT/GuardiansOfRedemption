using GuardiansOfRedemption.General.Global;
using OrchidMod;
using OrchidMod.Content.Guardian;
using Redemption.BaseExtension;
using Redemption.Globals;
using Redemption.Items.Armor.HM.Hardlight;
using Redemption.Items.Materials.PreHM;
using Redemption.NPCs.Lab.Janitor;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;


namespace GuardiansOfRedemption.Items.Armor.Hardlight
{
	[AutoloadEquip(EquipType.Head)]
	public class HardlightGreathelm : OrchidModGuardianEquipable
	{
		public static LocalizedText SetBonusText { get; private set; }

		public override void SetStaticDefaults()
		{
			SetBonusText = this.GetLocalization("SetBonus");
		}

		public override void SafeSetDefaults()
		{
			Item.width = 24;
			Item.height = 22;
			Item.value = Item.sellPrice(0, 0, 75);
			Item.rare = ItemRarityID.LightPurple;
			Item.defense = 24;
		}

		public override void UpdateEquip(Player player)
		{
		
			player.GetDamage<GuardianDamageClass>() += 0.13f;
			player.GetCritChance<GuardianDamageClass>() += 5;
			
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
			modPlayer.GuardianGuardMax++;
			modPlayer.GuardianSlamMax++;
			player.aggro += 100;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<HardlightPlate>() && legs.type == ModContent.ItemType<HardlightBoots>();
		}

		public override void UpdateArmorSet(Player player)
		{
			RedemptionGuardian modPlayer = player.GetModPlayer<RedemptionGuardian>();
			// Code borrowed from Redemption
			player.setBonus = Language.GetTextValue("Mods.Redemption.GenericTooltips.ArmorSetBonus.Hardlight.Keybind");
			if (!Main.dedServ)
			{
				foreach (string assignedKey in Redemption.Redemption.RedeSpecialAbility.GetAssignedKeys())
					player.setBonus = Language.GetTextValue("Mods.Redemption.GenericTooltips.ArmorSetBonus.Hardlight.Press") + assignedKey + Language.GetTextValue("Mods.Redemption.GenericTooltips.ArmorSetBonus.Hardlight.Support") + SetBonusText;
			}
			modPlayer.GuardianHardlight = true;
			player.RedemptionPlayerBuff().MetalSet = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient<Archcloth>(3)
		.AddIngredient<GraveSteelAlloy>(15)
			.AddTile(TileID.Anvils)
			.Register();
		}
	}
}
