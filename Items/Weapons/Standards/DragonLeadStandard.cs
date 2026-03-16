using Terraria;
using Terraria.ID;
using OrchidMod;
using OrchidMod.Content.Guardian;
using Redemption;
using Redemption.BaseExtension;
using Redemption.Globals;
using Redemption.Globals.Players;
using Redemption.Buffs.Debuffs;
using GuardiansOfRedemption.General;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace GuardiansOfRedemption.Items.Weapons.Standards;

public class DragonLeadStandard : OrchidModGuardianStandard {
    
    public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(ElementID.FireS, ElementID.IceS);

    public override void SafeSetDefaults() {
        Item.width = 42;
        Item.height = 42;
        Item.value = Item.sellPrice(0, 1);
        Item.rare = ItemRarityID.Orange;
        Item.useTime = 32;
        Item.UseSound = SoundID.DD2_BetsyWindAttack;
        SlamStacks = 2;
        FlagOffset = 6;
        AuraRange = 9;
        StandardDuration = 1200;
        AffectNearbyPlayers = true;
    }

    public override Color GetColor() => new(174, 140, 143);

    public override bool DrawAura(bool isPlayer, bool PlayerisOwner, bool isNPC, bool isOwner, bool isReinforced) => (isPlayer && PlayerisOwner);

    public override bool NearbyPlayerEffect(GuardianStandardStats standardStats, Player affectedPlayer, OrchidGuardian guardian, bool isLocalPlayer, bool reinforced)
    {
        BuffPlayer modPlayer = affectedPlayer.RedemptionPlayerBuff();

        modPlayer.ElementalDamage[ElementID.Fire] += 0.05f;
        modPlayer.ElementalResistance[ElementID.Ice] += 0.10f;
        if (reinforced && isLocalPlayer) {
            guardian.RedemptionGuardian().GuardianDragonLeadStandard = true;
        }
        return true;
    }
}