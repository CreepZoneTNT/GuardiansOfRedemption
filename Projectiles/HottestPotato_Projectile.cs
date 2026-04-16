using GuardiansOfRedemption.General.Global;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod;
using OrchidMod.Content.Guardian;
using OrchidMod.Content.Guardian.Weapons.Gauntlets;
using Redemption;
using Redemption.Base;
using Redemption.Globals;
using Redemption.Items.Weapons.PostML.Melee;
using Redemption.NPCs.Lab.MACE;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Projectiles;

public class HottestPotato_Projectile : GuardianRuneProjectile
{
    public int SetOff = 0;
    public List<Vector2> OldPosition;
    public List<float> OldRotation;

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
        Projectile.rotation = 3;
        OldPosition = new List<Vector2>();
        OldRotation = new List<float>();
    }
    public override bool SafeAI()
    {
        Player owner = Owner;
        Spin(2f);
        SetDistance(150f);

        OldPosition.Add(Projectile.Center);
        OldRotation.Add(Projectile.rotation);

        if (OldPosition.Count > 5)
        {
            OldPosition.RemoveAt(0);
            OldRotation.RemoveAt(0);
        }

        foreach (Projectile projectile in Main.projectile)
        {
            if (projectile.type == ModContent.ProjectileType<GauntletPunchProjectile>() && projectile.active && projectile.owner == Projectile.owner && projectile.ai[0] == 1f && projectile.Hitbox.Intersects(Projectile.Hitbox))
            {
                /*int projectileType = ModContent.ProjectileType<HottestPotato_Launched>();
                for (int i = 0; i < 1; i++)
                {
                    Vector2 velocity = Vector2.Normalize(projectile.velocity).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(2.5f, 3.5f))) * Main.rand.NextFloat(7.5f, 11.5f);
                    Projectile newProjectile = Projectile.NewProjectileDirect(projectile.GetSource_FromAI(), projectile.Center, velocity, projectileType, guardian.GetGuardianDamage(projectile.damage * 1f), 0.1f, owner.whoAmI);
                    newProjectile.rotation = newProjectile.velocity.ToRotation();
                    //newProjectile.velocity += owner.velocity * 1.5f;
                    newProjectile.netUpdate = true;
                }

                break;*/
                SetOff += 1;
                Projectile.damage = (int)(projectile.damage * 2.5f);
                Projectile.Kill();

            }


        }

        return true;
    }
    public override void OnKill(int timeLeft)
    {
        SoundEngine.PlaySound(CustomSounds.MissileExplosion, Projectile.position);
        RedeDraw.SpawnExplosion(Projectile.Center, Color.Cyan, scale: 3);
        for (int i = 0; i < Main.maxNPCs; i++)
        {
            NPC target = Main.npc[i];

            if (!target.active || target.dontTakeDamage)
                continue;

            if (target.immune[Projectile.whoAmI] > 0 || Projectile.DistanceSQ(target.Center) > 160 * 160)
                continue;

            target.immune[Projectile.whoAmI] = 20;
            int hitDirection = target.RightOfDir(Projectile);
            BaseAI.DamageNPC(target, Projectile.damage + 100, Projectile.knockBack, hitDirection, Projectile, crit: Projectile.HeldItemCrit());
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
                BaseAI.DamagePlayer(dumbass, 80, Projectile.knockBack, hitDirection, Projectile);
            }
        }

        else
        {

        }
    }
}
/* public class HottestPotato_Launched : OrchidModGuardianProjectile
{
    public override string Texture => "GuardiansOfRedemption/Projectiles/HottestPotato_Projectile";
    public List<Vector2> OldPosition;
    public List<float> OldRotation;

    public override void SafeSetDefaults()
    {
        Projectile.width = 22;
        Projectile.height = 22;
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.aiStyle = 0;
        Projectile.scale = 1f;
        Projectile.penetrate = 0;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 6;
        Projectile.timeLeft = 45;
        OldPosition = new List<Vector2>();
        OldRotation = new List<float>();
    }
    public override void OnKill(int timeLeft)
    {
        SoundEngine.PlaySound(CustomSounds.MissileExplosion, Projectile.position);
        RedeDraw.SpawnExplosion(Projectile.Center, Color.Cyan, scale: 0.5f);
        for (int i = 0; i < Main.maxNPCs; i++)
        {
            NPC target = Main.npc[i];

            if (!target.active || target.dontTakeDamage)
                continue;

            if (target.immune[Projectile.whoAmI] > 0 || Projectile.DistanceSQ(target.Center) > 40 * 40)
                continue;

            target.immune[Projectile.whoAmI] = 20;
            int hitDirection = target.RightOfDir(Projectile);
            BaseAI.DamageNPC(target, Projectile.damage * 4, Projectile.knockBack, hitDirection, Projectile, crit: Projectile.HeldItemCrit());
        }
        for (int i = 0; i < Main.maxPlayers; i++)
        {
            Player dumbass = Main.player[i];

            if (!dumbass.active)
                continue;

            if (Projectile.DistanceSQ(dumbass.Center) > 400 * 400)
                continue;

            int hitDirection = dumbass.RightOfDir(Projectile);
            BaseAI.DamagePlayer(dumbass, Projectile.damage * 4, Projectile.knockBack, hitDirection, Projectile);
        }
    }
} */