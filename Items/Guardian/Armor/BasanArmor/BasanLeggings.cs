using GuardiansOfRedemption.Buffs;
using GuardiansOfRedemption.Items.Other.Materials;
using OrchidMod;
using OrchidMod.Content.Guardian;
using rail;
using Redemption;
using Redemption.BaseExtension;
using Redemption.Globals;
using Redemption.Items.Materials.PreHM;
using Redemption.Rarities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace GuardiansOfRedemption.Items.Guardian.Armor.BasanArmor
{
	[AutoloadEquip(EquipType.Legs)]
	public class BasanLeggings : OrchidModGuardianEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 22;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Orange;
			Item.defense = 10;
		}

		public override void UpdateEquip(Player player)
		{
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
			player.moveSpeed += 0.1f;
			player.jumpSpeedBoost += 1.6f;
		}
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.AshWood, 20)
                .AddIngredient(ItemID.Bone, 20)
                .AddIngredient(ModContent.ItemType<BasanMaterial>(), 6)
                .AddTile(TileID.Hellforge)
                .Register();
        }
    }
}
