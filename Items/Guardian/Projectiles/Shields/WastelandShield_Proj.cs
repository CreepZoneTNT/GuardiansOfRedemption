using System;
using System.Collections.Generic;
using GuardiansOfRedemption.General;
using GuardiansOfRedemption.General.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod;
using OrchidMod.Content.Guardian;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace GuardiansOfRedemption.Items.Guardian.Projectiles.Shields;

public class WastelandShield_Proj : OrchidModGuardianProjectile
{

    public override void SafeSetDefaults()
    {
        Projectile.width = 30;
        Projectile.height = 10;
        Projectile.penetrate = 2;
        Projectile.hostile = false;
        Projectile.friendly = true;
        Projectile.tileCollide = true;
        Projectile.ignoreWater = true;
        Projectile.timeLeft = 30;
        Projectile.scale = 1f;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 10;
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
        Collision.HitTiles(Projectile.position, oldVelocity, Projectile.width, Projectile.height);
        return true;
    }
    public override void OnKill(int timeLeft)
    {
        for (int i = 0; i < 6; i++)
        {
            int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke,
                -Projectile.velocity.X * 0.1f, -Projectile.velocity.Y * 0.1f);
            Main.dust[d].noGravity = true;
        }
        for (int i = 0; i < 3; i++)
        {
            int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.FlameBurst,
                -Projectile.velocity.X * 0.1f, -Projectile.velocity.Y * 0.1f);
            Main.dust[d].noGravity = true;
        }
    }
}