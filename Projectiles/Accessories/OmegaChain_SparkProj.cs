using Redemption.Globals;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Projectiles.Accessories;

public class OmegaChain_SparkProj : ModProjectile
{
    public override string Texture => "OrchidMod/Assets/Textures/Misc/Invisible";
    
    public override void SetStaticDefaults()
    {
        ElementID.ProjThunder[Type] = true;
    }

    public override void SetDefaults()
    {
        Projectile.width = 16;
        Projectile.height = 16;
        Projectile.friendly = true;
        Projectile.usesIDStaticNPCImmunity = true;
        Projectile.idStaticNPCHitCooldown = 15;
        Projectile.timeLeft = 60;
    }

    public override void AI()
    {
        Dust.NewDustDirect(Projectile.Center, Projectile.width, Projectile.height, DustID.RedTorch);
    }
}