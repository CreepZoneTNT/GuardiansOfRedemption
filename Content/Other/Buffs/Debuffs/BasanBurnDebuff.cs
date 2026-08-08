using GuardiansOfRedemption.General.Global;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Content.Other.Buffs.Debuffs
{
    internal class BasanBurnDebuff : ModBuff
    {
        public int BurnIntensity = 4;
        
        public override string Texture => "Redemption/Buffs/Debuffs/_DebuffTemplate";
        
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            GoRGlobalPlayer globalPlayer = player.GetModPlayer<GoRGlobalPlayer>();
            globalPlayer.BasanDebuff = true;
            globalPlayer.BasanDebuffDuration = player.buffTime[buffIndex];
            if (Main.rand.NextBool(5) && !Main.gamePaused)
                Dust.NewDustDirect(player.position, player.width, player.height, DustID.Torch);
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<GlobalNPCs>().BasanDebuff = true;
            npc.GetGlobalNPC<GlobalNPCs>().BasanDebuffDuration = npc.buffTime[buffIndex];
            if (Main.rand.NextBool(5) && !Main.gamePaused)
                Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Torch);
        }
    }
}
