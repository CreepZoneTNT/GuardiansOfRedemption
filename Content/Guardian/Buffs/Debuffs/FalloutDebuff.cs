using Redemption.Globals;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Content.Guardian.Buffs.Debuffs
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
