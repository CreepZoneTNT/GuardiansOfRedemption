using GuardiansOfRedemption.Content.Guardian.Projectiles.Runes;
using OrchidMod.Content.Guardian;
using Redemption.Globals;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Content.Guardian.Weapons.Runes
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
