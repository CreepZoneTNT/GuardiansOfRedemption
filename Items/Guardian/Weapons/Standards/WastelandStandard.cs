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
using Redemption.Items.Placeable.Tiles;
using Redemption.Items.Materials.HM;

namespace GuardiansOfRedemption.Items.Guardian.Weapons.Standards;

public class WastelandStandard : OrchidModGuardianStandard
{
    public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(ElementID.PoisonS);

    public override void SafeSetDefaults()
    {
        Item.width = 38;
        Item.height = 38;
        Item.value = Item.sellPrice(0, 1);
        Item.rare = ItemRarityID.LightRed;
        Item.useTime = 40;
        Item.UseSound = SoundID.DD2_BetsyWindAttack;
        GuardStacks = 2;
        FlagOffset = 14;
        AuraRange = 12;
        StandardDuration = 1800;
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
        // ElementalNPC elementalNPC = npc.GetGlobalNPC<ElementalNPC>();
        // if (npc.friendly)
        // {
        //     float origResist = elementalNPC.elementDmg[ElementID.Poison];
        //     elementalNPC.elementDmg[ElementID.Poison] -= 0.12f;
        //     if (elementalNPC.elementDmg[ElementID.Poison] != origResist) DustHelper.DrawCircle(npc.Center, DustID.GreenTorch);

        // }
        if (reinforced && npc.GetGlobalNPC<ElementalNPC>().elementDmg[ElementID.Poison] < 1f)
        { 
            npc.AddBuff(ModContent.BuffType<FalloutDebuff>(), 30);
        }
        return true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
        .AddIngredient(ModContent.ItemType<PetrifiedWood>(), 30)
        .AddIngredient(ItemID.Silk, 5)
        .AddIngredient(ModContent.ItemType<ToxicBile>(), 3)
        .AddTile(TileID.MythrilAnvil)
        .Register();
    }
}
