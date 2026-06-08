using Microsoft.Xna.Framework;
using OrchidMod;
using OrchidMod.Content.Guardian;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Redemption.Buffs.Debuffs;
using Redemption.Items.Materials.HM;

namespace GuardiansOfRedemption.Content.Guardian.Weapons.Katars
{
    internal class XenomiteKatar : OrchidModGuardianKatar
    {
        public override void SafeSetDefaults()
        {
            Item.width = 34;
            Item.height = 34;
            Item.knockBack = 6f;
            Item.damage = 220;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.useTime = 20;
            JabVelocity = 25f;
            ParryDuration = 20;
        }

        public override Color GetColor()
        {
            return new Color(54, 193, 59);
        }

        public override void OnDashKatar(Player player, OrchidGuardian guardian, Projectile anchor)
        {
        }

        public override void OnHit(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, NPC.HitInfo hit, bool fullyCharged)
        {
            if(Main.rand.NextBool(3))
                target.AddBuff(ModContent.BuffType<GreenRashesDebuff>(), 2500);
        }

        public override void AddRecipes()
        {
            var recipe = CreateRecipe();
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.AddIngredient(ModContent.ItemType<Xenomite>(), 12);
            recipe.AddIngredient(ModContent.ItemType<ToxicBile>(), 4);
            recipe.AddIngredient(ItemID.SoulofMight, 18);
            recipe.Register();
        }
    }
}
