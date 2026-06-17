using OrchidMod.Content.Guardian;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace GuardiansOfRedemption.Content.Guardian.Projectiles.Shields
{
    internal class SkeletonWardenShield_BrokenShieldProj : OrchidModGuardianProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 90;
            Projectile.localNPCHitCooldown = 120;
            Projectile.rotation = Main.rand.Next();
        }

        public override void AI()
        {
            Projectile.velocity *= 0.95f;
        }

        public override void OnKill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item127, Projectile.position);
            int ironDust = Dust.NewDust(Projectile.Center - new Vector2(4, 4), 0, 0, DustID.Iron);
            int woodDust = Dust.NewDust(Projectile.Center - new Vector2(4, 4), 0, 0, DustID.t_BorealWood);
        }
    }
}
