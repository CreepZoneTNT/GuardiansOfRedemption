using Redemption.Base;
using Redemption.BaseExtension;
using Redemption.Buffs;
using Redemption.Buffs.Debuffs;
using Redemption.Buffs.NPCBuffs;
using Redemption.Globals;
using Redemption.Globals.NPCs;
using Redemption.Globals.Players;
using Redemption.Textures.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Buffs.Debuffs
{
    public class FalloutDebuff : ModBuff
    {
        public override string Texture => "Redemption/Buffs/Debuffs/_DebuffTemplate";
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
            BuffID.Sets.GrantImmunityWith[Type].Add(BuffID.Ichor);
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            // npc.GetGlobalNPC<GlobalNPCs>().WastelandStandardEffect = true;
            if (Main.rand.NextBool(6)) Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.GreenBlood);
        }
    }
}
