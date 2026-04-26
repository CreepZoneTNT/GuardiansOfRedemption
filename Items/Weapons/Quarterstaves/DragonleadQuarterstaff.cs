using GuardiansOfRedemption.General;
using GuardiansOfRedemption.Projectiles.Quarterstaves;
using Microsoft.Xna.Framework;
using OrchidMod;
using OrchidMod.Content.Guardian;
using Redemption;
using Redemption.BaseExtension;
using Redemption.Buffs.NPCBuffs;
using Redemption.Effects;
using Redemption.Globals;
using Redemption.Items.Materials.PreHM;
using Redemption.Particles;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Items.Weapons.Quarterstaves
{
	public class DragonleadQuarterstaff : OrchidModGuardianQuarterstaff {
		
		public bool NormalAttack;
		public bool HasThrownFireball;
		
		private DanTrail trail;
		public Color baseColor = Color.OrangeRed * 0.7f;
		public Color endColor = Color.Yellow * 0.2f;

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
			if (projectile.ai[2] == 0 && projectile.ai[0] == 0 && projectile.ai[1] == 0)
			{
				if (!HasThrownFireball)
				{
					Vector2 direction = Vector2.Normalize(Main.MouseWorld - projectile.Center);
                    SoundEngine.PlaySound(CustomSounds.FlameRise2, projectile.Center);
                    Projectile.NewProjectileDirect(projectile.GetSource_FromAI(), ((GuardianQuarterstaffAnchor)projectile.ModProjectile).GetQuarterstaffTip(), direction * Item.shootSpeed, ModContent.ProjectileType<DragonleadQuarterstaff_Projectile>(), guardian.GetGuardianDamage(Item.damage * 0.75f), 8f, projectile.owner);
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
			
		}
		

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient<DragonLeadAlloy>(12)
				.AddIngredient(ItemID.Bone, 2)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
	
}
