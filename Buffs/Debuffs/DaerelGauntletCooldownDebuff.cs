using GuardiansOfRedemption.Items.Weapons.Gauntlets;
using OrchidMod.Content.Guardian;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Buffs.Debuffs;

public class DaerelGauntletCooldownDebuff : ModBuff
{
    public override void SetStaticDefaults()
    {
        Main.buffNoSave[Type] = true;
        Main.debuff[Type] = true;
    }
}