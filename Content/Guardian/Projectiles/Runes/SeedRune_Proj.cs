using Microsoft.Xna.Framework;
using OrchidMod.Content.Guardian;
using Redemption.Globals;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace GuardiansOfRedemption.Content.Guardian.Projectiles.Runes
{
    public class SeedRune_Proj : GuardianRuneProjectile
    {
        public int TimeSpent = 0;

        public override void SetStaticDefaults()
        {
            ElementID.ProjPoison[Type] = true;
        }
        public override void RuneSetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 600;
            Projectile.scale = 1f;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
            Main.projFrames[Projectile.type] = 4;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 8;
        }

        public override void FirstFrame()
        {
            Projectile.frame = Main.rand.Next(4);
        }
        public override bool SafeAI()
        {
            Spin(4f);

            TimeSpent++;
            SetDistance(50 + (float)Math.Sin(TimeSpent * (MathHelper.Pi / 120f)) * 15f);
            Projectile.rotation += 0.05f;
            Lighting.AddLight(Projectile.Center, 1f, 0.6f, 0f);

            if (Main.rand.NextBool(20))
            {
                Dust.NewDust(Projectile.Center, Projectile.width / 2, Projectile.height / 2, DustID.GreenFairy, Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1), 0, Color.White, Main.rand.NextFloat(0.2f, 0.5f));
            }
            return true;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item177);
        }
    }
}
