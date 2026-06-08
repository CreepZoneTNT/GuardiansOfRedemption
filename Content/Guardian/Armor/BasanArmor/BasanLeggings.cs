using GuardiansOfRedemption.Content.Other.Materials;
using OrchidMod;
using OrchidMod.Content.Guardian;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Content.Guardian.Armor.BasanArmor
{
	[AutoloadEquip(EquipType.Legs)]
	public class BasanLeggings : OrchidModGuardianEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Orange;
			Item.defense = 7;
		}

		public override void UpdateEquip(Player player)
		{
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
			player.moveSpeed += 0.15f;
            player.GetAttackSpeed(DamageClass.Melee) += 0.08f;
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
