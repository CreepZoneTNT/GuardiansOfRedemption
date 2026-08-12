using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using OrchidMod.Utilities;
using Redemption;
using Redemption.Globals;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Content.Other.Misc;

public class ElementalModulator : ModItem
{
    public NPC LinkedDummy;
    public int LinkedDummyIndex = -1;

    public int ElementMode = 1;

    public static string TooltipPath => ModContent.GetInstance<GuardiansOfRedemption>().GetLocalizationKey("Items." + nameof(ElementalModulator));
    public static string CombatTextPath => TooltipPath + ".CombatText";

    public bool DummyIsValid(Player player, bool hovering = false) => LinkedDummy != null && LinkedDummyIndex != -1 && LinkedDummy.active && ValidDummyTypes.Contains(LinkedDummy.type) && LinkedDummy.whoAmI == LinkedDummyIndex && (!hovering || (LinkedDummy.Hitbox.Contains(Main.MouseWorld) && LinkedDummy.Distance(player.Center) < 1200f));

    public static List<int> ValidDummyTypes;
    
    public override void SetDefaults()
    {
        Item.width = 24;
        Item.height = 24;
        Item.useTime = 30;
        Item.useAnimation = 6;
        Item.rare = ItemRarityID.Quest;
        Item.useStyle = ItemUseStyleID.HoldUp;
        Item.UseSound = CustomSounds.Voice3 with {PitchVariance = 0.5f};
    }

    public override void SetStaticDefaults()
    {
        ValidDummyTypes = [NPCID.TargetDummy];
        Mod thoriumMod = OrchidMod.OrchidMod.ThoriumMod;
        if (thoriumMod != null)
        {
            ValidDummyTypes.Add(thoriumMod.Find<ModNPC>("AggroDummy").Type);
            ValidDummyTypes.Add(thoriumMod.Find<ModNPC>("BossDummy").Type);
        }
    }

    public override void HoldItem(Player player)
    {
        if (player.whoAmI == Main.myPlayer)
        {
            if (DummyIsValid(player, true))
            {
                if (player.OrchidPlayer().Timer120 % 30 == 0) DustHelper.DrawCircle(LinkedDummy.Center, DustID.CoralTorch, 3f, dustDensity: 0.5f, dustSize: 1.5f, nogravity: true);
                if (Main.mouseMiddle && Main.mouseMiddleRelease)
                {
                    ElementalNPC elementalNPC = LinkedDummy.GetGlobalNPC<ElementalNPC>();
                    
                    if (Main.keyState.PressingControl())
                    {
                        for (int i = 0; i < 15; i++)
                        {
                            elementalNPC.OverrideElement[i] = 0;
                            elementalNPC.OverrideMultiplier[i] = 1f;
                        }

                        CombatText.NewText(LinkedDummy.getRect(), Color.Goldenrod, CombatTextPath + ".ResetAll", true, true);
                    }
                    else
                    {
                        elementalNPC.OverrideElement[ElementMode] = 0;
                        elementalNPC.OverrideMultiplier[ElementMode] = 1f;
                        
                        CombatText.NewText(LinkedDummy.getRect(), Color.Yellow, CombatTextPath + ".Reset", dot: true);
                    }
                    
                    ElementalNPC.SetElementalMultipliers(LinkedDummy, ref elementalNPC.elementDmg);

                    SoundEngine.PlaySound(CustomSounds.ShootChange, player.Center);
                    player.ApplyItemTime(Item, callUseItem: false);
                }
            }
            else
            {
                foreach (NPC npc in Main.ActiveNPCs)
                {
                    if (ValidDummyTypes.Contains(npc.type) && npc.Distance(player.Center) < 1200f && npc.Hitbox.Contains(Main.MouseWorld))
                    {
                        if (player.OrchidPlayer().Timer120 % 30 == 0) DustHelper.DrawCircle(npc.Center, DustID.BoneTorch, 3f, dustDensity: 0.5f, dustSize: 1.5f, nogravity: true);
                        break;
                    }
                }
            }
        }
    }

    public override bool CanRightClick() => !RedeConfigServer.Instance.ElementDisable;

    public override void RightClick(Player player)
    {
        if (!RedeConfigServer.Instance.ElementDisable)
        {
            if (Main.keyState.PressingShift())
            {
                ElementMode--;
                if (ElementMode < 1)
                    ElementMode = 15;
            }
            else
            {
                ElementMode++;
                if (ElementMode > 15)
                    ElementMode = 1;
            }

            SoundEngine.PlaySound(CustomSounds.ShootChange, player.Center);
        }

    }

    public override bool ConsumeItem(Player player) => false;

    public override bool AltFunctionUse(Player player) => true;

    public override bool? UseItem(Player player)
    {
        if (RedeConfigServer.Instance.ElementDisable) return false;
        if (player.whoAmI == Main.myPlayer)
        {
            if (DummyIsValid(player))
            {
                ElementalNPC elementalNPC = LinkedDummy.GetGlobalNPC<ElementalNPC>();
                ref float multiplier = ref elementalNPC.OverrideMultiplier[ElementMode];
                elementalNPC.OverrideElement[ElementMode] = 1;
                
                if (LinkedDummy.Hitbox.Contains(Main.MouseWorld))
                {
                    if (Main.keyState.PressingControl())
                    {
                        LinkedDummy = null;
                        LinkedDummyIndex = -1;
                        CombatText.NewText(player.getRect(), Color.LimeGreen, Language.GetTextValue(CombatTextPath + ".Clear"), dot: true);
                        return true;
                    }
                    
                    if (player.altFunctionUse == 2)
                    {
                        multiplier -= 0.05f;
                        if (multiplier < 0f) multiplier = 0f;
                        CombatText.NewText(LinkedDummy.getRect(), Color.Red, "-5%", dot: true);
                    }
                    else
                    {
                        multiplier += 0.05f;
                        if (multiplier > 2f) multiplier = 2f;
                        CombatText.NewText(LinkedDummy.getRect(), Color.Green, "+5%", dot: true);   
                    }
                    
                    ElementalNPC.SetElementalMultipliers(LinkedDummy, ref elementalNPC.elementDmg);
                    return true;
                }
                else
                {
                    foreach (NPC npc in Main.ActiveNPCs)
                    {
                        if (ValidDummyTypes.Contains(npc.type) && npc.whoAmI != LinkedDummyIndex && npc.Distance(player.Center) < 1200f && npc.Hitbox.Contains(Main.MouseWorld))
                        {
                            LinkedDummy = npc;
                            LinkedDummyIndex = npc.whoAmI;
                            CombatText.NewText(player.getRect(), Color.LightYellow, Language.GetTextValue(CombatTextPath + ".Linked"), dot: true);
                            SoundEngine.PlaySound(CustomSounds.ShootChange, player.Center);
                            return true;
                        }
                    }
                }
            }
            else
            {
                if (LinkedDummy != null && Main.keyState.PressingControl())
                {
                    LinkedDummy = null;
                    LinkedDummyIndex = -1;
                    CombatText.NewText(player.getRect(), Color.Green, Language.GetTextValue(CombatTextPath + ".Clear"), dot: true);
                    return false;
                }
                
                foreach (NPC npc in Main.ActiveNPCs)
                {
                    if (ValidDummyTypes.Contains(npc.type) && npc.whoAmI != LinkedDummyIndex && npc.Distance(player.Center) < 1200f && npc.Hitbox.Contains(Main.MouseWorld))
                    {
                        LinkedDummy = npc;
                        LinkedDummyIndex = npc.whoAmI;
                        CombatText.NewText(player.getRect(), Color.LightYellow, Language.GetTextValue(CombatTextPath + ".Linked"), dot: true);
                        SoundEngine.PlaySound(CustomSounds.ShootChange, player.Center);
                        return true;
                    }
                }
            }
            
            return false;
        }
        return null;
    }

    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        int elementToGet = ElementMode == 15 ? 5 : ElementMode + 5;
        
        int index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("Tooltip3"));
        tooltips.Insert(index + 1, new TooltipLine(Mod, "ElementMode", Language.GetTextValue(TooltipPath + ".ElementMode", ElementID.BonusNameFromID(elementToGet))));

        string tooltipToGet = Language.GetTextValue(TooltipPath + (DummyIsValid(Main.LocalPlayer) ? ".LinkedDummy" : ".NoDummy"), nameof(LinkedDummy), LinkedDummy?.ai[0], LinkedDummy?.ai[1]);
        ElementalNPC elementalNPC = LinkedDummy?.GetGlobalNPC<ElementalNPC>();
        if (elementalNPC != null && elementalNPC.OverrideElement[ElementMode] == 1 && Main.keyState.PressingShift()) tooltipToGet += " (" + ElementID.BonusNameFromID(elementToGet) + ": " + MathF.Round(100 * elementalNPC.OverrideMultiplier[ElementMode]) / 100f + "x)";
        tooltips.Insert(index + 2, new TooltipLine(Mod, "LinkedDummy", tooltipToGet));

        // if (true && elementalNPC != null && Main.keyState.PressingControl())
        // {
        //     tooltips.Insert(index + 3, new TooltipLine(Mod, "Debug", string.Join(", ", elementalNPC.OverrideElement)));
        //     tooltips.Insert(index + 4, new TooltipLine(Mod, "Debug2", string.Join(", ", elementalNPC.OverrideMultiplier)));
        //     tooltips.Insert(index + 5, new TooltipLine(Mod, "Debug3", string.Join(", ", elementalNPC.elementDmg)));
        // }
    }
}