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
using Redemption.Dusts;
using OrchidMod;
using Redemption.Buffs.Debuffs;

namespace GuardiansOfRedemption.Projectiles.Runes
{
    public class SeedRune_Projectile : GuardianRuneProjectile
    {
        public int TimeSpent = 0;
        float rotation;
        bool flipped;

        public List<Vector2> OldPosition;
        public List<float> OldRotation;

        public override void SetStaticDefaults()
        {
            ElementID.ProjPoison[Type] = true;
        }

        public override void RuneSetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.aiStyle = 0;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            OldPosition = [];
            OldRotation = [];
        }

        public override void FirstFrame()
        {
            rotation = Main.rand.NextFloat(MathHelper.TwoPi);
            flipped = Main.rand.NextBool();
        }

        public override bool SafeAI()
        {

            Spin(0.5f);
            TimeSpent++;
            SetDistance((float)Math.Sin(Projectile.timeLeft * (MathHelper.Pi / 90f)) * 180f);

			Projectile.rotation = rotation + (float)Math.Sin(TimeSpent * (MathHelper.Pi / 60f)) * 0.4f;

            if (TimeSpent % 4 == 0)
            {
                OldPosition.Add(Projectile.Center);
                OldRotation.Add(Projectile.rotation);
            }

            if (OldPosition.Count > 10)
            {
                OldPosition.RemoveAt(0);
                OldRotation.RemoveAt(0);
            }
            
            if (Main.rand.NextBool(20))
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center, 12, 12, ModContent.DustType<XenoemiaDust>());
                dust.noGravity = true;
            }

            return true;
        }

        public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidGuardian guardian)
        {
            target.AddBuff(ModContent.BuffType<GreenRashesDebuff>(), 60);
        }
        
        public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            SpriteEffects effects = flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			float colorMult = 1f;
			if (Projectile.timeLeft < 60) colorMult *= Projectile.timeLeft / 60f;

			for (int i = 0; i < OldPosition.Count; i++)
			{
				Vector2 drawPosition = OldPosition[i] - Main.screenPosition;
				spriteBatch.Draw(texture, drawPosition, null, Color.White * 0.1f * (i + 1) * colorMult, OldRotation[i], texture.Size() * 0.5f, Projectile.scale * (i + 1) * 0.1f, effects, 0f);
			}

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);

			Vector2 drawPosition2 = Projectile.Center - Main.screenPosition;
			spriteBatch.Draw(texture, drawPosition2, null, Color.White * colorMult, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, effects, 0f);
			return false;
		}
    }
}