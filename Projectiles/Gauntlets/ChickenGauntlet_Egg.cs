using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Content.Guardian;
using Redemption.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Projectiles.Gauntlets;

public class ChickenGauntlet_EggProj : OrchidModGuardianProjectile
{
    public override string Texture => "Redemption/Items/Weapons/PreHM/Ranged/ChickenEgg";

    public override void SafeSetDefaults()
    {
        Projectile.width = 20;
        Projectile.height = 20;
        Projectile.friendly = false;
        Projectile.hostile = false;
    }
    
    public override void AI()
    {
        Projectile.velocity.Y += 0.8f;
        if (Projectile.velocity.Y > 16f) Projectile.velocity.Y = 16f;
        
        Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
    }

    public override void OnKill(int timeLeft)
    {
        SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
        Dust.NewDustDirect(Projectile.Center, Projectile.width, Projectile.height, DustID.BeachShell);
        Item.NewItem(Projectile.GetSource_FromThis(), Projectile.Center, ModContent.ItemType<ChickenGauntlet_EggItem>());
    }
}

public class ChickenGauntlet_EggItem : ModItem
{
    public int goingBad = 0;
    public override LocalizedText Tooltip => LocalizedText.Empty;
    
    public override string Texture => "Redemption/Items/Weapons/PreHM/Ranged/ChickenEgg";

    public override void SetStaticDefaults()
    {
        ItemID.Sets.ItemsThatShouldNotBeInInventory[Type] = true;
        ItemID.Sets.IgnoresEncumberingStone[Type] = true;
        ItemID.Sets.IsAPickup[Type] = true;
        ItemID.Sets.ItemSpawnDecaySpeed[Type] = 4;
        Item.ResearchUnlockCount = -1;
    }

    public override void SetDefaults()
    {
        Item.width = 16;
        Item.height = 20;
        Item.maxStack = 1;
        Item.rare = ItemRarityID.White;
    }

    public override void Update(ref float gravity, ref float maxFallSpeed)
    {
        if (goingBad <= 900)
        {
            goingBad += 1;
        }
        else
        {
            SoundEngine.PlaySound(SoundID.Item16);
            Item.TurnToAir();
            goingBad = 0;
        }
    }
    public override bool OnPickup(Player player)
    {
        SoundEngine.PlaySound(SoundID.Item2);
        if (Main.rand.NextBool(100))
        {
            CombatText.NewText(player.getRect(), Color.Wheat, "eg");
            player.statLife += 8;
            if (player.statLife > player.statLifeMax2) player.statLife = player.statLifeMax2;
            SoundEngine.PlaySound(new SoundStyle("GuardiansOfRedemption/Sounds/ASDFMovieEgg", 2), player.Center);
        }
        else player.Heal(8);
        player.AddBuff(BuffID.WellFed, 400);

        for (int i = 0; i < 6; i++)
        { 
            Dust.NewDust(player.position, player.width, player.height, DustID.MothronEgg, player.velocity.X * 0.3f, player.velocity.Y * 0.3f, Scale: 2);
        }
        return false;
    }
}