using GuardiansOfRedemption.General.Global;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod;
using OrchidMod.Content.Guardian;
using Redemption;
using Redemption.Base;
using Redemption.Globals;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Projectiles.Runes;

public class HottestPotato_Projectile : GuardianRuneProjectile
{
    public int SetOff = 0;
    public int Countdown = 60;
    float rotation = 0f;
    public List<Vector2> OldPosition;
    public List<float> OldRotation;

    public override void SetStaticDefaults()
    {
        ElementID.ProjExplosive[Type] = true;
    }
    public override void RuneSetDefaults()
    {
        Projectile.width = 22;
        Projectile.height = 22;
        Projectile.friendly = false;
        Projectile.hostile = false;
        Projectile.aiStyle = 0;
        Projectile.scale = 1f;
        Projectile.penetrate = -1;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 6;
        Main.projFrames[Projectile.type] = 2;
        OldPosition = new List<Vector2>();
        OldRotation = new List<float>();
    }



    public override void FirstFrame()
    {
        rotation = Main.rand.NextFloat(MathHelper.TwoPi);
    }
    public override bool SafeAI()
    {
        Player owner = Owner;
        Spin(1.5f);
        SetDistance(120f);

        Projectile.rotation = rotation;

        OldPosition.Add(Projectile.Center);
        OldRotation.Add(Projectile.rotation);

        if (++Projectile.frameCounter >= Countdown && Projectile.timeLeft <= 900 && Projectile.timeLeft > 300)
        {
            Projectile.frameCounter = 0;
            Countdown -= 3;

            if (++Projectile.frame >= Main.projFrames[Type])
                Projectile.frame = 0;

            if (Main.projFrames[Type] == 2)
            {
                SoundEngine.PlaySound(CustomSounds.DANShot);
            }
        }

        if (Projectile.timeLeft <= 300)
        {
            Projectile.frame = 1;
        }

        if (Projectile.timeLeft == 180)
        {
            SoundEngine.PlaySound(CustomSounds.EnergyCharge2);
        }

        if (Projectile.timeLeft <= 180)
        {
            Projectile.rotation += Main.rand.NextFloat(0, MathHelper.TwoPi) * 0.2f;
            Dust.NewDustDirect(Projectile.Center - new Vector2(10), 5, 20, DustID.Smoke, 0f, -1.4f);
        }
            if (OldPosition.Count > 5)
        {
            OldPosition.RemoveAt(0);
            OldRotation.RemoveAt(0);
        }
        {

            foreach (Projectile projectile in Main.projectile)
            {
                if (projectile.ModProjectile is GuardianShieldAnchor anchor && anchor.isSlamming is 1 && projectile.active && projectile.owner == Projectile.owner && projectile.Hitbox.Intersects(Projectile.Hitbox))
                {
                    SetOff += 1;
                    Projectile.damage = (int)(projectile.damage * 3f);
                    Projectile.Kill();
                }
                if (projectile.type == ModContent.ProjectileType<GauntletPunchProjectile>() && projectile.active && projectile.owner == Projectile.owner && projectile.ai[0] == 1f && projectile.Hitbox.Intersects(Projectile.Hitbox))
                {
                    SetOff += 1;
                    Projectile.damage = (int)(projectile.damage * 4f);
                    Projectile.Kill();
                }
            }
            return true;
        }
    }
    public override void OnKill(int timeLeft)
    {
        if (SetOff == 0)
        {
            SoundEngine.PlaySound(CustomSounds.HeavyExplosion1, Projectile.position);
            RedeDraw.SpawnExplosion(Projectile.Center, Color.Red, scale: 3);
        }

        if (SetOff == 1)
        {
            SoundEngine.PlaySound(CustomSounds.MissileExplosion, Projectile.position);
            RedeDraw.SpawnExplosion(Projectile.Center, Color.Cyan, scale: 3);
        }

        for (int i = 0; i < Main.maxNPCs; i++)
        {
            NPC target = Main.npc[i];

            if (!target.active || target.dontTakeDamage)
                continue;

            if (target.immune[Projectile.whoAmI] > 0 || Projectile.DistanceSQ(target.Center) > 160 * 160)
                continue;

            target.immune[Projectile.whoAmI] = 20;
            int hitDirection = target.RightOfDir(Projectile);
            if (SetOff == 0)
            { BaseAI.DamageNPC(target, 5000, Projectile.knockBack, hitDirection, Projectile, crit: Projectile.HeldItemCrit()); }
            if (SetOff == 1)
            { BaseAI.DamageNPC(target, Projectile.damage + 100, Projectile.knockBack, hitDirection, Projectile, crit: Projectile.HeldItemCrit()); }
        }
        if (SetOff == 0)
        {
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player dumbass = Main.player[i];

                if (!dumbass.active)
                    continue;

                if (Projectile.DistanceSQ(dumbass.Center) > 400 * 400)
                    continue;

                int hitDirection = dumbass.RightOfDir(Projectile);
                if (dumbass.difficulty == 2)
                {
                    BaseAI.DamagePlayer(dumbass, 50, Projectile.knockBack, hitDirection, Projectile);
                }
                else 
                { BaseAI.DamagePlayer(dumbass, 5000, Projectile.knockBack, hitDirection, Projectile); }
                
            }
        }
    }
}