using OrchidMod.Utilities;
using Redemption.Globals;
using Redemption.Items.Usable;
using Redemption.NPCs.Bosses.Erhan;
using Redemption.NPCs.Bosses.Thorn;
using Redemption.NPCs.Bosses.PatientZero;
using Redemption.NPCs.Minibosses.EaglecrestGolem;
using Redemption.NPCs.FowlMorning;
using Redemption.NPCs.Friendly.TownNPCs;
using Redemption.NPCs.Lab.Janitor;
using Redemption.NPCs.PreHM;
using Terraria;
using Terraria.Enums;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Redemption.Items.Placeable.Banners;
using Redemption.BaseExtension;
using GuardiansOfRedemption.Items.Guardian.Weapons.Gauntlets;
using GuardiansOfRedemption.Items.Guardian.Weapons.Quarterstaves;
using GuardiansOfRedemption.Items.Guardian.Weapons.Runes;
using GuardiansOfRedemption.Items.Guardian.Weapons.Shields;
using GuardiansOfRedemption.Items.Guardian.Weapons.Standards;
using GuardiansOfRedemption.Items.Guardian.Weapons.Warhammers;
using GuardiansOfRedemption.Items.Other.Materials;
using Redemption.NPCs.Friendly;
using OrchidMod.Content.Shapeshifter.Weapons.Symbiote;

namespace GuardiansOfRedemption.General.Global;

public class NPCLootAndShop : GlobalNPC
{    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
    {

        LeadingConditionRule nonExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
        
        if (npc.type == ModContent.NPCType<Erhan>())
            npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<ErhanStick>(), 3));

        if (npc.type == ModContent.NPCType<ErhanSpirit>())
            npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<ErhanStick>(), 3));

        if (npc.type == ModContent.NPCType<Thorn>())
            npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<ThornQuarterstaff>(), 3));

        if (npc.type == ModContent.NPCType<PZ>())
            npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<PZWarhammer>(), 4));

        if (npc.type == ModContent.NPCType<SkeletonWarden>())
            npcLoot.Add(new CommonDrop(ModContent.ItemType<SkeletonWardenShield>(), 40, chanceNumerator: 3));

        if (npc.type == ModContent.NPCType<Basan>())
            npcLoot.Add(new CommonDrop(ModContent.ItemType<BasanMaterial>(), 1, 15, 35));
    }
    public override void GetChat(NPC npc, ref string chat)
    {
        Player player = Main.LocalPlayer;
        if (npc.type == ModContent.NPCType<JanitorBot_NPC>()) {
            GlobalNPCs globalNPC = npc.GetGlobalNPC<GlobalNPCs>();
            if (Main.rand.NextBool() && globalNPC.janitorInsultAngery) {
                if (globalNPC.janitorInsultAware) 
                    chat = Language.GetTextValue("Mods.GuardiansOfRedemption.Dialogue.Janitor.WTFMan");
                else
                    chat = Language.GetTextValue("Mods.GuardiansOfRedemption.Dialogue.Janitor.Peeved");
            }
            else 
            if (Main.rand.NextBool(3) && Main.GetMoonPhase() == MoonPhase.Empty && RedeConditions.IsJanitor.IsMet() && Main.time is >= 16200 and <= 27000)
                chat = Language.GetTextValue("Mods.GuardiansOfRedemption.Dialogue.Janitor.UsedMop");
        }   
    }

    public override void ModifyActiveShop(NPC npc, string shopName, Item[] items)
    {
        if (npc.type == ModContent.NPCType<JanitorBot_NPC>() && npc.GetGlobalNPC<GlobalNPCs>().janitorInsultAngery && npc.GetGlobalNPC<GlobalNPCs>().janitorInsultAware)
            foreach (Item item in items) 
                if (item != null && item.type != ItemID.None && item.shopCustomPrice != null) {
                    int currentPrice = item.shopCustomPrice ?? item.value;
                    item.shopCustomPrice = (int)(currentPrice * 1.5f);
                }
                
    }

    public override void ModifyShop(NPCShop shop)
    {
        if (shop.NpcType == ModContent.NPCType<TreebarkDryad>() || shop.NpcType == ModContent.NPCType<TreebarkDryad_Savanna>())
            shop.Add(ModContent.ItemType<SymbioteToad>());
        if (shop.NpcType == ModContent.NPCType<Zephos>())
            shop.Add(ModContent.ItemType<ZephosWarhammer>(), Condition.DownedQueenBee);
        if (shop.NpcType == ModContent.NPCType<JanitorBot_NPC>())
            shop.Add(ModContent.ItemType<JanitorQuarterstaff>(), RedeConditions.IsJanitor, Condition.MoonPhaseNew, new Condition("Mods.GuardiansOfRedemption.Conditions.EarlyMorning", () => Main.time is >= 16200 and <= 27000));
        if (shop.NpcType == ModContent.NPCType<Daerel>())
        {
            shop.Add(ModContent.ItemType<DaerelGauntlet>(), Condition.DownedQueenBee);
            shop.Add(ModContent.ItemType<DaerelQuarterstaff>(), Condition.DownedDukeFishron);
        }
    }
}

public class ItemLoot : GlobalItem
{
    public override void ModifyItemLoot(Item item, Terraria.ModLoader.ItemLoot itemLoot)
    {
        if (item.type == ModContent.ItemType<ErhanBag>()) itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<ErhanStick>(), 3));
        if (item.type == ModContent.ItemType<ThornBag>()) itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<ThornQuarterstaff>(), 2));
        if (item.type == ModContent.ItemType<SoIBag>()) itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<SeedRune>(), 3));
        if (item.type == ModContent.ItemType<PZBag>()) itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<PZWarhammer>(), 4));
    }
}