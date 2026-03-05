using GuardiansOfRedemption.General.Global;
using Redemption.Globals;
using Redemption.NPCs.Bosses.Erhan;
using Terraria;
using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Achievements;

public class ErhanSlappedAchievement : ModAchievement
{
    public CustomIntCondition SlappedCondition {get; private set;}

    public override bool Hidden => RedeConditions.DownedErhan.IsMet();

    public override void SetStaticDefaults()
    {
        Achievement.SetCategory(AchievementCategory.Challenger);
        
        AddNPCKilledCondition([ModContent.NPCType<Erhan>(), ModContent.NPCType<ErhanSpirit>()]);
        SlappedCondition = AddIntCondition(1);
        
        // AchievementsHelper.OnNPCKilled += NPCKilledListener;
    }
    //
    // public override void Unload()
    // {
    //     AchievementsHelper.OnNPCKilled -= NPCKilledListener;
    // }
    //
    // private void NPCKilledListener(Player player, short npcId)
    // {
    //     if (player.whoAmI != Main.myPlayer) return;
    //     if (npcId == ModContent.NPCType<Erhan>() && ModContent.GetModNPC(npcId) is Erhan erhan && erhan.NPC.GetGlobalNPC<GlobalNPCs>().erhanOnlyHandJudged) SlappedCondition.Value = 1;
    //     else SlappedCondition.Value = 0;
    // }
}