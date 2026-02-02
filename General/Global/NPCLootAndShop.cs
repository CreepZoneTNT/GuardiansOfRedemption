using GuardiansOfRedemption.Items.Weapons.Shields;
using OrchidMod.Utilities;
using Redemption.NPCs.PreHM;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.General.Global;

public class NPCLootAndShop : GlobalNPC
{
    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
    {
        if (npc.type == ModContent.NPCType<SkeletonWarden>())
            npcLoot.Add(new CommonDrop(ModContent.ItemType<SkeletonWardenShield>(), 40, chanceNumerator: 3));
    }
}