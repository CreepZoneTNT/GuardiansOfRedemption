using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Content.Guardian;
using OrchidMod.Utilities;
using Redemption.Globals;
using Redemption.Items.Weapons.HM.Melee;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using SpriteBatchSnapshot = OrchidMod.Utilities.SpriteBatchSnapshot;

namespace GuardiansOfRedemption.Projectiles.Warhammers;

public class CyberWarhammer_HologramProj : OrchidModGuardianProjectile
{

    public List<Vector2> OldPosition;
    public List<float> OldRotation;
    
    public bool ShouldDrawTrail = false;

    public override void SetStaticDefaults() => ElementID.ProjThunder[Type] = true;
    
    public override void SafeSetDefaults()
    {
        Projectile.width = 32;
        Projectile.height = 32;
        Projectile.friendly = true;
        Projectile.tileCollide = false;
        Projectile.penetrate = 5;
        Projectile.timeLeft = 180;
        Projectile.light = 0.5f;
        OldPosition = [];
        OldRotation = [];
        ShouldDrawTrail = false;
    }

    public override void OnSpawn(IEntitySource source)
    {
        Projectile.rotation = Projectile.ai[0];
    }

    public override void AI()
    {
        if (ShouldDrawTrail)
        {
            OldPosition.Add(Projectile.Center);
            OldRotation.Add(Projectile.rotation);
            if (OldPosition.Count > 10)
            {
                OldPosition.RemoveAt(0);
                OldRotation.RemoveAt(0);
            }
        }
        Projectile.velocity *= 0.95f;
        if (Projectile.velocity.Length() <= 1f) Projectile.velocity = Vector2.Zero;
        
        Projectile.alpha += 6;
        if (Projectile.alpha >= byte.MaxValue) Projectile.Kill();
        Projectile.rotation += Projectile.ai[2] * Projectile.ai[1];
        
        Projectile.ai[2] *= 0.9f;
    }
    
    public override Color? GetAlpha(Color lightColor)
    {
        return new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0) * Projectile.Opacity;
    }
    
    public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
    {
        Texture2D texture = TextureAssets.Projectile[Projectile.type].Value; 
        for (int i = 0; i < OldPosition.Count; i++)
        {		
            Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
            spriteBatch.Draw(texture, OldPosition[i] - Main.screenPosition, null, color, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);        
        }
        return true;
    }

}