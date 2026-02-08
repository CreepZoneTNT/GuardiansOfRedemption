using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Content.Guardian;
using Redemption.BaseExtension;
using Redemption.Globals;
using Redemption.Projectiles.Ranged;
using Redemption.Textures;
using Terraria;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Projectiles.Accessories;

public class CosmosChain_StarProj : OrchidModGuardianProjectile
{
    public override string Texture => "Redemption/Textures/WhiteOrb";

    public override void SetStaticDefaults()
    {
        ElementID.ProjCelestial[Type] = true;
        ElementID.ProjArcane[Type] = true;
    }
    
    public List<Vector2> OldPosition;
    public List<Vector2> OldRotation;

    private NPC Target = null;

    public override void SafeSetDefaults()
    {
        Projectile.CloneDefaults(ModContent.ProjectileType<Twinklestar_TinyStar>());
        Projectile.friendly = true;
        Projectile.timeLeft = 300;
        Projectile.penetrate = 1;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = -1;
        OldPosition = [];
        OldRotation = [];
    }

    public override void AI()
    {
        if (Target == null)
        {
            int index = RedeHelper.GetNearestNPC(Projectile.Center, canBeChasedBy: true);
            if (index != -1) Target ??= Main.npc[index];
        }
        Projectile.rotation += 0.1f;
        Projectile.scale = Main.rand.NextFloat(0.1f, 0.3f);
        if (Target != null) Projectile.Move(Projectile.DirectionTo(Target.position), 2);
    }

    public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
    {
        
        Texture2D texture = ModContent.Request<Texture2D>("Redemption/Textures/WhiteFlare").Value;
        Projectile.Opacity = Projectile.timeLeft <= 20 ? (float) (1.0 - 0.05000000074505806 * (20 - Projectile.timeLeft)) : 1f;
        Color rgb = Main.hslToRgb(MathHelper.Lerp(0.0f, 1f, (float) (Main.GlobalTimeWrappedHourly * 64.0 % 360.0 / 360.0)), 1f, 0.75f);
        Color color = Color.Multiply(new Color(rgb.R, rgb.G, rgb.B, 0), Projectile.Opacity);
        Main.spriteBatch.Draw(CommonTextures.RainbowParticle2.Value, Projectile.Center - Main.screenPosition, null, color, Projectile.ai[0].InRadians().AngleLerp((Projectile.ai[0] + 90f).InRadians(), (float) ((120.0 - Projectile.timeLeft) / 120.0)), new Vector2(71f, 21f), 0.75f * Projectile.scale, SpriteEffects.None, 0.0f);
        Main.spriteBatch.Draw(CommonTextures.RainbowParticle2.Value, Projectile.Center - Main.screenPosition, null, color, Projectile.ai[0].InRadians().AngleLerp((Projectile.ai[0] + 90f).InRadians(), (float) ((120.0 - Projectile.timeLeft) / 120.0)) + 1.5707964f, new Vector2(71f, 21f), 0.75f * Projectile.scale, SpriteEffects.None, 0.0f);
        Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color * 0.5f, Projectile.ai[0].InRadians().AngleLerp((Projectile.ai[0] + 90f).InRadians(), (float) ((120.0 - Projectile.timeLeft) / 120.0)), new Vector2(57f, 57f), Projectile.scale + 1f, SpriteEffects.None, 0.0f);
        Main.spriteBatch.Draw(CommonTextures.GlowParticle.Value, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, new Vector2(64f, 64f), Projectile.scale * 0.3f, SpriteEffects.None, 0.0f);
        return false;
    }
}