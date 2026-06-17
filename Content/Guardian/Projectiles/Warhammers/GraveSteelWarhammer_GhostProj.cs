using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Content.Guardian;
using Redemption.BaseExtension;
using Redemption.Globals;
using Redemption.Particles;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;

namespace GuardiansOfRedemption.Content.Guardian.Projectiles.Warhammers;

public class GraveSteelWarhammer_GhostProj : OrchidModGuardianProjectile
{

    public List<Vector2> OldPosition;
    public List<float> OldRotation;

    public bool ShouldDrawTrail = false;

    public override void SetStaticDefaults() => ElementID.ProjThunder[Type] = true;
    
    public override void SafeSetDefaults()
    {
        Projectile.width = 36;
        Projectile.height = 30;
        Projectile.friendly = true;
        Projectile.tileCollide = false;
        Projectile.penetrate = -1;
        Projectile.timeLeft = 90;
        Projectile.light = 0.5f;
        Projectile.localNPCHitCooldown = 40;
        OldPosition = [];
        OldRotation = [];
        ShouldDrawTrail = true;
        Projectile.Redemption().IsHammer = false;
    }

    public override void AI()
    {
        RedeParticleManager.CreateSpiritParticle(Projectile.RandAreaInEntity() + (Projectile.velocity * 3), Vector2.Zero, 0.4f * Projectile.scale, Main.rand.Next(20, 30));


        if (Projectile.timeLeft > 60)
        {
            Projectile.rotation += 0.3f;
            Projectile.velocity *= 0.98f;
        }

        if (Projectile.timeLeft == 60)
        {

            Projectile.damage *= 6;
            NPC closestTarget = null;
            float distanceClosest = 500f;
            foreach (NPC npc in Main.npc)
            {
                float distance = Projectile.Center.Distance(npc.Center);
                if (IsValidTarget(npc) && distance < distanceClosest)
                {
                    closestTarget = npc;
                    distanceClosest = distance;
                }
            }

            if (closestTarget != null)
            {
                SoundEngine.PlaySound(SoundID.DD2_GhastlyGlaiveImpactGhost);
                Vector2 newVelocity = Vector2.Normalize(closestTarget.Center - Projectile.Center) * 10f;
                Projectile.velocity = newVelocity;
                Projectile.rotation = Projectile.velocity.ToRotation();
            }         
        }

        if (Projectile.timeLeft < 50)
        {
            Projectile.velocity *= 0.975f;
            Projectile.alpha += 5;
        }

        if (Projectile.alpha >= 255) Projectile.Kill();


        if (ShouldDrawTrail)
        {
            OldPosition.Add(Projectile.Center);
            OldRotation.Add(Projectile.rotation);

            if (OldPosition.Count > 10)
            {
                OldPosition.RemoveAt(0);
                OldRotation.RemoveAt(0);
            }
        }
    }
    public override void OnKill(int timeLeft)
    {        
        for (int i = 0; i < 10; i++)
        {
            RedeParticleManager.CreateSpiritParticle(Projectile.RandAreaInEntity() + (Projectile.velocity * 10), Projectile.velocity, 0.2f * Projectile.scale, Main.rand.Next(10, 20));
        }
    }
    public override Color? GetAlpha(Color lightColor)
    {
        return new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0) * Projectile.Opacity;
    }
    
    public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
    {
        Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

        for (int i = 0; i < OldPosition.Count; i++)
        {
            Vector2 drawPosition = OldPosition[i] - Main.screenPosition;
            Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length * i + 10) / (float)Projectile.oldPos.Length);
            spriteBatch.Draw(texture, drawPosition, null, color, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale * (i + 1) * 0.1f, SpriteEffects.None, 0);        
        }
        return true;
    }

}