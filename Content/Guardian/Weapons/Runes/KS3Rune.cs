using GuardiansOfRedemption.Content.Guardian.Projectiles.Runes;
using OrchidMod;
using OrchidMod.Content.Guardian;
using Redemption.Globals;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Content.Guardian.Weapons.Runes
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
