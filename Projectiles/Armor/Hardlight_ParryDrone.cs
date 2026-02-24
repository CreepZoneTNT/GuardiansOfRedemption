using System;
using GuardiansOfRedemption.General.Global;
using Microsoft.Xna.Framework;
using OrchidMod;
using OrchidMod.Content.Guardian;
using OrchidMod.Content.Guardian.Projectiles.Quarterstaves;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;

namespace GuardiansOfRedemption.Projectiles.Armor;

public class Hardlight_ParryDrone : OrchidModGuardianProjectile
{

    public OrchidGuardian guardian => Owner.GetModPlayer<OrchidGuardian>();
    public RedemptionGuardian addonGuardian => Owner.GetModPlayer<RedemptionGuardian>();
    
    public override void SetStaticDefaults()
    {
        Main.projFrames[Type] = 4;
    }
    
    public override void SafeSetDefaults()
    {
        Projectile.width = 26;
        Projectile.height = 54;
        Projectile.penetrate = -1;
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.timeLeft = 900;
    }

    public override void AI()
    {
        if (++Projectile.frameCounter >= 6)
        {
            Projectile.frameCounter = 0;
            if (++Projectile.frame >= 4) Projectile.frame = 0;
        }
        if (Projectile.soundDelay == 0)
        {
            SoundEngine.PlaySound(SoundID.Item24 with {Volume = 0.25f}, Projectile.position);
            Projectile.soundDelay = 10;
        }
        
        if (Projectile.timeLeft > 120)
        {
            int count = 0; // nb of more recent projectiles
            int countTotal = 0; // nb of other projectiles
            foreach (Projectile projectile in Main.projectile)
            {
                if (projectile.active && projectile.ModProjectile is Hardlight_ParryDrone && projectile.owner == Main.myPlayer)
                {
                    countTotal++;
                    if (projectile.timeLeft > Projectile.timeLeft) count++;
                }
            }

            Vector2 targetPosition = Owner.Center - Vector2.UnitY.RotatedBy(Projectile.timeLeft * 0.02f + MathHelper.TwoPi / countTotal * count) * (16f + Math.Max(Owner.width, Owner.height));
            Projectile.velocity = (targetPosition - Projectile.Center) * 0.1f + Owner.velocity;
        }
        else
        {
            Projectile.velocity.Y -= 0.4f;
            Projectile.velocity.X *= 0.9f;
            Projectile.localAI[0] = 1;

        }
        
        
        if (Owner.dead) Projectile.Kill();
    }

    public override void OnKill(int timeLeft)
    {
        for (int i = 0; i < 7; i++) Dust.NewDustDirect(Projectile.Center, 10, 10, DustID.Vortex, Main.rand.NextFloat(-2f, 2f), -2f);
        SoundEngine.PlaySound(SoundID.NPCDeath44, Projectile.position);
    }
}