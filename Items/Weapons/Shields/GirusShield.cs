using System;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod;
using OrchidMod.Common;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.General.Prefixes;
using OrchidMod.Utilities;
using OrchidMod.Content.Guardian;
using Redemption.Globals;
using Redemption.NPCs.Bosses.Gigapora;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using SpriteBatchSnapshot = OrchidMod.Utilities.SpriteBatchSnapshot;

namespace GuardiansOfRedemption.Items.Weapons.Shields;

public class GirusShield : OrchidModGuardianShield
{
    
    public int AnimTimer;
    public int AnimFrame;
    
    
    public int SlamTimer;
    public bool AttemptingCharge;
    public bool CanReleaseEarly;
    public bool ChargedSlam;
    
    private float DrawTimer;

    
    public override void SetStaticDefaults()
    { 
        ElementID.ItemFire[Type] = true;
    }
    
    public override void SafeSetDefaults()
    {
        Item.value = Item.sellPrice(0, 8);
        Item.width = 36;
        Item.height = 48;
        Item.knockBack = 6f;
        Item.damage = 300;
        Item.rare = ItemRarityID.Yellow;
        Item.useTime = 30;
        Item.shootSpeed = 16f;
        distance = 90f;
        slamDistance = 60f;
        blockDuration = 300;
        shouldFlip = true; 
        slamAutoReuse = false;
        
        AnimTimer = 0;
        AnimFrame = 0;
        
        
        SlamTimer = 0;
        AttemptingCharge = false;
        CanReleaseEarly = false;
        ChargedSlam = false;
    }

    public override bool CanUseItem(Player player)
    {
        bool slamButton = ModContent.GetInstance<OrchidClientConfig>().GuardianSwapPaviseImputs ? Main.mouseRight : Main.mouseLeft;
        
        
        return base.CanUseItem(player);
    }

    public override void ExtraAIShield(Projectile projectile)
    {
        if (projectile.owner == Main.myPlayer && projectile.ModProjectile is GuardianShieldAnchor anchor)
        {
            Player owner = Main.player[projectile.owner];
            
            OrchidGuardian guardian = owner.GetModPlayer<OrchidGuardian>();
            
            if (AnimTimer % (anchor.isSlamming is 1 or 2 ? 3f : 8f) == 0) AnimFrame++;
            if (AnimFrame == 10)
            {
                AnimFrame = 0;
                AnimTimer = 0;
            }
            
            Color glowColor = new Color(150, 20, 54);
            
            if (projectile.ai[0] > 0f || anchor.isSlamming is 1 or 2) Lighting.AddLight(projectile.Center, glowColor.ToVector3() * (anchor.isSlamming is 1 or 2 ? 1.5f : 1f));
            
            float colorMult = 0.4f + Math.Abs((1f * Main.LocalPlayer.GetModPlayer<OrchidPlayer>().Timer120 - 60) / 120f);
            Vector2 corePosition = owner.MountedCenter + Vector2.UnitX.RotatedBy((projectile.Center - owner.MountedCenter).ToRotation()) * 45 - Main.screenPosition + Vector2.UnitY * owner.gfxOffY;
            Lighting.AddLight(corePosition, glowColor.ToVector3() * (anchor.isSlamming is 1 or 2 ? 1f : colorMult));
            
            
            bool slamButton = ModContent.GetInstance<OrchidClientConfig>().GuardianSwapPaviseImputs ? Main.mouseRight : Main.mouseLeft;
            bool slamButtonRelease = ModContent.GetInstance<OrchidClientConfig>().GuardianSwapPaviseImputs ? Main.mouseRightRelease : Main.mouseLeftRelease;
            
            if (slamButton)
            {
                if (slamButtonRelease) AttemptingCharge = true;
                SlamTimer++;
                
                if (SlamTimer is > 15 and <= 120)
                {
                    CanReleaseEarly = true;
                }
                if (SlamTimer > 120)
                {
                    if (!ChargedSlam)
                    {
                        SoundEngine.PlaySound(SoundID.MaxMana);
                        Dust.NewDustDirect(owner.Center, 8, 8, DustID.LifeDrain);
                        ChargedSlam = true;
                        CanReleaseEarly = false;
                        AttemptingCharge = false;
                    }
                    SlamTimer = 120;
                }
            }
        }
    }

    public override void Slam(Player player, Projectile shield)
    {
        if (ChargedSlam)
        {
            OrchidGuardian guardian = player.GetModPlayer<OrchidGuardian>();
            SoundEngine.PlaySound(SoundID.Item92);
            DustHelper.DrawCircle(shield.Center, DustID.LifeDrain, 4f, dustSize: 2f, nogravity: true);
            Projectile dualcastBall = Projectile.NewProjectileDirect(shield.GetSource_FromThis(), shield.Center, Vector2.UnitX.RotatedBy(shield.rotation + MathHelper.Pi) * 16, ModContent.ProjectileType<ShieldCore_DualcastBall>(), guardian.GetGuardianDamage(Item.damage * 0.5f), shield.knockBack, player.whoAmI);
            dualcastBall.friendly = true;
        }
        SlamTimer = 0;
        ChargedSlam = false;
    }

    public override bool Block(Player player, Projectile shield, Projectile projectile)
    {
        if (projectile.type == ModContent.ProjectileType<ShieldCore_DualcastBall>())
        {
            OrchidGuardian guardian = player.GetModPlayer<OrchidGuardian>();
            
            
            projectile.ai[0]++;
            
            guardian.GuardianShieldSpikeReflect = 0;
            
            projectile.position += Vector2.UnitX.RotatedBy(shield.rotation + MathHelper.Pi) * 15;
            switch (projectile.ai[0])
            {
                case < 5:
                    projectile.damage = (int)(Item.damage * 0.5f * projectile.ai[0]);
                    projectile.timeLeft = 600;
                    projectile.velocity = Vector2.UnitX.RotatedBy(shield.rotation + MathHelper.Pi) * 16 + player.velocity;
                    break;
                case 5:
                    projectile.damage = (int)(Item.damage * 3f);
                    projectile.timeLeft = 60;
                    projectile.velocity = Vector2.UnitX.RotatedBy(shield.rotation + MathHelper.Pi) * 80 + player.velocity;
                    break;
                default:
                    DustHelper.DrawCircle(shield.Center, DustID.LifeDrain, 4f, nogravity: true);
                    SoundEngine.PlaySound(SoundID.NPCDeath56);
                    return true;
            }
            
            CombatText.NewText(projectile.getRect(), new Color(150, 20, 54), (int)projectile.ai[0]);
            
            if (projectile.ai[0] > 6) projectile.ai[0] = 6;
            
            SoundEngine.PlaySound(SoundID.Item94);
            
            shield.ai[0] = 1;
            guardian.AddGuard();
            
            return false;
        }
        return true;
    }

    public override void HoldItemFrame(Player player)
    {
        AnimTimer++;
    }

    public override bool PreDrawShield(SpriteBatch spriteBatch, Projectile projectile, Player player, ref Color lightColor)
    {
        if (projectile.ModProjectile is GuardianShieldAnchor anchor)
        {
            if (projectile.ai[0] > 0f || anchor.isSlamming is 1 or 2)
            {
                Texture2D shieldTexture = ModContent.Request<Texture2D>(ShieldTexture + "_Anim").Value;
                Rectangle frame = shieldTexture.Frame(1, 10, 0, AnimFrame);
                
                Vector2 drawPosition = projectile.Center - Main.screenPosition + Vector2.UnitY * player.gfxOffY;
                SpriteEffects effect = projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                float flippedRotation = projectile.rotation + (projectile.spriteDirection == 1 ? 0 : MathHelper.Pi);
                
                spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
                spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });
                
                RedeDraw.DrawTreasureBagEffect(spriteBatch, shieldTexture, ref DrawTimer, drawPosition, frame, Color.Red * (anchor.isSlamming is 1 or 2 ? 1f : 0.5f), flippedRotation, frame.Size() * 0.5f, projectile.scale);
                spriteBatch.Draw(shieldTexture, drawPosition, frame, lightColor * (0.8f + Math.Abs((1f * Main.LocalPlayer.GetModPlayer<OrchidPlayer>().Timer120 - 60) / 120f)), flippedRotation, frame.Size() * 0.5f, projectile.scale, effect, 0f);
                
                spriteBatch.End();
                spriteBatch.Begin(spriteBatchSnapshot);
            }
        }
        return false;
    }

    public override void PostDrawShield(SpriteBatch spriteBatch, Projectile projectile, Player player, Color lightColor)
    {

        Texture2D coreTexture = ModContent.Request<Texture2D>(ShieldTexture + "Core").Value;
        Rectangle frame = coreTexture.Frame(1, 10, 0, AnimFrame);
        Vector2 drawPosition = player.MountedCenter + Vector2.UnitX.RotatedBy((projectile.Center - player.MountedCenter).ToRotation()) * 45 - Main.screenPosition + Vector2.UnitY * player.gfxOffY;
        SpriteEffects effect = projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
        float colorMult = projectile.ai[1] + projectile.ai[0] > 0 ? 1f : 0.4f + Math.Abs((1f * Main.LocalPlayer.GetModPlayer<OrchidPlayer>().Timer120 - 60) / 120f);
        float flippedRotation = projectile.rotation + (projectile.spriteDirection == 1 ? 0 : MathHelper.Pi);
        
        spriteBatch.Draw(coreTexture, drawPosition, frame, lightColor * colorMult, flippedRotation, frame.Size() * 0.5f, projectile.scale, effect, 0f);
    }
}