using GuardiansOfRedemption.Items.Weapons.Shields;
using GuardiansOfRedemption.Items.Weapons.Warhammers;
using OrchidMod.Utilities;
using Redemption.Items.Usable;
using Redemption.NPCs.Bosses.Erhan;
using Redemption.NPCs.Friendly.TownNPCs;
using Redemption.NPCs.PreHM;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.General.Global;

public class NPCLootAndShop : GlobalNPC
{
    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
    {
        LeadingConditionRule nonExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
        if (npc.type == ModContent.NPCType<Erhan>())
            npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<ErhanWarhammer>(), 3));
        
        if (npc.type == ModContent.NPCType<SkeletonWarden>())
            npcLoot.Add(new CommonDrop(ModContent.ItemType<SkeletonWardenShield>(), 40, chanceNumerator: 3));
    }

    public override void ModifyShop(NPCShop shop)
    {
        if (shop.NpcType == ModContent.NPCType<Zephos>())
            shop.Add(ModContent.ItemType<ZephosWarhammer>(), Condition.DownedQueenBee);
    }
}

public class ItemLoot : GlobalItem
{
    public override void ModifyItemLoot(Item item, Terraria.ModLoader.ItemLoot itemLoot)
    {
        if (item.type == ModContent.ItemType<ErhanBag>()) itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<ErhanWarhammer>(), 3));
    }
}