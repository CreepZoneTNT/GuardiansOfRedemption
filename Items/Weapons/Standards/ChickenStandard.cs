using GuardiansOfRedemption.General;
using GuardiansOfRedemption.General.Global;
using GuardiansOfRedemption.Projectiles.Gauntlets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod;
using OrchidMod.Content.Guardian;
using Redemption.BaseExtension;
using Redemption.Globals;
using Redemption.Globals.Players;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Items.Weapons.Standards;

public class ChickenStandard : OrchidModGuardianStandard
{
    public override void SafeSetDefaults()
    {
        Item.width = 38;
        Item.height = 38;
        Item.value = Item.sellPrice(0, 0, 16);
        Item.rare = ItemRarityID.Blue;
        Item.useTime = 60;
        Item.UseSound = SoundID.DD2_BetsyWindAttack;
        SlamStacks = 1;
        FlagOffset = 14;
        AuraRange = 8;
        StandardDuration = 20 * 60;
        AffectNearbyPlayers = true;
    }

    public override Color GetColor() => new(142, 37, 37);

    public override bool DrawAura(bool isPlayer, bool PlayerisOwner, bool isNPC, bool isOwner, bool isReinforced) => (isPlayer && PlayerisOwner);

    public override bool NearbyPlayerEffect(GuardianStandardStats standardStats, Player affectedPlayer, OrchidGuardian guardian, bool isLocalPlayer, bool reinforced)
    {
        // BuffPlayer modPlayer = affectedPlayer.RedemptionPlayerBuff();

        standardStats.lifeRegen += 2;

        if (reinforced && isLocalPlayer)
        {
            guardian.RedemptionGuardian().GuardianChickenStandard = true;
            if (Main.rand.NextBool(400))
            {
                Vector2 velocity = Vector2.UnitY.RotatedByRandom(MathHelper.Pi / 3) * -10;
                Vector2 position = affectedPlayer.Top;
                Projectile.NewProjectileDirect(affectedPlayer.GetSource_FromThis(), position, velocity, ModContent.ProjectileType<ChickenGauntlet_EggProj>(), 0, 0);
            }
        }

        if (guardian.RedemptionGuardian().GuardianChickenStandard && isLocalPlayer)
        {
            Dust.NewDustDirect(affectedPlayer.position, affectedPlayer.width, affectedPlayer.height, DustID.RedMoss, affectedPlayer.velocity.X * 0.3f, affectedPlayer.velocity.Y * 0.3f, Scale: 1);
        }
        return false;
    }
    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        base.ModifyTooltips(tooltips);

        if (Main.keyState.PressingShift())
        {
            TooltipLine line = new(Mod, "Lore", Language.GetTextValue("Mods.GuardiansOfRedemption.SpecialTooltips.ChickenStandard"))
            {
                OverrideColor = Color.LightGray
            };
            tooltips.Add(line);
        }
        else
        {
            TooltipLine line = new(Mod, "HoldShift", Language.GetTextValue("Mods.Redemption.SpecialTooltips.Viewer"))
            {
                OverrideColor = Color.Gray
            };
            tooltips.Add(line);
        }
    }
}