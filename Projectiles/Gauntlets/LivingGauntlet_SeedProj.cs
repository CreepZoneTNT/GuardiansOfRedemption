using Microsoft.Xna.Framework;
using OrchidMod.Content.General.Dusts;
using OrchidMod.Content.Guardian;
using Redemption.Globals;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Projectiles.Gauntlets;

public class LivingGauntlet_SeedProj : OrchidModGuardianProjectile
{
    public override void SetStaticDefaults()
    {
        ElementID.ProjNature[Type] = true;
    } 

    public override void SafeSetDefaults()
    {
        Projectile.width = 14;
        Projectile.height = 14;
        Projectile.friendly = true;
        Projectile.penetrate = -1;
    }

    public override void AI()
    {
        Projectile.ai[0]++;
        if (Projectile.ai[0] > 30)
        {
            Projectile.velocity.Y += 0.4f;
            if (Projectile.velocity.Y > 16f) Projectile.velocity.Y = 16f;
            Projectile.rotation += 0.1f * Projectile.velocity.Y * Projectile.direction;
        }
        else Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
    }

    public override void OnKill(int timeLeft)
    {
        SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
        SoundEngine.PlaySound(SoundID.NPCDeath15, Projectile.Center);
        Dust.NewDustDirect(Projectile.Center, Projectile.width, Projectile.height, DustID.GrassBlades);
        Dust.NewDustDirect(Projectile.Center, Projectile.width, Projectile.height, ModContent.DustType<LeafDust>());
    }
}