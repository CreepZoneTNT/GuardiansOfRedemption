using GuardiansOfRedemption.Achievements;
using GuardiansOfRedemption.Items.Weapons.Quarterstaves;
using GuardiansOfRedemption.Projectiles.Shields;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod;
using OrchidMod.Content.Guardian;
using Redemption;
using Redemption.Helpers;
using Redemption.NPCs.Bosses.Erhan;
using Redemption.Textures;
using Redemption.UI.ChatUI;
using Terraria;
using Terraria.Audio;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.General.Global;

public class GlobalNPCs : GlobalNPC
{
    public override bool InstancePerEntity => true;
    
    
    private bool erhanHandJudged;
    public bool erhanOnlyHandJudged = true;
    public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
    {
        if (npc.ModNPC is Erhan erhan)
        {
            erhan.OnHitByProjectile(projectile, hit, damageDone);
            Texture2D bubble = !Main.dedServ ? CommonTextures.TextBubble_Epidotra.Value : null;
            SoundStyle voice = CustomSounds.Voice4 with { Pitch = -0.2f };
            Vector2 modifier = new(0,-200);
            if (projectile.ModProjectile is GuardianQuarterstaffAnchor anchor && anchor.QuarterstaffItem.ModItem is ErhanStick)
            {
                
                if (!erhanHandJudged && erhan.AIState is Erhan.ActionState.Attacks)
                {
                    string s1 = Language.GetTextValue("Mods.GuardiansOfRedemption.Cutscene.Erhan.HandOfJudgement1");
                    string s2 = Language.GetTextValue("Mods.GuardiansOfRedemption.Cutscene.Erhan.HandOfJudgement2");
                    string text = Main.rand.NextBool() ? s2 : s1; 
                    Dialogue d = new(npc, text, Color.LightGoldenrodYellow, new Color(100, 86, 0), voice, 0.03f, 2f, 0.5f, true, null, bubble, null, modifier);
                    ChatUI.Visible = true;
                    ChatUI.Add(d);
                    erhanHandJudged = true;
                }
            }
            else erhanOnlyHandJudged = false;
        }
    }

    public override void OnKill(NPC npc)
    {
        
        if (erhanOnlyHandJudged) ModContent.GetInstance<ErhanSlappedAchievement>().SlappedCondition.Value = 1;
    }
}