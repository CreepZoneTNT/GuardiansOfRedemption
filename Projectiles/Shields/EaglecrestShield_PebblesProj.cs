using System;
using System.Collections.Generic;
using GuardiansOfRedemption.General;
using GuardiansOfRedemption.General.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod;
using OrchidMod.Content.Guardian;
using Redemption;
using Redemption.Buffs.NPCBuffs;
using Redemption.Globals;
using Redemption.Helpers;
using Redemption.Items.Weapons.PreHM.Melee;
using Redemption.NPCs.Critters;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

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
    

    private int projVariant;

    public override void SetStaticDefaults()
    {
        ElementID.ProjEarth[Type] = true;
        ElementID.ProjThunder[Type] = true;
    }

    public override void SafeSetDefaults()
    {
        Projectile.width = 8;
        Projectile.height = 8;
        Projectile.penetrate = 3;
        Projectile.hostile = false;
        Projectile.friendly = true;
        Projectile.tileCollide = true;
        Projectile.ignoreWater = true;
        Projectile.timeLeft = 300;
        Projectile.scale = 1.5f;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 20;
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

    public override void AI()
    {
        Projectile.ai[0]++;
        if (Projectile.ai[0] > 10) Projectile.velocity.Y += 0.4f;
        
        Projectile.rotation = 0.2f * Projectile.velocity.ToRotation() - MathHelper.PiOver2;
    }

    

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
        SoundEngine.PlaySound(SoundID.Dig);
        

        if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X;
        if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y;
        Projectile.velocity *= 0.6f;
        Projectile.penetrate--;
        
        if (Projectile.penetrate == 0) Projectile.Kill();
        
        return false;
    }

    public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidGuardian guardian)
    {
        Projectile.Kill();
        
        RedemptionGuardian addonGuardian = guardian.RedemptionGuardian();
        if (addonGuardian.EaglecrestShieldTarget == null) addonGuardian.EaglecrestShieldTarget = target;
        if (target == addonGuardian.EaglecrestShieldTarget)
        {
            if (++addonGuardian.EaglecrestShieldHitCount == 5) CombatText.NewText(player.getRect(), Color.Gold, addonGuardian.EaglecrestShieldHitCount, true);
            else CombatText.NewText(player.getRect(), Color.Yellow, addonGuardian.EaglecrestShieldHitCount);
            if (addonGuardian.EaglecrestShieldHitCount >= 5)
            {
                addonGuardian.EaglecrestShieldTarget = null;
                addonGuardian.EaglecrestShieldHitCount = 0;
                
                
                SoundEngine.PlaySound(CustomSounds.Thunderstrike, Projectile.Center);
                
                for (int i = 0; i < 3; i++) DustHelper.DrawParticleElectricity(Projectile.Center - new Vector2(0.0f, 400f), Projectile.Center, 2f, density: 0.1f, colorType: 1);
                DustHelper.DrawCircle(Projectile.Center - Vector2.UnitY * 400f, DustID.Sandnado, RatioX: 4f, RatioY: 4f, dustSize: 3f, nogravity: true);
                RedeHelper.NPCRadiusDamage(48, Projectile, guardian.GetGuardianDamage(90), 8f, 0);
                target.AddBuff(ModContent.BuffType<ElectrifiedDebuff>(), 60);
            }
        }
        else
        {
            addonGuardian.EaglecrestShieldTarget = null;
            addonGuardian.EaglecrestShieldHitCount = 0;
        }
    }
    public override void OnKill(int timeLeft)
    {
        SoundEngine.PlaySound(SoundID.Tink);
        for (int i = 0; i < 5; i++) Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Stone);
    }

    public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
    {
        spriteBatch.Draw(DrawTexture, Projectile.position - Main.screenPosition, null, Color.White, Projectile.rotation, DrawTexture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);
        return false;
    }
}