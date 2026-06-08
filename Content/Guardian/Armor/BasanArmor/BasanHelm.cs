using GuardiansOfRedemption.General.Global;
using GuardiansOfRedemption.Content.Other.Materials;
using OrchidMod;
using OrchidMod.Content.Guardian;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace GuardiansOfRedemption.Content.Guardian.Armor.BasanArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class BasanHelm : OrchidModGuardianEquipable
    {
        //public static LocalizedText SetBonusText { get; private set; }

        public override void SetStaticDefaults()
        {
            ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
        }  

        public override void SafeSetDefaults()
        {
            Item.width = 22;
            Item.height = 24;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 6;
        }

        public override void UpdateEquip(Player player)
        {
            OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
            player.GetDamage<GuardianDamageClass>() += 0.08f;
            player.aggro += 300;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<BasanChest>() && legs.type == ModContent.ItemType<BasanLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            RedemptionGuardian modPlayer = player.GetModPlayer<RedemptionGuardian>();
            player.GetDamage<GuardianDamageClass>() += 0.05f;
            modPlayer.GuardianBasan = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.AshWood, 25)
                .AddIngredient(ItemID.Bone, 25)
                .AddIngredient(ModContent.ItemType<BasanMaterial>(), 6)
                .AddTile(TileID.Hellforge)
                .Register();
        }
    }
}
