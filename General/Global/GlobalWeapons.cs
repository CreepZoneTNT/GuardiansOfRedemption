using GuardiansOfRedemption.Content.Guardian.Projectiles.Gauntlets;
using Microsoft.Xna.Framework;
using OrchidMod.Common;
using OrchidMod.Content.Guardian;
using Redemption.BaseExtension;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.General.Global;

public class GlobalWeapons : GlobalItem
{
    public override bool InstancePerEntity => true;

    public override void SetDefaults(Item entity)
    {
        if (entity.ModItem is OrchidModGuardianHammer)
            entity.Redemption().TechnicallyHammer = true;
        else if (entity.ModItem is OrchidModGuardianKatar)
            entity.Redemption().TechnicallySlash = true;
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

    public override void GetHealLife(Item item, Player player, bool quickHeal, ref int healValue)
    {
        if (player.RedemptionGuardian().GuardianChickenStandard)
        {
            int EggSplosion = item.healLife / 20;
            for (int EggMinimum = 0; EggMinimum < EggSplosion; EggMinimum++)
            {
                Vector2 velocity = Vector2.UnitY.RotatedByRandom(MathHelper.Pi / 3) * -15;
                Vector2 position = player.Top;
                Projectile.NewProjectileDirect(player.GetSource_FromThis(), position, velocity, ModContent.ProjectileType<ChickenGauntlet_EggProj>(), 0, 0);
            }
        }
       
    }
}