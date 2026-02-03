using OrchidMod;
using OrchidMod.Content.Guardian;
using Redemption;
using Redemption.Items.Materials.PreHM;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace GuardiansOfRedemption.Items.Weapons.Warhammers;

public class WeaponGraveSteelWarhammer : OrchidModGuardianHammer
{

    public override void SafeSetDefaults()
    {
        Item.width = 36;
        Item.height = 36;
        Item.value = Item.sellPrice(0, 0, 2, 20);
        Item.rare = ItemRarityID.Blue;
        Item.UseSound = SoundID.Item1;
        Item.knockBack = 8f;
        Item.shootSpeed = 10f;
        Item.damage = 180;
        Range = 30;
        Penetrate = true;
        GuardStacks = 1;
        SwingSpeed = 1.5f;
        ReturnSpeed = 0.8f;
        BlockDuration = 150;
    }

    public override void OnThrow(Player player, OrchidGuardian guardian, Projectile projectile, bool Weak)
    {
        if (!Weak) projectile.penetrate = 2;
        else projectile.penetrate = 1;
    }

    public override void ExtraAI(Player player, OrchidGuardian guardian, Projectile projectile)
    {
        GuardianHammerAnchor anchor = projectile.ModProjectile as GuardianHammerAnchor;
        if (projectile.ai[1] > 0 && anchor.range < 0) projectile.penetrate = -1;
    }

    public override void OnThrowHit(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, float knockback, bool crit, bool Weak)
    {
        if (projectile.penetrate == 1)
        {
            for (int i = 0; i < 5; i++) Dust.NewDustDirect(projectile.Center, projectile.width, projectile.height, DustID.Lead, projectile.velocity.X, projectile.velocity.Y);
            SoundEngine.PlaySound(CustomSounds.GuardBreak, target.Center);
        }
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<GraveSteelAlloy>(10)
            .AddRecipeGroup(RecipeGroupID.Wood)
            .AddTile(TileID.Anvils)
            .Register();
    }
}