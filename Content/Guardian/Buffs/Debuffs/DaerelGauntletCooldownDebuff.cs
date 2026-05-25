using Terraria;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Content.Guardian.Buffs.Debuffs;

public class DaerelGauntletCooldownDebuff : ModBuff
{
    public override void SetStaticDefaults()
    {
        Main.buffNoSave[Type] = true;
        Main.debuff[Type] = true;
    }
}