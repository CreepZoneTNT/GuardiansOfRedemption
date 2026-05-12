using Microsoft.Xna.Framework;
using OrchidMod.Content.Guardian;
using Redemption;
using Redemption.Base;
using Redemption.Globals;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace GuardiansOfRedemption.Content.Guardian.Projectiles.Runes;

public class HottestPotato_Projectile : GuardianRuneProjectile
{
    public bool SetOff;
    public int Cooldown;
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
        Projectile.aiStyle = 0;
        Projectile.penetrate = -1;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 6;
        Main.projFrames[Projectile.type] = 2;
        OldPosition = [];
        OldRotation = [];
    }

    public override void FirstFrame()
    {
        rotation = Main.rand.NextFloat(MathHelper.TwoPi);
    }

    public override bool SafeAI()
    {

        if (OldPosition.Count > 5)
        {
            OldPosition.RemoveAt(0);
            OldRotation.RemoveAt(0);
        }

        Spin(1.5f);
        SetDistance(140 + (float)Math.Sin(Projectile.timeLeft * (MathHelper.Pi / 180f)) * 20f);

        Projectile.rotation = rotation;

        OldPosition.Add(Projectile.Center);
        OldRotation.Add(Projectile.rotation);

        if (--Cooldown <= 0) Cooldown = 0;

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


        if (Projectile.timeLeft <= 180)
        {

            if (Projectile.timeLeft == 180)
                SoundEngine.PlaySound(CustomSounds.EnergyCharge2);

            Projectile.rotation += Main.rand.NextFloat(0, MathHelper.TwoPi) * 0.2f;
            if (Main.rand.NextBool((int)(Projectile.timeLeft / 60f) + 1)) Dust.NewDustDirect(Projectile.Center - new Vector2(10), 5, 20, DustID.Smoke, 0f, -1.4f);
            if (Projectile.timeLeft <= 60) Dust.NewDustDirect(Projectile.Center - new Vector2(10), 5, 20, DustID.Torch, 0f, -1.4f);
        }
    

        if (Cooldown == 0)
        {
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active && proj.owner == Main.myPlayer && proj.Hitbox.Intersects(Projectile.Hitbox) && proj.friendly && proj.damage > 0 && proj.ModProjectile is OrchidModGuardianAnchor)
                {
                    SetOff = true;
                    Projectile.damage = (int)(proj.damage * 3f);
                    Projectile.Kill();
                    foreach (Projectile proj2 in Main.projectile)
                        if (proj2.active && proj2.owner == Main.myPlayer && proj2.ModProjectile is HottestPotato_Projectile potato)
                            potato.Cooldown = 20;
                    break;
                }
            }
        }
        
        return true;
    }

    public override void OnKill(int timeLeft)
    {
        if (SetOff)
        {
            SoundEngine.PlaySound(CustomSounds.MissileExplosion, Projectile.position);
            RedeDraw.SpawnExplosion(Projectile.Center, Color.Cyan, scale: 3);
        }
        else
        {
            SoundEngine.PlaySound(CustomSounds.HeavyExplosion1, Projectile.position);
            RedeDraw.SpawnExplosion(Projectile.Center, Color.Red, scale: 3);
        }

        RedeHelper.NPCRadiusDamage(160, Projectile, SetOff ? Projectile.damage + 100 : 5000, Projectile.knockBack);
        if (!SetOff) 
        {
            foreach (Terraria.Player target in Main.ActivePlayers)
            {
                if (!target.dead && Projectile.Distance(target.Center) < 160)
                {
                    int hitDirection = target.RightOfDir(Projectile);
                    BaseAI.DamagePlayer(target, target.difficulty == PlayerDifficultyID.Hardcore ? 50 : 5000, Projectile.knockBack, hitDirection, Projectile, false, true);
                }
            }
        }
    }
}