using GuardiansOfRedemption.Content.Guardian.Projectiles.Warhammers;
using Microsoft.Xna.Framework;
using OrchidMod;
using OrchidMod.Content.Guardian;
using Redemption.Items.Materials.PreHM;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Content.Guardian.Weapons.Warhammers;

public class WeaponGraveSteelWarhammer : OrchidModGuardianHammer
{

    public override void SafeSetDefaults()
    {
        Item.width = 36;
        Item.height = 36;
        Item.value = Item.sellPrice(0, 0, 2, 20);
        Item.rare = ItemRarityID.Green;
        Item.UseSound = SoundID.Item1;
        Item.knockBack = 8f;
        Item.shootSpeed = 12f;
        Item.damage = 50;
        Item.useTime = 34;
        Range = 30;
        Penetrate = false;
        GuardStacks = 1;
        SwingSpeed = 1.2f;
        BlockDuration = 200;
    }

    public override void OnThrowHit(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, float knockback, bool crit, bool Weak)
    {
        if (!Weak)
        { 
            Projectile ghostHammer = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.position, new Vector2(0, -4.2f), ModContent.ProjectileType<GraveSteelWarhammer_GhostProj>(), guardian.GetGuardianDamage(Item.damage * 0.5f), projectile.knockBack, projectile.owner);
        }
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<GraveSteelAlloy>(10)
            .AddRecipeGroup(RecipeGroupID.Wood, 3)
            .AddTile(TileID.Anvils)
            .Register();
    }
}