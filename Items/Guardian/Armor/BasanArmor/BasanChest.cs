using GuardiansOfRedemption.Buffs;
using GuardiansOfRedemption.Items.Other.Materials;
using OrchidMod;
using OrchidMod.Content.Guardian;
using OrchidMod.Content.Shapeshifter.Misc;
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
	[AutoloadEquip(EquipType.Body)]
	public class BasanChest : OrchidModGuardianEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 34;
			Item.height = 22;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Orange;
			Item.defense = 12;
		}

		public override void UpdateEquip(Player player)
		{
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
            player.GetDamage<GuardianDamageClass>() += 0.08f;
			player.GetAttackSpeed(DamageClass.Melee) += 0.08f;
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
