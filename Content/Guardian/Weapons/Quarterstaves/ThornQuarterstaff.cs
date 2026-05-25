using GuardiansOfRedemption.Content.Guardian.Projectiles.Quarterstaves;
using OrchidMod;
using OrchidMod.Content.Guardian;
using Redemption.Globals;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Content.Guardian.Weapons.Quarterstaves
{
	public class ThornQuarterstaff : OrchidModGuardianQuarterstaff {

        public override void SetStaticDefaults()
        {
            ElementID.ItemPoison[Type] = true;
            ElementID.ItemNature[Type] = true;
        }
        public override void SafeSetDefaults()
		{
			Item.width = 46;
			Item.height = 46;
			Item.value = Item.sellPrice(0, 0, 8, 80);
			Item.rare = ItemRarityID.Green;
			Item.useTime = 35;
			ParryDuration = 50;
			Item.knockBack = 4f;
			Item.damage = 50;
			GuardStacks = 1;
			SwingSpeed = 0.8f;
			CounterSpeed = 0.8f;
			JabSpeed = 0.8f;
		}

        public override void OnAttack(Player player, OrchidGuardian guardian, Projectile projectile, bool jabAttack, bool counterAttack)
        {
            
            if (counterAttack && IsLocalPlayer(player))
            {
                foreach (NPC npc in Main.npc)
                {
                    Collision.CheckAABBvAABBCollision(player.position, npc.position, player.Hitbox.Size(), npc.Hitbox.Size());
                    if (!npc.friendly)
                    {
                        guardian.DoParryItemParry(npc);
                    }
                }
            }
        }
        public override void ExtraAIQuarterstaffCounterattacking(Player player, OrchidGuardian guardian, Projectile projectile)
        {
            int damage = guardian.GetGuardianDamage(Item.damage * 0.25f);
            int projectileType = ModContent.ProjectileType<ThornQuarterstaff_Projectile>();
            Projectile newProjectile = Projectile.NewProjectileDirect(Item.GetSource_FromAI(), projectile.Center, player.velocity, projectileType, damage, 0f, projectile.owner, 0.8f, Main.rand.NextFloat(3.14f));
            newProjectile.CritChance = guardian.GetGuardianCrit(Item.crit);
        }
    }
}
