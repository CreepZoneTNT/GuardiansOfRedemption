using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod;
using OrchidMod.Content.Guardian;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Projectiles.Shields;

public class EaglecrestShield_PebblesProj : OrchidModGuardianProjectile
{
    public override string Texture => "Redemption/Empty";
    
    private Texture2D DrawTexture;
    
    private static string TexturePath = "Redemption/Gores/Hostile/AncientGladestoneGolemGore";
    private Texture2D TextureVar1 = ModContent.Request<Texture2D>(TexturePath + "6").Value;
    private Texture2D TextureVar2 = ModContent.Request<Texture2D>(TexturePath + "7").Value;
    private Texture2D TextureVar3 = ModContent.Request<Texture2D>(TexturePath + "8").Value;

    public List<Vector2> OldPosition;
    public List<float> OldRotation;
    
    public int SelectedItem { get; set; } = -1;
    public Item GuardianItem => Main.player[Projectile.owner].inventory[SelectedItem];

    private int projVariant = 0;
    
    
    public override void SafeSetDefaults()
    {
        Projectile.CloneDefaults(1013);
        Projectile.aiStyle = ProjAIStyleID.Boulder;
        Projectile.width = 8;
        Projectile.height = 8;
        Projectile.penetrate = 3;
        Projectile.hostile = false;
        Projectile.friendly = true;
        Projectile.tileCollide = true;
        Projectile.ignoreWater = true;
        Projectile.timeLeft = 300;
        Projectile.scale = 1.5f;
        OldPosition = [];
        OldRotation = [];
    }

    public override void OnSpawn(IEntitySource source)
    {
        
    
        projVariant = Main.rand.Next(0, 3);
        
        switch (projVariant)
        {
            case 0:
                DrawTexture = TextureVar1;
                break;
            case 1:
                DrawTexture = TextureVar2;
                break;
            case 2:
                DrawTexture = TextureVar3;
                break;
            default: 
                DrawTexture = TextureVar1;
                break;
        }
        
        Projectile.width = DrawTexture.Width;
        Projectile.height = DrawTexture.Width;
    }

    public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidGuardian guardian) => Projectile.Kill();

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
        SoundEngine.PlaySound(SoundID.Dig);
        

        if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X;
        if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y;
        Projectile.velocity *= 0.75f;
        Projectile.penetrate--;
        
        if (Projectile.penetrate == 0) Projectile.Kill();
        
        return false;
    }

    public override void OnKill(int timeLeft)
    {
        SoundEngine.PlaySound(SoundID.Tink);
        for (int i = 0; i < 3; i++) Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Stone);
    }

    public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
    {
        spriteBatch.Draw(DrawTexture, Projectile.position - Main.screenPosition, null, Color.White, Projectile.rotation, DrawTexture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);
        return false;
    }
}