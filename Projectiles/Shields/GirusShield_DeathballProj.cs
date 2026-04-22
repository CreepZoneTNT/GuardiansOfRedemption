using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Content.Guardian;
using Redemption.Effects.PrimitiveTrails;
using Redemption.Globals;
using Redemption.NPCs.Bosses.Gigapora;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace GuardiansOfRedemption.Projectiles.Shields;

// Code borrowed from Redemption.NPCs.Bosses.Gigapora.ShieldCore_DualcaseBall.cs
public class GirusShield_DualcastBallProj : OrchidModGuardianProjectile, ITrailProjectile, IManualTrailProjectile
{

    public override string Texture => "Redemption/NPCs/Bosses/Gigapora/ShieldCore_DualcastBall";
    public override void SetStaticDefaults()
    {
        Main.projFrames[Projectile.type] = 4;
        ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
    }
    
    public override void SafeSetDefaults()
    {
        Projectile.width = 30;
        Projectile.height = 30;
        Projectile.friendly = true;
        Projectile.hostile = true;
        Projectile.tileCollide = false;
        Projectile.penetrate = 1;
        Projectile.timeLeft = 300;
    }
    public void DoTrailCreation(TrailManager tManager)
    {
        tManager.CreateTrail(Projectile, new GradientTrail(new Color(223, 62, 55), new Color(150, 20, 54)), new RoundCap(), new DefaultTrailPosition(), 100f, 200f, new ImageShader(ModContent.Request<Texture2D>("Redemption/Textures/Trails/Trail_4", AssetRequestMode.ImmediateLoad).Value, 0.01f, 1f, 1f));
    }
    public override void AI()
    {
        if (++Projectile.frameCounter >= 4)
        {
            Projectile.frameCounter = 0;
            if (++Projectile.frame >= 4)
                Projectile.frame = 0;
        }
        if (Projectile.localAI[0] == 0)
        {
            DustHelper.DrawCircle(Projectile.Center, DustID.LifeDrain, 4, dustSize: 2, nogravity: true);
            Projectile.localAI[0] = 1;
        }
        Vector2 move = Vector2.Zero;
        float distance = 2000f;
        bool targeted = false;
        for (int p = 0; p < Main.maxPlayers; p++)
        {
            Player target = Main.player[p];
            if (!target.active || target.dead || target.invis)
                continue;

            Vector2 newMove = target.Center - Projectile.Center;
            float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
            if (distanceTo < distance)
            {
                move = target.Center;
                distance = distanceTo;
                targeted = true;
            }
        }
        if (targeted)
            Projectile.Move(move, 36, 80);
        else
            Projectile.velocity *= 0.94f;

        if (Projectile.timeLeft <= 260)
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile target = Main.projectile[i];
                if (target.active && target.friendly && target.damage > 0 && target.whoAmI != Projectile.whoAmI && target.type != ModContent.ProjectileType<GuardianShieldAnchor>() && Projectile.Hitbox.Intersects(target.Hitbox))
                {
                    target.Kill();
                    Projectile.Kill();
                    break;
                }

            }
        }
    }
    
    public override Color? GetAlpha(Color lightColor)
    {
        return new Color(1f, 1f, 1f, 0f) * Projectile.Opacity;
    }
    
    public override void OnKill(int timeLeft)
    {
        SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Projectile.position);
        for (int i = 0; i < 10; i++)
        {
            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, Scale: 2);
            dust.shader = GameShaders.Armor.GetSecondaryShader(GameShaders.Armor.GetShaderIdFromItemId(ItemID.RedandBlackDye), Main.LocalPlayer);
            dust.velocity *= 14;
            dust.noGravity = true;
            dust.noLight = true;
        }
    }
    

    public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
    {
        modifiers.SourceDamage *= 0.2f;
    }
    
    public override void OnHitPlayer(Player target, Player.HurtInfo info)
    {
        // if (target.statLife <= 0) {
            WeightedRandom<NetworkText> gibDeaths = new(Main.rand);
            gibDeaths.Add(NetworkText.FromKey("Mods.GuardiansOfRedemption.StatusMessage.Death.DualcastBall1", target.name), 5);
            gibDeaths.Add(NetworkText.FromKey("Mods.GuardiansOfRedemption.StatusMessage.Death.DualcastBall2", target.name), 5);
            gibDeaths.Add(NetworkText.FromKey("Mods.GuardiansOfRedemption.StatusMessage.Death.DualcastBall3", target.name), 5);
            gibDeaths.Add(NetworkText.FromKey("Mods.GuardiansOfRedemption.StatusMessage.Death.DualcastBall4", target.name), 5);
            gibDeaths.Add(NetworkText.FromKey("Mods.GuardiansOfRedemption.StatusMessage.Death.DualcastBall5", target.name, target.HeldItem.Name));
            
            NetworkText gibDeath = gibDeaths.Get();
            
            info.DamageSource.CustomReason = gibDeath;
            // target.KillMe(PlayerDeathReason.ByCustomReason(gibDeath), Projectile.damage, 1);
        // }
        
        Projectile.Kill();   
    }
}