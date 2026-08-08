using System;
using System.Collections.Generic;
using GuardiansOfRedemption.Content.Other.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.ModObjects;
using Redemption;
using Redemption.BaseExtension;
using Redemption.Dusts;
using Redemption.Globals;
using Redemption.Items.Weapons.HM.Ammo;
using Redemption.NPCs.FowlMorning;
using Redemption.Projectiles.Magic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Content.Other.Projectiles.Ranged;

public class BasanRifleProjectile : OrchidModProjectile
{

    public List<Vector2> OldPosition;
    public List<float> OldRotation;
    public int TimeSpent;
    public int Bounces;
    
    public static Color GhostfireColor = new (217, 84, 155, 0);

    private float DrawTimer;

    public override void SetStaticDefaults()
    {
        ElementID.ProjFire[Type] = true;
        ElementID.ProjShadow[Type] = true;
    }

    public override void AltSetDefaults()
    {
        Projectile.width = 10;
        Projectile.height = 10;
        Projectile.friendly = true;
        Projectile.DamageType = DamageClass.Ranged;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = -1;
        Projectile.timeLeft = 600;
        Projectile.light = 0.6f;
        Projectile.ignoreWater = true;
        Projectile.tileCollide = true;
        Projectile.penetrate = -1;
        Projectile.extraUpdates = 3;

        OldPosition = [];
        OldRotation = [];
        TimeSpent = 0;
        Bounces = 0;
    }

    public override void AI()
    {
        Projectile.rotation = Projectile.velocity.ToRotation();
        
        if (TimeSpent > 75)
        {
            Projectile.velocity.Y += MathF.Max(Projectile.velocity.Length() * 0.01f, 0.1f);
            if (Projectile.velocity.Y > 24f) Projectile.velocity.Y = 24f;
        }

        // Projectile.velocity *= 0.99f;

        if (Main.rand.NextBool(1))
        {
            Dust dust = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<GlowDust>(), -Projectile.velocity.RotatedByRandom(0.236f) * Main.rand.NextFloat(0.1f, 0.4f), 240, Bounces >= 25 ? new Color(251, 151, 146, 0) : GhostfireColor, Main.rand.NextFloat(0.125f, 0.625f));
            dust.noGravity = true;
        }
        
        OldPosition.Add(Projectile.Center);
        OldRotation.Add(Projectile.rotation);
        if (OldPosition.Count > 20)
        {
            OldPosition.RemoveAt(0);
            OldRotation.RemoveAt(0);
        }
        
        if (Projectile.velocity.Length() <= 0.2f) Projectile.Kill();

        TimeSpent++;
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        // Collision.HitTiles(Projectile.position, oldVelocity, Projectile.width, Projectile.height);
        Bounce(oldVelocity);
        SoundEngine.PlaySound(CustomSounds.BulletBounce with { PitchRange = (-0.4f, 0.4f), Volume = 0.2f, MaxInstances = 5 }, Projectile.Center);
        Bounces++;
        switch (Bounces)
        {
            case 5:
            {
                SoundEngine.PlaySound(CustomSounds.Reflect, Projectile.Center);
                for (int i = 0; i < 10; i++)
                {
                    Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.FlameBurst, Main.rand.NextVector2CircularEdge(2.5f, 2.5f), Scale: 1.5f);
                    dust.noGravity = true;
                }

                break;
            }
            case 25:
                SoundEngine.PlaySound(CustomSounds.Alarm2 with {Volume = 0.5f, Pitch = -0.6f}, Projectile.Center);
                break;
            case 50:
                Projectile.Kill();
                Owner.RedemptionScreen().Rumble(10, 2);
                break;
        }

        return false;
    }
    
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        target.AddBuff(ModContent.BuffType<BasanBurnDebuff>(), 120);
        target.immune[Owner.whoAmI] = 10;
        RedeHelper.NPCRadiusDamage(Bounces >= 50 ? 400 : Bounces >= 5 ? 80 : 40, Projectile, Projectile.damage, Projectile.knockBack);
        for (int i = 0; i < (Bounces >= 5 ? 20 : 10); i++)
        {
            Dust dust = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<GlowDust>(), Main.rand.NextVector2Unit() * (Bounces >= 50 ? 25f : Bounces >= 5 ? 5f : 2.5f) * Main.rand.NextFloat(), 240, GhostfireColor, Main.rand.NextFloat(0.125f, 0.625f));
            dust.noGravity = true;
        }
        SoundEngine.PlaySound(Bounces >= 5 ? SoundID.Item74 : SoundID.Item73, Projectile.Center);
        Projectile.Kill();
    }

    public override void OnKill(int timeLeft)
    {
        RedeHelper.NPCRadiusDamage(Bounces > 5 ? 80 : 40, Projectile, Projectile.damage, Projectile.knockBack);
        for (int i = 0; i < (Bounces >= 50 ? 40 : Bounces >= 5 ? 20 : 10); i++)
        {
            Dust dust = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<GlowDust>(), Main.rand.NextVector2Unit() * (Bounces >= 50 ? 25f : Bounces >= 5 ? 5f : 2.5f) * Main.rand.NextFloat(), 240, GhostfireColor, Main.rand.NextFloat(0.125f, 0.625f));
            dust.noGravity = true;
        }
        SoundEngine.PlaySound(Bounces >= 5 ? SoundID.Item74 : SoundID.Item73, Projectile.Center);

        if (Bounces >= 5)
        {
            // int damage = Projectile.damage;
            // damage = (int)(Owner.GetDamage(DamageClass.Ranged).ApplyTo(damage) + Owner.GetDamage(DamageClass.Generic).ApplyTo(damage) - damage);
            // Projectile goon = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, -Vector2.UnitY.RotatedByRandom(0.236f) * 10f, ModContent.ProjectileType<Chick_Proj>(), damage, 4f);
            // goon.DamageType = DamageClass.Ranged;
        }
    }

    public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
    {
        Texture2D texture = TextureAssets.Projectile[Type].Value;
        Vector2 drawPosition = Projectile.Center - Main.screenPosition;
        Vector2 origin = texture.Size() * 0.5f;

        for (int k = OldPosition.Count - 1; k > 0; k--)
        {
            Color color = Projectile.GetAlpha(lightColor) * ((k + 1) / (float)OldPosition.Count);
            spriteBatch.Draw(texture, OldPosition[k] - Main.screenPosition, null, color, OldRotation[k], origin, Projectile.scale, SpriteEffects.None, 0.3f);
        }
        
        if (Bounces >= 5)
            RedeDraw.DrawTreasureBagEffect(spriteBatch, texture, ref DrawTimer, drawPosition, null, GhostfireColor, Projectile.rotation, origin, Projectile.scale * 1.2f);
        
        spriteBatch.Draw(texture, drawPosition, null, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0.1f);
        return false;
    }
}