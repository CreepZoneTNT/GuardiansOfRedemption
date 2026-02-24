using OrchidMod;
using OrchidMod.Content.Guardian;
using Redemption.Buffs.Debuffs;
using Redemption.Globals;
using Redemption.Items.Weapons.HM.Melee;
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

		public override string QuarterstaffTexture => Texture;

		public override void SafeSetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.value = Item.sellPrice(0, 0, 54);
			Item.rare = ItemRarityID.Blue;
			Item.useTime = 30;
			Item.UseSound = SoundID.Item175;
			ParryDuration = 30;
			Item.knockBack = 7f;
			Item.damage = 42;
			SlamStacks = 1;
			SwingSpeed = 1.75f;
			JabSpeed = 1.75f;
			JabStyle = 2;
			CounterSpeed = 1.5f;
		}

		public override void OnHit(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, NPC.HitInfo hit, bool jabAttack, bool counterAttack)
		{
			if (!jabAttack) target.AddBuff(ModContent.BuffType<HolyFireDebuff>(), 15);
			else target.AddBuff(BuffID.OnFire, 120);
		}

		public override void ExtraAIQuarterstaffCounterattacking(Player player, OrchidGuardian guardian, Projectile projectile)
		{
			
		}

		public override void QuarterstaffModifyHitNPC(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, ref NPC.HitModifiers modifiers, bool jabAttack, bool counterAttack, bool firstHit)
		{
			if (!jabAttack)
			{
				if (NPCLists.Demon.Contains(target.type)) modifiers.FinalDamage *= 2f;
			}
		}
	}
}
