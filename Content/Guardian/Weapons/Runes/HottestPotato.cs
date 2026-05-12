using GuardiansOfRedemption.Content.Guardian.Projectiles.Runes;
using OrchidMod.Content.Guardian;
using Redemption;
using Redemption.Globals;
using Redemption.Items.Materials.HM;
using Redemption.Items.Materials.PostML;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Content.Guardian.Weapons.Runes
{
    public class HottestPotato : OrchidModGuardianRune
    {
        public override void SetStaticDefaults()
        {
            ElementID.ItemExplosive[Type] = true;
        }

        public override void SafeSetDefaults()
        {
            Item.damage = 1;
            Item.width = 22;
            Item.height = 38;
            Item.useTime = 20;

            Item.knockBack = 0f;
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.sellPrice(0, 50);
            Item.UseSound = CustomSounds.AlarmItem;
            Item.shoot = ModContent.ProjectileType<HottestPotato_Projectile>();

            RuneCost = 2;
            RuneNumber = 4;
            RuneDuration = 45 * 60;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Plutonium>(), 12)
                .AddIngredient(ModContent.ItemType<Uranium>(), 25)
                .AddIngredient(ModContent.ItemType<Plating>(), 10)
                .AddIngredient(ModContent.ItemType<Capacitor>(), 2)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}
