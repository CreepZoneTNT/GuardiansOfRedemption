using GuardiansOfRedemption.Buffs;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod;
using OrchidMod.Content.Guardian;
using Redemption;
using Redemption.BaseExtension;
using Redemption.Buffs.NPCBuffs;
using Redemption.Globals;
using Redemption.Rarities;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace GuardiansOfRedemption.Items.Guardian.Projectiles.Warhammers;

public class PZWarhammer_CloudProj : OrchidModGuardianProjectile
{
    public override string Texture => "Redemption/Textures/IceMist";
    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("Virulent Gas");
    }
    public override void SafeSetDefaults()
    {
        Projectile.width = 150;
        Projectile.height = 150;
        Projectile.penetrate = -1;
        Projectile.hostile = false;
        Projectile.friendly = true;
        Projectile.tileCollide = false;
        Projectile.alpha = 255;
        Projectile.timeLeft = 240;
        Projectile.scale = Main.rand.NextFloat(1, 1.5f);
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 120;
        }
    public override void AI()
    {
        if (Projectile.localAI[0] == 0)
            Projectile.localAI[0] = Main.rand.Next(1, 3);

        if (Projectile.localAI[0] == 1)
            Projectile.rotation -= 0.003f;
        else if (Projectile.localAI[0] == 2)
            Projectile.rotation += 0.003f;

        if (Projectile.timeLeft < 80)
        {
            Projectile.alpha += 20;
            if (Projectile.alpha >= 255)
                Projectile.Kill();
        }
        else
        {
            Projectile.alpha -= 5;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC target = Main.npc[i];
                if (!target.active || !target.CanBeChasedBy())
                    continue;

                if (!Projectile.Hitbox.Intersects(target.Hitbox))
                    continue;

                ;
                target.AddBuff(ModContent.BuffType<ViralityDebuff>(), 200);
            }
        }
    }
    
    public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
    {
        Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
        Vector2 drawOrigin = new(texture.Width / 2, texture.Height / 2);
        var effects = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

        Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(Color.ForestGreen), Projectile.rotation, drawOrigin, Projectile.scale, effects, 0);
        return false;
    }
}