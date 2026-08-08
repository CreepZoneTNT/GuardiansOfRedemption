using GuardiansOfRedemption.General.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod;
using OrchidMod.Content.Shapeshifter;
using Redemption;
using Redemption.Globals;
using System.Collections.Generic;
using GuardiansOfRedemption.Content.Other.Buffs.Debuffs;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Content.Shapeshifter.Projectiles.Sage
{
    public class SageBasan_Proj : OrchidModShapeshifterProjectile
    {
        public List<Vector2> OldPosition;
        public List<float> OldRotation;
        public float squish;
        public int Timespent = 0;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ElementID.ProjFire[Type] = true;
            ElementID.ProjWind[Type] = true;
        }
        public override void SafeSetDefaults()
        {
            Projectile.DamageType = ModContent.GetInstance<ShapeshifterDamageClass>();
            Projectile.width = 52;
            Projectile.height = 98;
            Projectile.scale = 0.5f;

            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 40;

            OldPosition = [];
            OldRotation = [];
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 90;
        }
        public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidShapeshifter shapeshifter)
        {
            int additiveDuration = target.GetGlobalNPC<GlobalNPCs>().BasanDebuffDuration;

            if (target.HasBuff(ModContent.BuffType<BasanBurnDebuff>()))
                target.AddBuff(ModContent.BuffType<BasanBurnDebuff>(), additiveDuration + 80);
        }
        public override void AI()
        {
            Projectile.LookByVelocity();

            Projectile.rotation = Projectile.velocity.ToRotation();
            

            squish += 0.01f;
            Projectile.alpha += 5;
            if (Projectile.alpha >= 255)
                Projectile.Kill();
        }
        public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new(texture.Width / 2, texture.Height / 2);
            SpriteEffects effects = SpriteEffects.None;
            Vector2 scale = new(Projectile.scale + squish, Projectile.scale - squish);

            Main.spriteBatch.End();
            Main.spriteBatch.BeginAdditive();

            for (int k = Projectile.oldPos.Length - 1; k > 0; k--)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);

                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(Color.White), Projectile.rotation, drawOrigin, scale, effects, 0);

            }
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(Color.White) * 0.5f, Projectile.rotation, drawOrigin, new Vector2(scale.X + 0.2f, scale.Y + 0.2f), effects, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.BeginDefault();
            return false;
        }
    }
}