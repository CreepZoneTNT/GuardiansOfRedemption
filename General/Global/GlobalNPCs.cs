using GuardiansOfRedemption.Achievements;
using GuardiansOfRedemption.Items.Weapons.Quarterstaves;
using GuardiansOfRedemption.Projectiles.Shields;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod;
using OrchidMod.Content.Guardian;
using Redemption;
using Redemption.Base;
using Redemption.Helpers;
using Redemption.NPCs.Bosses.Erhan;
using Redemption.Textures;
using Redemption.UI.ChatUI;
using Redemption.Items.Armor.Vanity.TBot;
using Redemption.Globals;
using Terraria;
using Terraria.Audio;
using Terraria.Localization;
using Terraria.ModLoader;
using Redemption.NPCs.Lab.Janitor;
using Terraria.Enums;
using Terraria.DataStructures;
using GuardiansOfRedemption.Buffs.Debuffs;

namespace GuardiansOfRedemption.General.Global;

public class GlobalNPCs : GlobalNPC
{
    public override bool InstancePerEntity => true;

    public bool FalloutDebuff = false;
    public enum JanitorInsultState {
        /// <summary> Default state </summary>
        Idle,
        /// <summary> Player started the interaction by cleaning hardened sludge with the Used Mop </summary>
        GoodSamaritan,
        /// <summary> Player started the interaction by placing hardened sludge </summary>
        Trolled,
        /// <summary> Player placed more hardened sludge after first cleaning it with the Used Mop </summary>
        Fakeout,
        /// <summary> Player cleaned hardened sludge with the Used Mop after first placing it down </summary>
        Apology,
        /// <summary> Player placed even more hardened sludge after cleaning it up </summary>
        Betrayal
    }

    public JanitorInsultState janitorInsultState = JanitorInsultState.Idle;
    public bool janitorInsultAngery = false;
    public bool janitorInsultAware = false;
    public int janitorInsultDelay = 0;
    public int janitorInsultAngerCooldown = 0;
    public int janitorInsultCooldown = 0;

    public override void OnSpawn(NPC npc, IEntitySource source)
    {
    }

    public override void ResetEffects(NPC npc)
    {
        if (npc.type == ModContent.NPCType<JanitorBot_NPC>()) {
            if (janitorInsultCooldown == 0) {

                if (janitorInsultAware) janitorInsultAware = false;
                janitorInsultState = JanitorInsultState.Idle;
            }
            if (--janitorInsultDelay < 0) janitorInsultDelay = 0;
            if (--janitorInsultCooldown < 0) janitorInsultCooldown = 0;
            if (--janitorInsultAngerCooldown < 0) {
                janitorInsultAngerCooldown = 0;
                if (janitorInsultAngery) janitorInsultAngery = false;            
            }
        }  
    }
    
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

    public override void DrawEffects(NPC npc, ref Color drawColor)
    {
        if (npc.type == ModContent.NPCType<JanitorBot_NPC>() && janitorInsultAngery) {
            drawColor = drawColor.MultiplyRGB(Color.Tomato);
        }
    }

    public override void HitEffect(NPC npc, NPC.HitInfo hit)
    {
        if(npc.HasBuff<FalloutDebuff>())
        {
        }
    }
    public override void OnKill(NPC npc)
    {
        
        if (erhanOnlyHandJudged) ModContent.GetInstance<ErhanSlappedAchievement>().SlappedCondition.Value = 1;
    }
}