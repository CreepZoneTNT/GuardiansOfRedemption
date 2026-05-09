using GuardiansOfRedemption.General;
using GuardiansOfRedemption.General.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod;
using OrchidMod.Content.Guardian;
using Redemption.BaseExtension;
using Redemption.Globals;
using Redemption.Globals.Players;
using GuardiansOfRedemption.Buffs.Debuffs;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Items.Weapons.Standards;

public class WastelandStandard : OrchidModGuardianStandard
{
    public override void SafeSetDefaults()
    {
        Item.width = 38;
        Item.height = 38;
        Item.value = Item.sellPrice(0, 0, 0, 25);
        Item.rare = ItemRarityID.White;
        Item.useTime = 40;
        Item.UseSound = SoundID.DD2_BetsyWindAttack;
        SlamStacks = 1;
        FlagOffset = 14;
        AuraRange = 12;
        StandardDuration = 20 * 60;
        AffectNearbyPlayers = true;
        AffectNearbyNPCs = true;
    }

    public override Color GetColor() => new(77, 101, 81);

    public override bool DrawAura(bool isPlayer, bool PlayerisOwner, bool isNPC, bool isOwner, bool isReinforced) => (isPlayer && PlayerisOwner);

    public override bool NearbyPlayerEffect(GuardianStandardStats standardStats, Player affectedPlayer, OrchidGuardian guardian, bool isLocalPlayer, bool reinforced)
    {
        BuffPlayer modPlayer = affectedPlayer.RedemptionPlayerBuff();

        modPlayer.ElementalResistance[ElementID.Poison] += 0.12f;
        affectedPlayer.RedemptionRad().protectionLevel += 1;

        return false;
    }
    public override bool NearbyNPCEffect(Player player, OrchidGuardian guardian, NPC npc, bool isLocalPlayer, bool reinforced)
    {
        if (reinforced && npc.GetGlobalNPC<ElementalNPC>().elementDmg[11] < 1)
        { 
            npc.AddBuff(ModContent.BuffType<FalloutDebuff>(), 30);
        }
        return true;
    }
}
