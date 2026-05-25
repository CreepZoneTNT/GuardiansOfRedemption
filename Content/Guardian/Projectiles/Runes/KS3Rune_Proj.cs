using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Content.Guardian;
using Redemption.Globals;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using GuardiansOfRedemption.Content.Guardian.Weapons.Runes;

namespace GuardiansOfRedemption.Content.Guardian.Projectiles.Runes
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
            OldPosition = [];
            OldRotation = [];
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
            Player owner = Owner;

            TimeSpent++;
            if (KS3Rune.Boosted)
            { SetDistance(120 + (float)Math.Sin(TimeSpent * (MathHelper.Pi / 120f)) * 80f); }
            else 
            { SetDistance(90 + (float)Math.Sin(TimeSpent * (MathHelper.Pi / 120f)) * 50f); }
            

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
            int frameHeight = texture.Height / 14;

            if (Projectile.position.X < Owner.position.X)
            {
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, frameHeight * Projectile.frame, texture.Width, frameHeight), Projectile.GetAlpha(new Color(255, 255, 255, 0)), Projectile.rotation, new Vector2(texture.Width, frameHeight) * .5f, Vector2.One, SpriteEffects.FlipHorizontally, 0);
            }
            else if (Projectile.position.X > Owner.position.X)
            { 
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, frameHeight * Projectile.frame, texture.Width, frameHeight), Projectile.GetAlpha(new Color(255, 255, 255, 0)), Projectile.rotation, new Vector2(texture.Width, frameHeight) * .5f, Vector2.One, SpriteEffects.None, 0);
            }

            if (KS3Rune.Boosted)
            {
                Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
                for (int k = Projectile.oldPos.Length - 1; k > 0; k--)
                {
                    Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                    Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);

                    if (Projectile.position.X < Owner.position.X)
                        { Main.EntitySpriteDraw(texture, drawPos, new Rectangle(0, frameHeight * Projectile.frame, texture.Width, frameHeight), color, Projectile.rotation, new Vector2(texture.Width, frameHeight) * .5f, Vector2.One, SpriteEffects.FlipHorizontally, 0); }
                    else if (Projectile.position.X > Owner.position.X)
                        { Main.EntitySpriteDraw(texture, drawPos, new Rectangle(0, frameHeight * Projectile.frame, texture.Width, frameHeight), color, Projectile.rotation, new Vector2(texture.Width, frameHeight) * .5f, Vector2.One, SpriteEffects.None, 0); }
                }
            }
            return false;
        }
    }
}