using System;
using GuardiansOfRedemption.Achievements;
using GuardiansOfRedemption.Buffs.Debuffs;
using GuardiansOfRedemption.Items.Weapons.Quarterstaves;
using GuardiansOfRedemption.Projectiles.Shields;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod;
using OrchidMod.Content.Guardian;
using Redemption;
using Redemption.Base;
using Redemption.Globals;
using Redemption.Helpers;
using Redemption.Items.Armor.Vanity.TBot;
using Redemption.NPCs.Bosses.Erhan;
using Redemption.NPCs.Lab.Janitor;
using Redemption.Particles;
using Redemption.Textures;
using Redemption.UI.ChatUI;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.General.Global;

public class GlobalNPCs : GlobalNPC
{
    public override bool InstancePerEntity => true;

    public bool FalloutDebuff = false;
    public bool BasanDebuff = false;
    public int BasanDebuffDuration = 0;
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


        if (npc.HasBuff<BasanBurnDebuff>())
        {
            drawColor = Color.Lerp(drawColor, new Color(220, 150, 150), 0.5f);
        }
        if (npc.HasBuff<FalloutDebuff>())
        {
            drawColor = drawColor.MultiplyRGB(Color.SeaGreen * 0.5f);
        }
    }


    public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
    {   
    }

    public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
    {
        if (!RedeConfigServer.Instance.ElementDisable && !ItemLists.NoElement.Contains(item.type) && item.HasElement(ElementID.Poison) && npc.HasBuff<FalloutDebuff>())
        {
            ElementalNPC elementalNPC = npc.GetGlobalNPC<ElementalNPC>();
            float poisonResist = elementalNPC.elementDmg[ElementID.Poison];
            
            float mult = 1;
            if (poisonResist < 1) mult = poisonResist + (1 - poisonResist) / 2f;

            // Code borrowed from Redemption for calculating and adjusting CombatText value
            float baseMult = 1;
            ElementalNPC.ElementalEffects(npc, player, item, ref baseMult, ref modifiers);
            ElementalNPC.SetElementalMultipliers(npc, ref npc.GetGlobalNPC<ElementalNPC>().elementDmg);
            for (int j = 0; j < npc.GetGlobalNPC<ElementalNPC>().elementDmg.Length; j++)
            {
                if (elementalNPC.elementDmg[j] is 1 || !item.HasElement(j))
                    continue;
                baseMult *= elementalNPC.elementDmg[j];
            }
            baseMult = (int)Math.Round(baseMult * 100);
            baseMult /= 100f;
            if (npc.boss && !elementalNPC.uncappedBossMultiplier)
                baseMult = MathHelper.Clamp(baseMult, .75f, 1.25f);


            foreach (CombatText combatText in Main.combatText)
            {
                if (combatText.active && combatText.alpha == 1f && combatText.color == Color.IndianRed && combatText.crit && combatText.dot && combatText.text == baseMult + "x")
                {
                    baseMult = (int)Math.Round(baseMult * (mult / poisonResist) * 100) / 100f;
                    if (npc.boss && !elementalNPC.uncappedBossMultiplier)
                        baseMult = MathHelper.Clamp(baseMult, .75f, 1.25f);

                    combatText.color = Color.SeaGreen;
                    combatText.text = baseMult + "x";
                }
            }

            modifiers.FinalDamage *= mult / poisonResist;
        }
    }

    public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
    {
        if (!RedeConfigServer.Instance.ElementDisable && projectile.active && projectile.owner == Main.myPlayer && !ItemLists.NoElement.Contains(projectile.type) && projectile.HasElement(ElementID.Poison) && npc.HasBuff<FalloutDebuff>())
        {
            ElementalNPC elementalNPC = npc.GetGlobalNPC<ElementalNPC>();
            float poisonResist = elementalNPC.elementDmg[ElementID.Poison];

            float mult = 1;
            if (poisonResist < 1) mult = poisonResist + (1 - poisonResist) / 2f;


            // Code borrowed from Redemption for calculating and adjusting CombatText value
            float baseMult = 1;
            ElementalNPC.ElementalEffects(npc, projectile, ref baseMult, ref modifiers);
            ElementalNPC.SetElementalMultipliers(npc, ref npc.GetGlobalNPC<ElementalNPC>().elementDmg);
            for (int j = 0; j < npc.GetGlobalNPC<ElementalNPC>().elementDmg.Length; j++)
            {
                if (elementalNPC.elementDmg[j] is 1 || !projectile.HasElement(j))
                    continue;
                baseMult *= elementalNPC.elementDmg[j];
            }
            baseMult = (int)Math.Round(baseMult * 100);
            baseMult /= 100;
            if (npc.boss && !elementalNPC.uncappedBossMultiplier)
                baseMult = MathHelper.Clamp(baseMult, .75f, 1.25f);


            foreach (CombatText combatText in Main.combatText)
            {
                if (combatText.active && combatText.alpha == 1f && combatText.color == Color.IndianRed && combatText.crit && combatText.dot && combatText.text == baseMult + "x")
                {
                    baseMult = (int)Math.Round(baseMult * (mult / poisonResist) * 100) / 100f;
                    if (npc.boss && !elementalNPC.uncappedBossMultiplier)
                        baseMult = MathHelper.Clamp(baseMult, .75f, 1.25f);

                    combatText.color = Color.SeaGreen;
                    combatText.text = baseMult + "x";
                }
            }

            modifiers.FinalDamage *= mult / poisonResist;
        }
    }

    public override void UpdateLifeRegen(NPC npc, ref int damage)
    {
        if(npc.HasBuff<BasanBurnDebuff>())
        {            
            if (npc.lifeRegen > 0)
                npc.lifeRegen = 0;

            if (BasanDebuffDuration > 624)
            {
                BasanDebuffDuration = 624;
            }

                if (NPCLists.Plantlike.Contains(npc.type) || NPCLists.Cold.Contains(npc.type) || NPCLists.IsSlime.Contains(npc.type))
            {
                npc.lifeRegen -= 8 + (BasanDebuffDuration / 4);
                damage = 8 + (BasanDebuffDuration / 12);
            }
            else
                npc.lifeRegen -= 8 + (BasanDebuffDuration / 4);
                damage = 4 + (BasanDebuffDuration / 24);
            /*
            if (NPCLists.Plantlike.Contains(npc.type) || NPCLists.Cold.Contains(npc.type) || NPCLists.IsSlime.Contains(npc.type))
            {
                npc.lifeRegen -= (6 + BasanDebuffCount * 6);
                damage = 6 + (int)(BasanDebuffCount * 1.5);
            }
            else
                npc.lifeRegen -= (4 + BasanDebuffCount * 3);
                damage = 4 + BasanDebuffCount;*/
        }
    }

    public override void OnKill(NPC npc)
    {
        
        if (erhanOnlyHandJudged) ModContent.GetInstance<ErhanSlappedAchievement>().SlappedCondition.Value = 1;
    }
}