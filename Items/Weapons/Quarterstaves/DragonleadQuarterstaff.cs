using GuardiansOfRedemption.General;
using Microsoft.Xna.Framework;
using OrchidMod;
using OrchidMod.Content.Guardian;
using Redemption;
using Redemption.BaseExtension;
using Redemption.Buffs.NPCBuffs;
using Redemption.Effects;
using Redemption.Items.Materials.PreHM;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Items.Weapons.Quarterstaves
{
	public class DragonleadQuarterstaff : OrchidModGuardianQuarterstaff {
		
		public bool NormalAttack;
		public bool HasYeetedMeteor;
		
		private DanTrail trail;
		public Color baseColor = Color.OrangeRed * 0.7f;
		public Color endColor = Color.Yellow * 0.2f;
		
		public override void SafeSetDefaults()
		{
			Item.width = 46;
			Item.height = 46;
			Item.value = Item.sellPrice(0, 1);
			Item.rare = ItemRarityID.Orange;
			Item.useTime = 28;
			ParryDuration = 50;
			Item.knockBack = 4f;
			Item.damage = 80;
			Item.shootSpeed = 10f;
			SlamStacks = 1;
			CounterSpeed = 1.4f;
			
			NormalAttack = false;
			HasYeetedMeteor = true;
		}

		public override void OnAttack(Player player, OrchidGuardian guardian, Projectile projectile, bool jabAttack, bool counterAttack)
		{
			if (!jabAttack && !counterAttack)
			{
				projectile.ai[2] = -40f;
				projectile.width = (int)(projectile.width * 1.5f);
				projectile.height = (int)(projectile.height * 1.5f);
				NormalAttack = true;
				HasYeetedMeteor = false;
			}
			for (int i = 0; i < 5; i++)
			{
				Dust dust = Dust.NewDustDirect(((GuardianQuarterstaffAnchor)projectile.ModProjectile).GetQuarterstaffTip(0.5f), 16, 16, DustID.SolarFlare);
				dust.noGravity = true;
			}
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
				if (!HasYeetedMeteor)
				{
					Vector2 direction = Vector2.Normalize(Main.MouseWorld - projectile.Center);
					SoundEngine.PlaySound(CustomSounds.Swoosh1, projectile.Center);
					Projectile.NewProjectileDirect(projectile.GetSource_FromAI(), ((GuardianQuarterstaffAnchor)projectile.ModProjectile).GetQuarterstaffTip(), direction * Item.shootSpeed, ProjectileID.BoulderStaffOfEarth, guardian.GetGuardianDamage(Item.damage * 1.5f), 8f, projectile.owner);
					HasYeetedMeteor = true;
				}
				NormalAttack = false;
			} 
			if (Main.rand.NextBool(4))
			{
				Dust dust = Dust.NewDustDirect(((GuardianQuarterstaffAnchor)projectile.ModProjectile).GetQuarterstaffTip(0.5f), 16, 16, DustID.SolarFlare);
				dust.noGravity = true;
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
	}
	
}
