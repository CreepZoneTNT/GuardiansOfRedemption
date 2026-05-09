using GuardiansOfRedemption.Projectiles.Runes;
using Microsoft.Xna.Framework;
using OrchidMod;
using OrchidMod.Content.Guardian;
using OrchidMod.Content.Guardian.Misc;
using OrchidMod.Utilities;
using Redemption;
using Redemption.BaseExtension;
using Redemption.Globals;
using Redemption.Items.Materials.HM;
using Redemption.Items.Materials.PostML;
using Redemption.Items.Materials.PreHM;
using Redemption.Projectiles.Magic;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Items.Guardian.Weapons.Runes
{
    public class SeedRune : OrchidModGuardianRune
    {
        public override void SetStaticDefaults()
        {
            ElementID.ItemPoison[Type] = true;
            ElementID.ItemArcane[Type] = true;
        }
        public override void SafeSetDefaults()
        {
            Item.damage = 60;
            Item.width = 24;
            Item.height = 20;
            Item.useTime = 36;

            Item.knockBack = 3f;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(0, 0, 20);
            Item.UseSound = SoundID.Item110;    
            Item.shoot = ModContent.ProjectileType<SeedRune_Projectile>();

            RuneCost = 3;
            RuneNumber = 4;
            RuneAmountScaling = 2;
            RuneDuration = 24 * 60;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<XenomiteShard>(), 16)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
