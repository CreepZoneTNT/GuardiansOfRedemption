using GuardiansOfRedemption.Projectiles.Warhammers;
using Microsoft.Xna.Framework;
using OrchidMod;
using OrchidMod.Content.Guardian;
using Redemption.BaseExtension;
using Redemption.Globals;
using Redemption.Rarities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Items.Weapons.Warhammers;

public class PZWarhammer : OrchidModGuardianHammer
{
    public override void SetStaticDefaults()
    {
        ElementID.ItemPoison[Type] = true;
    }

    public override void SafeSetDefaults()
    {
        Item.width = 62;
        Item.height = 50;
        Item.value = Item.sellPrice(20, 0);
        Item.rare = ModContent.RarityType<TurquoiseRarity>();
        Item.UseSound = SoundID.Item1;
        Item.knockBack = 20f;
        Item.shootSpeed = 20f;
        Item.damage = 770;
        Item.useTime = 50;
        Range = 60;
        SlamStacks = 2;
        ReturnSpeed = 0.6f;
        BlockDuration = 480;
        // Item.Redemption().TechnicallyHammer = true;
        Item.Redemption().CanSwordClash = true;
    }
    public override void OnThrowHitFirst(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, float knockback, bool crit, bool Weak)
    {
        if (!Weak && !NPCLists.Robotic.Contains(target.type) && !NPCLists.Inorganic.Contains(target.type) && !NPCLists.Spirit.Contains(target.type))
            Projectile.NewProjectileDirect(projectile.GetSource_FromAI(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<PZWarhammer_CystProj>(), guardian.GetGuardianDamage(projectile.damage * 0.5f), 0, projectile.owner, target.whoAmI, Main.rand.Next(8), Main.rand.NextFloat(0.8f, 1.2f));
        
    }
    public override void OnMeleeHit(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, float knockback, bool crit, bool FullyCharged)
    {
        if (!NPCLists.Robotic.Contains(target.type) && !NPCLists.Inorganic.Contains(target.type) && !NPCLists.Spirit.Contains(target.type))
            Projectile.NewProjectileDirect(projectile.GetSource_FromAI(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<PZWarhammer_CystProj>(), guardian.GetGuardianDamage(projectile.damage * 0.25f), 0, projectile.owner, target.whoAmI, Main.rand.Next(8), Main.rand.NextFloat(0.8f, 1.2f));
    }
    
    public override void ExtraAI(Player player, OrchidGuardian guardian, Projectile projectile)
    {
        if (projectile.ModProjectile is GuardianHammerAnchor anchor)

            if (anchor.BlockDuration % 15 == 0 && anchor.BlockDuration != 0)
            {
                Projectile.NewProjectileDirect(projectile.GetSource_FromAI(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<PZWarhammer_CloudProj>(),guardian.GetGuardianDamage(projectile.damage * 0.1f), 0, projectile.owner);
            }
    }
}


 