using GuardiansOfRedemption.General;
using GuardiansOfRedemption.Content.Guardian.Projectiles.Quarterstaves;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod;
using OrchidMod.Content.Guardian;
using OrchidMod.Utilities;
using Redemption;
using Redemption.BaseExtension;
using Redemption.Buffs.NPCBuffs;
using Redemption.Effects.Trails;
using Redemption.Effects.Trails.Tips;
using Redemption.Globals;
using Redemption.Items.Materials.PreHM;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Content.Guardian.Weapons.Quarterstaves
{
	public class DragonleadQuarterstaff : OrchidModGuardianQuarterstaff {
		
		public bool NormalAttack;
		public bool HasThrownFireball;
		
		public Vector2 tip;

        public override void SetStaticDefaults()
        {
			ElementID.ItemFire[Type] = true;
        }

		public override void SafeSetDefaults()
		{
			Item.width = 46;
			Item.height = 56;
			Item.value = Item.sellPrice(0, 1);
			Item.rare = ItemRarityID.Orange;
			Item.useTime = 28;
			ParryDuration = 50;
			Item.knockBack = 4f;
			Item.damage = 80;
			Item.shootSpeed = 2f;
			SwingStyle = 3;
			SlamStacks = 1;
			CounterSpeed = 1.4f;
			NormalAttack = false;
			HasThrownFireball = true;
		}

		public override void OnAttack(Player player, OrchidGuardian guardian, Projectile projectile, bool jabAttack, bool counterAttack)
		{
			if (!jabAttack && !counterAttack)
			{
				projectile.ai[2] = -40f;
				projectile.width = (int)(projectile.width * 1.5f);
				projectile.height = (int)(projectile.height * 1.5f);
				NormalAttack = true;
                HasThrownFireball = false;
			}
			/*for (int i = 0; i < 5; i++) 
			{
				Dust dust = Dust.NewDustDirect(((GuardianQuarterstaffAnchor)projectile.ModProjectile).GetQuarterstaffTip(0.5f), 16, 16, DustID.SolarFlare);
				dust.noGravity = true;
			*/
		}

		public override void OnHit(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, NPC.HitInfo hit, bool jabAttack, bool counterAttack)
		{
			if (player.RedemptionPlayerBuff().dragonLeadBonus) target.AddBuff(ModContent.BuffType<DragonblazeDebuff>(), 300);
			else target.AddBuff(BuffID.OnFire, 120);
		}

		public override void ExtraAIQuarterstaffCounterattacking(Player player, OrchidGuardian guardian, Projectile projectile)
		{
			if (NormalAttack && Main.netMode != NetmodeID.Server)
			{
			}
		}

		public override void ExtraAIQuarterstaff(Player player, OrchidGuardian guardian, Projectile projectile)
		{
			if (projectile.ModProjectile is GuardianQuarterstaffAnchor anchor)
			{
				tip = anchor.GetQuarterstaffTip();

            	Vector2 armCenter = player.RotatedRelativePoint(player.MountedCenter, true) + new Vector2(-player.direction * 2, -2);

				if (projectile.ai[2] == 0 && projectile.ai[0] == 0 && projectile.ai[1] == 0)
				{
					if (!HasThrownFireball)
					{
						Vector2 direction = Vector2.Normalize(Main.MouseWorld - projectile.Center);
						SoundEngine.PlaySound(CustomSounds.FlameRise2, projectile.Center);
						Projectile.NewProjectileDirect(projectile.GetSource_FromAI(), tip, direction * Item.shootSpeed, ModContent.ProjectileType<DragonleadQuarterstaff_Projectile>(), guardian.GetGuardianDamage(Item.damage * 0.75f), 8f, projectile.owner);
						HasThrownFireball = true;
					}
					//NormalAttack = false;
				} 
				// Waiting until some of the particle changes get backported from the private beta
				/*if (Main.rand.NextBool(4))
				{
					float velocityY = Main.rand.NextFloat(-1f, -0.5f);
					Vector2 velocity = new Vector2(0, velocityY);
					RedeParticleManager.CreateEmberParticle(((GuardianQuarterstaffAnchor)projectile.ModProjectile).GetQuarterstaffTip(0.35f), velocity, 1f, 60);
				}*/
				if (projectile.ai[0] == 41)
				{
					for (int i = 0; i < oldDirVector.Length; i++)
						oldDirVector[i] = tip; 
				}

				for (int k = oldDirVector.Length - 1; k > 0; k--)
				{
					oldDirVector[k] = oldDirVector[k - 1];
				}
				oldDirVector[0] = tip;

				if (Main.netMode != NetmodeID.Server)
				{
					TrailHelper.ManageSwordTrailPosition(oldDirVector.Length, armCenter, oldDirVector, ref directionVectorCache, ref positionCache);
					ManageTrail(projectile);
				}
			}
			
			
		}
		
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient<DragonLeadAlloy>(12)
				.AddIngredient(ItemID.Bone, 2)
				.AddTile(TileID.Anvils)
				.Register();
		}

		// Code borrowed from Redemption (credit to Halmska and the Redemption devs)
		private Vector2[] oldDirVector = new Vector2[60];
        private List<Vector2> directionVectorCache = [];
        private List<Vector2> positionCache = [];
        private DanTrail trail;
        public Color baseColor = Color.OrangeRed * .7f;
        public Color endColor = Color.Yellow * .2f;

		public void ManageTrail(Projectile projectile)
        {
            trail= new DanTrail(RedeGraphics.Instance.Primitives, new NoTip(),
            factor =>
            {
                float mult = factor;
                float delay = 0;
                if (mult < 0.98f)
                    delay = 1;
                return MathF.Pow(mult, 0.2f) * projectile.scale * delay;
            },
            factor =>
            {
                float progress = EaseFunction.EaseCubicOut.Ease(1 - factor.X);
                return Color.Lerp(baseColor, endColor, EaseFunction.EaseCubicIn.Ease(progress)) * (1 - progress) * projectile.Opacity;
            });
            trail.SetPositions(positionCache.ToArray(), projectile.Center);
        }

        public override bool PreDrawQuarterstaff(SpriteBatch spriteBatch, Projectile projectile, Player player, ref Color lightColor)
        {
			spriteBatch.End(out SpriteBatchSnapshot snapshot);
            spriteBatch.BeginDefault();

            Effect effect = ModContent.Request<Effect>("Redemption/Effects/GlowTrailShader", AssetRequestMode.ImmediateLoad).Value;

            Matrix world = Matrix.CreateTranslation(-Main.screenPosition.X, -Main.screenPosition.Y, 0);
            Matrix view = Main.GameViewMatrix.ZoomMatrix;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, -1, 1);

            Texture2D texture = player.direction == -1 ? ModContent.Request<Texture2D>("Redemption/Textures/Trails/SlashTrail_5").Value : ModContent.Request<Texture2D>("Redemption/Textures/Trails/SlashTrail_5_flipped2").Value;

            effect.Parameters["transformMatrix"].SetValue(world * view * projection);
            effect.Parameters["sampleTexture"].SetValue(texture);
            effect.Parameters["time"].SetValue(1);
            effect.Parameters["repeats"].SetValue(-1);

            trail?.Render(effect);

            spriteBatch.End();
            spriteBatch.Begin(snapshot);

            return true;
        }
	}
	
}
