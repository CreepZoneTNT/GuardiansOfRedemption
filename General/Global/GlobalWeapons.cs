using GuardiansOfRedemption.General.Global;
using GuardiansOfRedemption.Items.Guardian.Weapons.Standards;
using GuardiansOfRedemption.Projectiles.Gauntlets;
using Microsoft.Xna.Framework;
using OrchidMod.Common;
using OrchidMod.Content.Guardian;
using OrchidMod.Content.Guardian.Weapons.Gauntlets;
using OrchidMod.Content.Guardian.Weapons.Misc;
using OrchidMod.Content.Guardian.Weapons.Warhammers;
using Redemption.BaseExtension;
using Redemption.Dusts;
using Redemption.Rarities;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.General.Global;

public class GlobalWeapons : GlobalItem
{
    public override bool InstancePerEntity => true;

    public override void SetDefaults(Item entity)
    {
        if (entity.ModItem is OrchidModGuardianHammer)
        {
            entity.Redemption().TechnicallyHammer = true;
        }
    }
    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {
        ClientConfig addonClientConfig = ModContent.GetInstance<ClientConfig>();
        if (addonClientConfig.ClassTooltipMode == ClientConfig.ClassTagModes.Show || addonClientConfig.ClassTooltipMode == ClientConfig.ClassTagModes.FollowOrchid && ModContent.GetInstance<OrchidClientConfig>().ShowClassTags)
        {
            ModItem modItem = item?.ModItem;
            if (modItem?.Mod is GuardiansOfRedemption && item.DamageType == ModContent.GetInstance<GuardianDamageClass>())
            {
                var index = tooltips.FindIndex(i => i.Mod.Equals("Terraria") && i.Name.Equals("ItemName"));

                if (index < 0) return;
        
                tooltips.Insert(index + 1, new TooltipLine(Mod, "ClassTag", Language.GetTextValue("Mods.OrchidMod.DamageClasses.Guardian")) { OverrideColor = OrchidColors.GetClassTagColor(ClassTags.Guardian) });
            }
        }
    }
    /*public override void OnConsumeItem(Item item, Player player)
    {   
        if (item.healLife > 0) { 

            if (player.RedemptionGuardian().GuardianChickenStandard)
            {
                int EggSplosion = item.healLife / 20;
                for (int EggMinimum = 0; EggMinimum < EggSplosion; EggMinimum++)
                {
                Vector2 velocity = Vector2.UnitY.RotatedByRandom(MathHelper.Pi / 3) * -15;
                Vector2 position = player.Center + Vector2.UnitY * player.height * 0.5f;
                Projectile.NewProjectileDirect(player.GetSource_FromThis(), position, velocity, ModContent.ProjectileType<ChickenGauntlet_EggProj>(), 0, 0);
                }
            }
        }
    }*/
}