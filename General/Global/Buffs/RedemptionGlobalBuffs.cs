using Redemption.Buffs.Debuffs;
using Redemption.Globals;
using Terraria;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.General.Global.Buffs;

public class RedemptionGlobalBuffs : GlobalBuff
{
    public override void Update(int type, NPC npc, ref int buffIndex)
    {
        if (type == ModContent.BuffType<HazardLaserDebuff>()) npc.lifeRegen -= 500;
    }
}