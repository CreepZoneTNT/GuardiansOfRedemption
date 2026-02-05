using Microsoft.Xna.Framework;
using OrchidMod.Content.Guardian;
using OrchidMod.Content.Guardian.Weapons.Warhammers;
using Redemption.BaseExtension;
using Redemption.Globals;
using Redemption.Helpers;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.General.Global;

public class GlobalProjectiles : GlobalProjectile
{
    public override bool InstancePerEntity => true;

   

    public override void OnSpawn(Projectile projectile, IEntitySource source)
    {
        Player owner = Main.player[projectile.owner];
        
        if (projectile.owner == Main.myPlayer)
        {
            MoRGuardianPlayer modPlayer = owner.GetModPlayer<MoRGuardianPlayer>();
            if (projectile.ModProjectile is GuardianHammerAnchor && modPlayer.CosmosChainUpdates && projectile.extraUpdates < 1) projectile.extraUpdates = 1;
            else projectile.extraUpdates++;
        }
        
        if (projectile.type == ProjectileID.BladeOfGrass && source is EntitySource_Parent { Entity: Projectile parentProjectile } && parentProjectile.type == ModContent.ProjectileType<GuardianHammerAnchor>())
        {
            projectile.GetGlobalProjectile<ElementalProjectile>().OverrideElement[ElementID.Poison] = 1;
            CombatText.NewText(projectile.getRect(), Color.Green, "Test!");
        }
    }
}