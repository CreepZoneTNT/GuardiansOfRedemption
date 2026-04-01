using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using OrchidMod.Content.Guardian;
using Redemption.BaseExtension;
using Redemption.Items.Materials.HM;
using GuardiansOfRedemption.General;
using GuardiansOfRedemption.Projectiles.Quarterstaves;
using OrchidMod;
using static Terraria.Player;

namespace GuardiansOfRedemption.Items.Weapons.Quarterstaves;
public class UraniumQuarterstaff : OrchidModGuardianQuarterstaff {

    public Vector2 tip;

    public override void SafeSetDefaults() {
        Item.width = 52;
        Item.height = 58;
        Item.value = Item.sellPrice(0, 6);
        Item.rare = ItemRarityID.Lime;
        Item.useTime = 32;
        Item.knockBack = 5f;
        Item.damage = 235;
        SlamStacks = 2;
        GuardStacks = 1;
        ParryDuration = 160;
    }

    public override void ExtraAIQuarterstaff(Player player, OrchidGuardian guardian, Projectile projectile) {

        tip = (projectile.ModProjectile as GuardianQuarterstaffAnchor).GetQuarterstaffTip(0.4f);
    }

    public override void OnHitFirst(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, NPC.HitInfo hit, bool jabAttack, bool counterAttack)
    {
        if (counterAttack)
        {
            Projectile.NewProjectileDirect(projectile.GetSource_FromAI(), target.Center, Vector2.Zero, ModContent.ProjectileType<UraniumQuarterstaff_RealisticExplosionProj>(), guardian.GetGuardianDamage(Item.damage * 1.5f), 16f, projectile.owner);
            player.RedemptionScreen().ScreenShakeIntensity = 6f;
        }
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<Uranium>(10)
            .AddIngredient<Plating>(4)
            .AddIngredient<Capacitor>(2)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }

}