using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Content.Guardian;
using OrchidMod.Utilities;
using Redemption.Globals;
using GuardiansOfRedemption.Items.Guardian.Weapons.Runes;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Redemption.NPCs.Friendly.TownNPCs;

namespace GuardiansOfRedemption.Projectiles.Runes
{
    public class KS3Rune_Proj : GuardianRuneProjectile
    {
        public int TimeSpent = 0;

        public List<Vector2> OldPosition;
        public List<float> OldRotation;
        public bool ShouldDrawTrail = false;

        public override void SetStaticDefaults()
        {
            ElementID.ProjThunder[Type] = true;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void RuneSetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 50;
            Projectile.friendly = true;
            Projectile.aiStyle = 0;
            Projectile.alpha = 60;
            Projectile.scale = 1f;
            Projectile.penetrate = -1;
            OldPosition = new List<Vector2>();
            OldRotation = new List<float>();
            ShouldDrawTrail = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Main.projFrames[Projectile.type] = 14;
        }
        
        public override bool? CanCutTiles()
        {
            return false;
        }

        public override void FirstFrame()
        {
            if (Projectile.ai[2] != 0) TimeSpent += 60;
        }

        public override bool SafeAI()
        {
            SetDistance(KS3Rune.Boosted ? 120 + (float)Math.Sin(TimeSpent * (MathHelper.Pi / 120f)) * 80f : 90 + (float)Math.Sin(TimeSpent * (MathHelper.Pi / 120f)) * 50f);
            
            TimeSpent++;

            for (int i = 0; i < OldPosition.Count; i++)
            {
                Vector2 pos = OldPosition[i];
                pos.Y -= 4f;
                OldPosition[i] = pos;
            }

            OldPosition.Add(Projectile.Center + new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-3f, 3f)));
            OldRotation.Add(Projectile.rotation + Main.rand.NextFloat(MathHelper.Pi));

            if (OldPosition.Count > 13)
            {
                OldPosition.RemoveAt(0);
                OldRotation.RemoveAt(0);
            }

            if (++Projectile.frameCounter >= 3)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Type])
                    Projectile.frame = 0;
            }

            return true;
        }
        public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            SpriteEffects effects = (Projectile.velocity.X < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None);

            Rectangle frame = texture.Frame(1, 14, 0, Projectile.frame);

            spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, frame, lightColor, Projectile.rotation, frame.Size() * 0.5f, Projectile.scale, effects, 0f);            
            return false;
        }
    }
}