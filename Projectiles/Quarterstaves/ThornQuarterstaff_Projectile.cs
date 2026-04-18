using Microsoft.Xna.Framework;
using OrchidMod;
using OrchidMod.Content.Guardian;
using OrchidMod.Content.Guardian.Misc;
using Redemption;
using Redemption.BaseExtension;
using Redemption.Globals;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Projectiles.Quarterstaves;
public class ThornQuarterstaff_Projectile : OrchidModGuardianProjectile
{
    public List<Vector2> OldPosition;
    public List<float> OldRotation;

    public override void SetStaticDefaults()
    {
        ElementID.ProjNature[Type] = true;
        ElementID.ProjPoison[Type] = true;
    }

    public override void SafeSetDefaults()
    {
        Projectile.width = 88;
        Projectile.height = 88;
        Projectile.friendly = true;
        Projectile.aiStyle = -1;
        Projectile.timeLeft = 90;
        Projectile.scale = 1f;
        Projectile.penetrate = -1;
        Projectile.tileCollide = false;
        Projectile.alpha = 0;
        Projectile.rotation = 10;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 10;
        OldPosition = new List<Vector2>();
        OldRotation = new List<float>();
    }

    public override void OnSpawn(IEntitySource source)
    {
        Projectile.scale = 0f;
    }

    public override void AI()
    {
        Player owner = Owner;
        
        Projectile.Center = owner.Center;
        Projectile.rotation += 0.2f;

        if (Projectile.scale < 1.8f && Projectile.timeLeft > 85)
        {
            Projectile.scale += 0.15f;
        }
        else if (Projectile.scale > 1f && Projectile.timeLeft > 30)
        {
            Projectile.scale -= 0.1f;
        }
        else if (Projectile.scale > 0.1 && Projectile.timeLeft <= 30)
        {
            Projectile.scale -= 0.1f;
        }
    }
}