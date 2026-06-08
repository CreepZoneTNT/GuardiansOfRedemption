using OrchidMod;
using OrchidMod.Content.Guardian;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using GuardiansOfRedemption.Content.Other.Materials;

namespace GuardiansOfRedemption.Content.Guardian.Armor.BasanArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class BasanChest : OrchidModGuardianEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 34;
			Item.height = 22;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Orange;
			Item.defense = 8;
		}

		public override void UpdateEquip(Player player)
		{
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
            player.GetDamage<GuardianDamageClass>() += 0.08f;
			player.GetAttackSpeed(DamageClass.Melee) += 0.08f;
            player.moveSpeed += 0.15f;
            modPlayer.GuardianSlamMax += 2;
		}

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.AshWood, 35)
                .AddIngredient(ItemID.Bone, 35)
                .AddIngredient(ModContent.ItemType<BasanMaterial>(), 8)
                .AddTile(TileID.Hellforge)
                .Register();
        }
    }
}
