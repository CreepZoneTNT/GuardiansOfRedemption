using GuardiansOfRedemption.Buffs;
using OrchidMod;
using OrchidMod.Content.Guardian;
using Redemption;
using Redemption.BaseExtension;
using Redemption.Globals;
using Redemption.Rarities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace GuardiansOfRedemption.Items.Weapons.Warhammers;

public class PZWarhammer : OrchidModGuardianHammer
{
    public override void SafeSetDefaults()
    {
        Item.width = 62;
        Item.height = 50;
        Item.value = Item.sellPrice(20, 0);
        Item.rare = ModContent.RarityType<TurquoiseRarity>();
        Item.UseSound = SoundID.Item1;
        Item.knockBack = 20f;
        Item.shootSpeed = 20f;
        Item.damage = 800;
        Item.useTime = 50;
        Range = 60;
        SlamStacks = 2;
        ReturnSpeed = 0.6f;
        BlockDuration = 480;
        // Item.Redemption().TechnicallyHammer = true;
        Item.Redemption().CanSwordClash = true;
    }

    public override void OnThrowHitFirst(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, float knockback, bool crit, bool Weak)
    {
        //do your magic
    }
}
