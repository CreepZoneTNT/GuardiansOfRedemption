using System.Collections.Generic;
using Microsoft.Xna.Framework;
using OrchidMod;
using OrchidMod.Content.Guardian;
using Redemption.BaseExtension;
using Redemption.Buffs.Debuffs;
using Redemption.Globals;
using Redemption.Items.Weapons.HM.Melee;
using Redemption.Projectiles.Melee;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Items.Weapons.Quarterstaves
{
	public class ErhanStick : OrchidModGuardianQuarterstaff {
		
		public override void SetStaticDefaults()
		{ 
			ElementID.ItemHoly[Type] = true;
		}
		public override void SafeSetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.value = Item.sellPrice(0, 0, 54);
			Item.rare = ItemRarityID.Blue;
			Item.useTime = 40;
			Item.UseSound = SoundID.Item175;
			ParryDuration = 30;
			Item.knockBack = 7f;
			Item.damage = 21;
			SlamStacks = 1;
			JabSpeed = 1.5f;
			SwingSpeed = 2.0f;
			JabStyle = 2;
			CounterSpeed = 1.5f;
			SwingDamage = 2.5f;
			JabChargeGain = 0.05f;
		}


		public override void OnAttack(Player player, OrchidGuardian guardian, Projectile projectile, bool jabAttack, bool counterAttack)
		{
			if (!jabAttack || counterAttack)
			{
				SoundEngine.PlaySound(SoundID.Item175 with {PitchRange = (-0.5f, 0.5f)}, player.Center);
			}
		}

		public override void OnHit(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, NPC.HitInfo hit, bool jabAttack, bool counterAttack)
		{
			if (!jabAttack || counterAttack)
			{
				target.AddBuff(ModContent.BuffType<HolyFireDebuff>(), counterAttack ? 30 : 15);
				player.RedemptionScreen().ScreenShakeIntensity = 1f;
			}
		}
		
		public override void ExtraAIQuarterstaffCounterattacking(Player player, OrchidGuardian guardian, Projectile projectile)
		{
			if (((GuardianQuarterstaffAnchor)projectile.ModProjectile).TimeSpent % 4 == 0) Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.Center, Vector2.UnitX.RotatedBy(projectile.rotation + MathHelper.PiOver4) * 10f, ModContent.ProjectileType<Lightmass>(), guardian.GetGuardianDamage(Item.damage * 0.5f), Item.knockBack);
		}

		public override void QuarterstaffModifyHitNPC(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, ref NPC.HitModifiers modifiers, bool jabAttack, bool counterAttack, bool firstHit)
		{
			if (NPCLists.Demon.Contains(target.type)) modifiers.FinalDamage *= 2f;
		}
	}
}
