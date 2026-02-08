using OrchidMod.Content.Guardian;
using OrchidMod.Content.Guardian.Weapons.Misc;
using OrchidMod.Content.Guardian.Weapons.Warhammers;
using Redemption.BaseExtension;
using Terraria;
using Terraria.ID;
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
}