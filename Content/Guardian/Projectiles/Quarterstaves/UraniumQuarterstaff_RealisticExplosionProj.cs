using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace GuardiansOfRedemption.Content.Guardian.Projectiles.Quarterstaves;
public class UraniumQuarterstaff_RealisticExplosionProj : ModProjectile {
    public override void SetStaticDefaults() {
        Main.projFrames[Projectile.type] = 17;
    }

    public override void SetDefaults() {
        Projectile.width = 142;
        Projectile.height = 142;
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.penetrate = -1;
    }

    public override void OnSpawn(IEntitySource source) {
        Projectile.Resize(200, 200);
        SoundEngine.PlaySound(new SoundStyle("GuardiansOfRedemption/Sounds/DeltaruneExplosion"), Projectile.Center);
    }


    public override void AI() {
        Projectile.velocity = Vector2.Zero;
        if (++Projectile.frameCounter > 2) {
            Projectile.frameCounter = 0;
            if (++Projectile.frame >= Main.projFrames[Projectile.type]) Projectile.Kill();
        }
    }

    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {
        target.AddBuff(BuffID.OnFire3, 300);
    }
}