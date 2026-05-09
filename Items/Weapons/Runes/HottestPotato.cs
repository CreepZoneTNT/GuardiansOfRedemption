using GuardiansOfRedemption.Projectiles.Runes;
using Microsoft.Xna.Framework;
using OrchidMod;
using OrchidMod.Content.Guardian;
using OrchidMod.Content.Guardian.Misc;
using Redemption;
using Redemption.BaseExtension;
using Redemption.Globals;
using Redemption.Items.Materials.HM;
using Redemption.Items.Materials.PostML;
using Redemption.Projectiles.Magic;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Items.Weapons.Runes
{
    public class HottestPotato : OrchidModGuardianRune
    {
        public Player holder;
        public override void SetStaticDefaults()
        {
            ElementID.ItemExplosive[Type] = true;
        }
        public override void SafeSetDefaults()
        {
            Item.damage = 1;
            Item.DamageType = ModContent.GetInstance<GuardianDamageClass>();
            Item.width = 22;
            Item.height = 38;
            Item.useTime = 20;

            Item.knockBack = 0f;
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.sellPrice(0, 50, 0, 0);
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
