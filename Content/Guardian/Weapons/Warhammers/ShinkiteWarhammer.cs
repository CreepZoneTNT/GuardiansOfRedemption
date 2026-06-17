using Microsoft.Xna.Framework;
using OrchidMod;
using OrchidMod.Content.Guardian;
using Redemption.Dusts.Tiles;
using Redemption.Globals;
using Redemption.Rarities;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Content.Guardian.Weapons.Warhammers;

public class ShinkiteWarhammer : OrchidModGuardianHammer
{

    public override void SetStaticDefaults()
    { 
        ElementID.ItemFire[Type] = true;
        ElementID.ItemShadow[Type] = true;
        ElementID.ItemExplosive[Type] = true;
    }

    public override void SafeSetDefaults()
    {
        Item.width = 58;
        Item.height = 58;
        Item.value = Item.sellPrice(20, 0);
        Item.rare = ModContent.RarityType<TurquoiseRarity>();
        Item.UseSound = SoundID.Item1;
        Item.knockBack = 12f;
        Item.shootSpeed = 12f;
        Item.damage = 666;
        Item.useTime = 40;
        Range = 100;
        SlamStacks = 3;
        GuardStacks = 2;
        BlockDuration = 300;
        Penetrate = true;
        TileBounce = true;
        HitCooldown = 60;
        HoldOffset = 12f;
    }

    public override void ExtraAI(Player player, OrchidGuardian guardian, Projectile projectile, bool OffHand) 
    {
        if (projectile.ModProjectile is GuardianHammerAnchor anchor) {
            if (projectile.timeLeft < 598 && anchor.range > 0 && anchor.BlockDuration == 0) {
                NPC HomingTarget = projectile.FindTargetWithinRange(480f, true);
                
                if (HomingTarget != null && HomingTarget.active) 
                    projectile.velocity = Vector2.UnitX.RotatedBy(projectile.velocity.ToRotation().AngleTowards(projectile.AngleTo(HomingTarget.Center), MathHelper.Pi/40)) * projectile.velocity.Length();
            }
        }
    }


    public override void OnMeleeHitFirst(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, float knockback, bool crit, bool Weak, bool OffHand)
    {
        SoundEngine.PlaySound(SoundID.Item14);
        target.immune[player.whoAmI] = 40;
        RedeHelper.NPCRadiusDamage(96, projectile, guardian.GetGuardianDamage(Item.damage * 0.8f), 10f, 30);
        for (int i = 0; i < 10; i++) {
            Vector2 pos = Main.rand.NextVector2CircularEdge(6f, 6f);
            Dust dust1 = Dust.NewDustPerfect(target.Center + pos, ModContent.DustType<ShinkiteDust>(), -pos * 0.4f);
            dust1.noGravity = true;
            Dust dust2 = Dust.NewDustPerfect(target.Center, DustID.DemonTorch, pos * 0.4f);
            dust2.noGravity = true;
        }
    }
}
