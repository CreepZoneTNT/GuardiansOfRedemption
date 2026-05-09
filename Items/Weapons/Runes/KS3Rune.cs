using GuardiansOfRedemption.Projectiles.Runes;
using GuardiansOfRedemption.Projectiles.Shields;
using JetBrains.Annotations;
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

namespace GuardiansOfRedemption.Items.Weapons.Runes
{
    public class KS3Rune : OrchidModGuardianRune
    {
        public static bool Boosted = false;
        public override void SetStaticDefaults()
        {
            ElementID.ItemThunder[Type] = true;

        }
        public override void SafeSetDefaults()
        {
            Item.damage = 125;
            Item.DamageType = ModContent.GetInstance<GuardianDamageClass>();
            Item.width = 24;
            Item.height = 40;
            Item.useTime = 30;

            Item.knockBack = 10f;
            Item.rare = ItemRarityID.LightPurple;
            Item.value = Item.sellPrice(0, 6, 0, 0);
            Item.UseSound = SoundID.Item176;    
            Item.shoot = ModContent.ProjectileType<KS3Rune_Proj>();

            RuneCost = 3;
            RuneNumber = 2;
            RuneDuration = 45 * 60;
        }
        public override void Activate(Player player, OrchidGuardian guardian, int type, int damage, float knockback, int critChance, int duration, float distance, int amount)
        {
            int damageEqualAmount;

            if (amount > 2)
            { damageEqualAmount = damage + (amount * 20); Boosted = true; }
            else
            { damageEqualAmount = damage; Boosted = false; }
            
            NewRuneProjectiles(player, guardian, duration, type, damageEqualAmount, knockback, critChance, distance, 2, 90f);
        }
    }
}
