using GuardiansOfRedemption.Buffs;
using OrchidMod;
using OrchidMod.Content.Guardian;
using Redemption;
using Redemption.BaseExtension;
using Redemption.Globals;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace GuardiansOfRedemption.Items.Weapons.Warhammers;

public class ZephosWarhammer : OrchidModGuardianHammer
{
    public override void SafeSetDefaults()
    {
        Item.width = 46;
        Item.height = 44;
        Item.value = Item.sellPrice(0, 5);
        Item.rare = ItemRarityID.Orange;
        Item.UseSound = SoundID.Item1;
        Item.knockBack = 10f;
        Item.shootSpeed = 12f;
        Item.damage = 240;
        Item.useTime = 45;
        Range = 60;
        SlamStacks = 2;
        ReturnSpeed = 0.6f;
        BlockDuration = 210;
        // Item.Redemption().TechnicallyHammer = true;
        Item.Redemption().CanSwordClash = true;
    }

    public override void OnThrowHitFirst(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, float knockback, bool crit, bool Weak)
    {
        if (!Weak) player.AddBuff(ModContent.BuffType<ZephosWarhammerBuff>(), 360);
    }

    public override void OnMeleeHit(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, float knockback, bool crit, bool FullyCharged)
    {
        if (FullyCharged && NPCLists.SkeletonHumanoid.Contains(target.type)) target.CalculateHitInfo(guardian.GetGuardianDamage(projectile.damage * 1.5f), projectile.position.X >= target.position.X ? -1 : 1, crit, knockback, ModContent.GetInstance<GuardianDamageClass>());
    }
}