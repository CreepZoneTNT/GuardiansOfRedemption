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
    public class MoonflareRune : OrchidModGuardianRune
    {
        public static bool Boosted = false;
        public override void SetStaticDefaults()
        {
            ElementID.ItemFire[Type] = true;
            ElementID.ItemNature[Type] = true;
            ElementID.ItemArcane[Type] = true;
        }
        public override void SafeSetDefaults()
        {
            Item.damage = 40;
            Item.width = 30;
            Item.height = 34;
            Item.useTime = 50;

            Item.knockBack = 3f;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 8, 0);
            Item.UseSound = SoundID.Item176;    
            Item.shoot = ModContent.ProjectileType<MoonflareRune_Projectile>();

            RuneCost = 2;
            RuneNumber = 1;
            RuneDuration = 24 * 60;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);

            int index = tooltips.FindIndex(ttip => ttip.Mod == "Terraria" && ttip.Name == "Tooltip1");
            string text = "There is no moonlight to reflect...";
            if (Main.dayTime || Main.moonPhase == 4)
            {
                TooltipLine line = new(Mod, "text", text)
                {
                    OverrideColor = Color.LightGray
                };
                tooltips.Insert(2, line);
            }
        }

        public override void Activate(Player player, OrchidGuardian guardian, int type, int damage, float knockback, int critChance, int duration, float distance, int amount)
        {
            if (amount > 1)
            { Boosted = true; }
            else
            { Boosted = false; }

            NewRuneProjectiles(player, guardian, duration, type, damage + (amount * 8), knockback, critChance, distance, 1, 90f);

        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<MoonflareFragment>(), 10)
                .AddTile(TileID.Anvils)
                .AddCondition(RedeConditions.InMoonlight)
                .Register();
        }
    }
}
