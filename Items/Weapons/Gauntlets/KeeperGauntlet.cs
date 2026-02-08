using System.Collections.Generic;
using Microsoft.Xna.Framework;
using OrchidMod;
using OrchidMod.Content.Guardian;
using Redemption;
using Redemption.Buffs.NPCBuffs;
using Redemption.Globals;
using Redemption.Items.Materials.PreHM;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Items.Weapons.Gauntlets
{
	public class KeeperGauntlet : OrchidModGuardianGauntlet
	{
		public override void SafeSetDefaults()
		{
			Item.width = 44;
			Item.height = 50;
			Item.knockBack = 4f;
			Item.damage = 120;
			Item.value = Item.sellPrice(0, 1);
			Item.rare = ItemRarityID.Blue;
			Item.useTime = 20;
			StrikeVelocity = 16f;
			ParryDuration = 60;
			PunchSpeed = 1.3f;
		}

		public override Color GetColor(bool offHand)
		{
			return new Color(117, 51, 66);
		}

		public override void ExtraAIGauntlet(Player player, OrchidGuardian guardian, Projectile anchor, bool offHandGauntlet)
		{
			GuardianGauntletAnchor modAnchor = anchor.ModProjectile as GuardianGauntletAnchor;
			if (modAnchor.Charging)
			{
				if (guardian.GuardianItemCharge >= 180f)
				{	
					if (!modAnchor.Ding)
						RedeDraw.SpawnRing(anchor.Center, Color.DarkRed, 0.2f);
					else StrikeVelocity = 30f;
				}
				else StrikeVelocity = 16f;
			}
			
		}

		public override void OnHit(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, NPC.HitInfo hit, bool charged)
		{
			if (charged) target.AddBuff(ModContent.BuffType<NecroticGougeDebuff>(), 300);
			SoundEngine.PlaySound(CustomSounds.SwordClash, projectile.position);
			RedeDraw.SpawnExplosion(RedeHelper.CenterPoint(projectile.Center, target.Center), Color.White, shakeAmount: 0, scale: 1f, noDust: true, rot: Main.rand.NextFloat(MathHelper.PiOver4, 3 * MathHelper.PiOver4), tex: "Redemption/Textures/SwordClash");
		}

		public override void OnParryGauntlet(Player player, OrchidGuardian guardian, Entity aggressor, Projectile anchor)
		{
			player.AddBuff(BuffID.Swiftness, 300);	
		}

		public override void AddRecipes()
		{
			foreach (var evilBar in (List<short>)[ItemID.DemoniteBar, ItemID.CrimtaneBar])
			{
				CreateRecipe()
				.AddIngredient<GrimShard>()
				.AddIngredient(evilBar, 12)
				.AddTile(TileID.Anvils)
				.Register();
			}
		}
	}
}
