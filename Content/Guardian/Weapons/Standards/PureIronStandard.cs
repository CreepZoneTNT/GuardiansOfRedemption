using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.Localization;
using OrchidMod;
using OrchidMod.Content.Guardian;
using Redemption.BaseExtension;
using Redemption.Globals;
using Redemption.Globals.Players;
using Redemption.Items.Tools.PreHM;
using Redemption.Items.Materials.PreHM;
using Redemption.Items.Placeable.Tiles;
using Redemption.Dusts;
using GuardiansOfRedemption.General;

namespace GuardiansOfRedemption.Content.Guardian.Weapons.Standards;

public class PureIronStandard : OrchidModGuardianStandard {
    
    public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(ElementID.IceS, ElementID.FireS);

    public override void SafeSetDefaults() {
        Item.width = 58;
        Item.height = 58;
        Item.value = Item.sellPrice(0, 1);
        Item.rare = ItemRarityID.Orange;
        Item.useTime = 32;
        Item.UseSound = SoundID.DD2_BetsyWindAttack;
        SlamStacks = 1;
        GuardStacks = 1;
        FlagOffset = 6;
        AuraRange = 9;
        StandardDuration = 1200;
        AffectNearbyPlayers = true;
    }

    public override Color GetColor() => new(125, 131, 150);

    public override bool DrawAura(bool isPlayer, bool PlayerisOwner, bool isNPC, bool isOwner, bool isReinforced) => (isPlayer && PlayerisOwner);

    public override bool NearbyPlayerEffect(GuardianStandardStats standardStats, Player affectedPlayer, OrchidGuardian guardian, bool isLocalPlayer, bool reinforced)
    {
        BuffPlayer modPlayer = affectedPlayer.RedemptionPlayerBuff();

        modPlayer.ElementalDamage[ElementID.Ice] += 0.05f;
        modPlayer.ElementalResistance[ElementID.Fire] += 0.10f;
        if (reinforced && isLocalPlayer) {
            guardian.RedemptionGuardian().GuardianPureIronStandard = true;
            if (Main.rand.NextBool(4)) Dust.NewDustDirect(affectedPlayer.position, affectedPlayer.width, affectedPlayer.height, ModContent.DustType<SnowflakeDust>());
        }
        return true;
    }

    public override void AddRecipes() {
        CreateRecipe()
        .AddIngredient(ModContent.ItemType<PureIronAlloy>(), 8)
        .AddIngredient(ModContent.ItemType<ElderWood>(), 3)
        .AddIngredient(ItemID.Silk, 3)
        .AddTile(TileID.Anvils)
        .Register();
    }
}

public class PureIronStandard_IceShardProj : IceSpikeShard {
    public override string Texture => "Redemption/Projectiles/Magic/Icefall_Proj";
    
    public override void SetDefaults() {
        base.SetDefaults();
        Projectile.friendly = false;
        Projectile.DamageType = ModContent.GetInstance<GuardianDamageClass>();
    }

    public override void OnSpawn(IEntitySource source)
    {
        base.OnSpawn(source);
        Projectile.localAI[1] = 5;
    }
    
    public override void AI() {
        if (Projectile.localAI[1] == 0) Projectile.friendly = true;
        Projectile.localAI[1]--;
        base.AI();

    }
}