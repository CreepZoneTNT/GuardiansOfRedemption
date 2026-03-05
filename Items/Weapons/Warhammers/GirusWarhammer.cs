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

public class GirusWarhammer : OrchidModGuardianHammer
{
    public override void SafeSetDefaults()
    {
        Item.width = 32;
        Item.height = 32;
        Item.value = Item.sellPrice(0, 1);
        Item.rare = ItemRarityID.Yellow;
        Item.UseSound = SoundID.Item1;
        Item.knockBack = 7f;
        Item.shootSpeed = 20f;
        Item.damage = 340;
        Item.useTime = 30;
        Range = 75;
        SlamStacks = 2;
        GuardStacks = 1;
        BlockDuration = 480;
    }

    public override void OnThrowHitFirst(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, float knockback, bool crit, bool Weak)
    {
        //do your magic
    }
}
