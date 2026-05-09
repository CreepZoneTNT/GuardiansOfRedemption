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
    internal class SeedRune : OrchidModGuardianRune
    {
        public override void SetStaticDefaults()
        {
            ElementID.ItemPoison[Type] = true;

        }
        public override void SafeSetDefaults()
        {
            Item.damage = 28;
            Item.DamageType = ModContent.GetInstance<GuardianDamageClass>();
            Item.width = 24;
            Item.height = 20;

            Item.useTime = 15;
            Item.knockBack = 0f;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(0, 0, 20, 0);
            Item.UseSound = SoundID.Item177;    
            Item.shoot = ModContent.ProjectileType<SeedRune_Proj>();

            RuneDistance = 70f;
            RuneCost = 2;
            RuneNumber = 6;
            RuneDuration = 20 * 60;
        }
    }
}
