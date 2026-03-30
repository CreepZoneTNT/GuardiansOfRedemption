using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Redemption;
using Redemption.BaseExtension;
using Redemption.Globals;
using Redemption.NPCs.Lab.Janitor;
using Redemption.Tiles.Tiles;
using Redemption.UI.ChatUI;
using Terraria;
using Terraria.Audio;
using Terraria.Localization;
using Terraria.ModLoader;
using static GuardiansOfRedemption.General.Global.GlobalNPCs;

namespace GuardiansOfRedemption.General.Global;

public class GlobalTiles : GlobalTile {
    
    public override void PlaceInWorld(int i, int j, int type, Item item)
    {
        Vector2 tilePos = new(i * 16, j * 16);
        Player player = Main.LocalPlayer;
        if (type == ModContent.TileType<HardenedSludgeTile>()) {
            foreach (var npc in Main.npc) {
                if (npc.type == ModContent.NPCType<JanitorBot_NPC>() && npc.Sight(player, 144, false, false, canSeeHiding: true)) {
                    GlobalNPCs globalNPC = npc.GetGlobalNPC<GlobalNPCs>();
                    if (globalNPC.janitorInsultDelay > 0) return;
                    
                    SoundStyle voice = CustomSounds.Voice6 with { Pitch = -0.2f };
                    string text = Language.GetTextValue("Mods.GuardiansOfRedemption.Cutscene.Janitor.Trolled.Default");

                    ref JanitorInsultState state = ref globalNPC.janitorInsultState;

                    globalNPC.janitorInsultDelay = 240;
                    globalNPC.janitorInsultCooldown = 900;

                    if (state == JanitorInsultState.Idle && state != JanitorInsultState.Trolled) {
                        state = JanitorInsultState.Trolled;
                        if (player.invis)
                            text = Language.GetTextValue("Mods.GuardiansOfRedemption.Cutscene.Janitor.Trolled.Invisible");
                        else if (RedeWorld.Alignment < 0) {
                            globalNPC.janitorInsultAngery = true;
                            text = Language.GetTextValue("Mods.GuardiansOfRedemption.Cutscene.Janitor.Trolled.BadRoute");
                        }
                        else if (RedeConditions.IsTBotHead.IsMet())
                            text = Language.GetTextValue("Mods.GuardiansOfRedemption.Cutscene.Janitor.Trolled.Android");
                        else if (RedeConditions.IsJanitor.IsMet())
                            text = Language.GetTextValue("Mods.GuardiansOfRedemption.Cutscene.Janitor.Trolled.Janitor");
                        else text = Language.GetTextValue("Mods.GuardiansOfRedemption.Cutscene.Janitor.Trolled.GoodRoute");
                    }
                    else if (state == JanitorInsultState.GoodSamaritan && state != JanitorInsultState.Fakeout) {
                        state = JanitorInsultState.Fakeout;
                        if (player.invis)
                            text = Language.GetTextValue("Mods.GuardiansOfRedemption.Cutscene.Janitor.Fakeout.Invisible");
                        else if (RedeWorld.Alignment < 0) {
                            globalNPC.janitorInsultAngery = true;
                            text = Language.GetTextValue("Mods.GuardiansOfRedemption.Cutscene.Janitor.Fakeout.BadRoute");
                        }
                        else if (RedeConditions.IsTBotHead.IsMet())
                            text = Language.GetTextValue("Mods.GuardiansOfRedemption.Cutscene.Janitor.Fakeout.Android");
                        else if (RedeConditions.IsJanitor.IsMet())
                            text = Language.GetTextValue("Mods.GuardiansOfRedemption.Cutscene.Janitor.Fakeout.Janitor");
                        else text = Language.GetTextValue("Mods.GuardiansOfRedemption.Cutscene.Janitor.Fakeout.GoodRoute");
                    }
                    else if (state == JanitorInsultState.Apology && state != JanitorInsultState.Betrayal) {
                        state = JanitorInsultState.Betrayal;
                        globalNPC.janitorInsultAngery = true;
                        if (player.invis)
                            text = Language.GetTextValue("Mods.GuardiansOfRedemption.Cutscene.Janitor.Betrayal.Invisible");
                        else if (RedeWorld.Alignment < 0) {
                            text = Language.GetTextValue("Mods.GuardiansOfRedemption.Cutscene.Janitor.Betrayal.BadRoute");
                        }
                        else if (RedeConditions.IsTBotHead.IsMet())
                            text = Language.GetTextValue("Mods.GuardiansOfRedemption.Cutscene.Janitor.Betrayal.Android");
                        else if (RedeConditions.IsJanitor.IsMet())
                            text = Language.GetTextValue("Mods.GuardiansOfRedemption.Cutscene.Janitor.Betrayal.Janitor");
                        else text = Language.GetTextValue("Mods.GuardiansOfRedemption.Cutscene.Janitor.Betrayal.GoodRoute");
                    }
                    else if (player.invis) return;
                    if (!player.invis) globalNPC.janitorInsultAware = true;

                    Dialogue d = new(npc, text, Color.LightGoldenrodYellow, new Color(100, 86, 0), voice, 0.03f, 2f, 0.5f, true);
                    ChatUI.Visible = true;
                    ChatUI.Add(d);

                }
            }
        }
    }
}