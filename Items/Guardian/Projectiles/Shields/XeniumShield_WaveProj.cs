using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.Guardian;
using Redemption.Globals;
using Redemption.Dusts;
using Redemption.Dusts.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Items.Guardian.Projectiles.Shields;

    public class XeniumShield_WaveProj : OrchidModGuardianProjectile
    {
		public int TimeSpent = 0;
        Vector2 SpawnPosition;
        public List<Vector2> OldPosition;
        public List<float> OldRotation;

        public override string Texture => "Redemption/Empty";

        public override void SetStaticDefaults()
        {
            ElementID.ProjThunder[Type] = true;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 50; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0; // The recording mode
        }

        public override void SafeSetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.penetrate = -1;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 65;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 60;
            OldPosition = [];
            OldRotation = [];
        }

        public override void OnSpawn(IEntitySource source)
        {
            SpawnPosition = Projectile.position;
        }

        public override void AI()
        {
            Player owner = Owner;
            TimeSpent++;

            if (Strong)
            {
                SpawnPosition += Projectile.velocity;

                Vector2 Offset = new Vector2(0, MathHelper.Lerp(-30, 30, (float)Math.Sin((TimeSpent / 5f) + 1f) / 2f)).RotatedBy(Projectile.velocity.ToRotation());
                Projectile.position = SpawnPosition + Offset;
                
                // if (TimeSpent % 6 == 0)
                    Projectile newProjectile = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, Projectile.type, Projectile.damage, 0f, owner.whoAmI);

            }
            else 
            {    
                if (Projectile.alpha < 255) Projectile.alpha += 3;
                if (Projectile.alpha >= 255) Projectile.Kill();
            }

            if (Main.rand.NextBool(10)) Dust.NewDustDirect(Projectile.Center, Projectile.width, Projectile.height, ModContent.DustType<GlowDust>(), Alpha: Projectile.alpha, newColor: Color.SeaGreen);

            Lighting.AddLight(Projectile.Center, 0f, 0.3f, 0f); // R G B values from 0 to 1f. This is the red from the Crimson Heart pet
        }
       
        // public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
        // {
        //     Texture2D texture = TextureAssets.Projectile[Type].Value;
        //     Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
        //     for (int k = Projectile.oldPos.Length - 1; k > 0; k--)
        //     {
        //         Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
        //         Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
        //         Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
        //     }

        //     return true;
        // }
    }
