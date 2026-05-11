using Redemption.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using GuardiansOfRedemption.General.Global;
using static System.Net.Mime.MediaTypeNames;

namespace GuardiansOfRedemption.Buffs.Debuffs
{
    internal class BasanBurnDebuff : ModBuff
    {
        public int BurnIntensity = 4;
        public int BurnDuration = 0;
        public override string Texture => "Redemption/Buffs/Debuffs/_DebuffTemplate";
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<GlobalNPCs>().BasanDebuff = true;
            npc.GetGlobalNPC<GlobalNPCs>().BasanDebuffDuration = npc.buffTime[buffIndex];
            if (Main.rand.NextBool(5) && !Main.gamePaused)
                Dust.NewDust(npc.position / 2, npc.width / 2, npc.height / 2, DustID.Torch);
        }
    }
}
