using System.Collections.Generic;
using OrchidMod.Common;
using OrchidMod.Content.Guardian;
using OrchidMod.Content.Guardian.Weapons.Misc;
using OrchidMod.Content.Guardian.Weapons.Warhammers;
using Redemption.BaseExtension;
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
        if (entity.ModItem is OrchidModGuardianHammer hammer)
        {
            entity.Redemption().TechnicallyHammer = true;
        }
    }
    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {
        if (ModContent.GetInstance<OrchidClientConfig>().ShowClassTags)
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
}