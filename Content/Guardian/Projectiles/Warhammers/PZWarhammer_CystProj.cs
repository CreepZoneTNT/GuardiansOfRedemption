using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod;
using OrchidMod.Content.Guardian;
using Redemption.Dusts;
using Redemption.Globals;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;


namespace GuardiansOfRedemption.Content.Guardian.Projectiles.Warhammers;

public class PZWarhammer_CystProj : OrchidModGuardianProjectile
{
    public Vector2 Offset;

    public override void SafeSetDefaults()
    {
        Projectile.width = 28;
        Projectile.height = 28;
        Projectile.penetrate = -1;
        Projectile.hostile = false;
        Projectile.friendly = true;
        Projectile.tileCollide = false;
        Projectile.timeLeft = 600;
        Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 30;
    }
    public override void OnSpawn(IEntitySource source)
    {
        Projectile.scale = 0.2f * Projectile.ai[2];

        SoundEngine.PlaySound(SoundID.Item95, Projectile.position);

        NPC latchedNPC = Main.npc[(int)Projectile.ai[0]];

        Offset = Main.rand.NextVector2FromRectangle(latchedNPC.Hitbox) - latchedNPC.Center;

        for (int i = 0; i < 15; i++)
        {
            int dustIndex = Dust.NewDust(Projectile.Center - new Vector2(10), 20, 20, ModContent.DustType<SludgeDust>());
        }
    }

    public override bool? CanHitNPC(NPC target)
    {
        if (Projectile.timeLeft > 540 || target.whoAmI != (int)Projectile.ai[0])
            return false;

        return base.CanHitNPC(target);
    }

    public override void AI()
    {
        NPC latchedNPC = Main.npc[(int)Projectile.ai[0]];
        
        if (latchedNPC != null && latchedNPC.active && !latchedNPC.friendly)
        {
            Projectile.Center = latchedNPC.Center + Offset;
            if (Main.rand.NextBool(12)) Dust.NewDustDirect(Projectile.Center - new Vector2(10), 20, 20, ModContent.DustType<SludgeDust>());
        }
        if (!latchedNPC.active)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath22, Projectile.position);
            for (int i = 0; i < 25; i++)
            {
                Dust.NewDustDirect(Projectile.Center - new Vector2(10), 20, 20, ModContent.DustType<SludgeDust>());
            }
            Projectile.Kill();
        }

        if (Projectile.scale < Projectile.ai[2])
            Projectile.scale += (0.05f * Projectile.ai[2]);
        else if (Projectile.scale > Projectile.ai[2])
            Projectile.scale -= (0.05f * Projectile.ai[2]);

        if (Projectile.timeLeft < 100)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath22, Projectile.position);
            for (int i = 0; i < 15; i++)
            {
                int dustIndex = Dust.NewDust(Projectile.Center - new Vector2(10), 20, 20, ModContent.DustType<SludgeDust>());
            }
            Projectile.Kill();
        }
    }

    public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidGuardian guardian)
    { 
        NPC latchedNPC = Main.npc[(int)Projectile.ai[0]];

        if (latchedNPC != null && latchedNPC.active && !latchedNPC.friendly && target.whoAmI == (int)Projectile.ai[0])
        {
            Projectile.scale = 1.3f * Projectile.ai[2];
        }
    }


    public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
    {
        Texture2D drawTexture = ModContent.Request<Texture2D>(Texture).Value;
        Rectangle frame = drawTexture.Frame(1, 8, 0, (int)Projectile.ai[1]);

        Main.EntitySpriteDraw(drawTexture, Projectile.Center - Main.screenPosition, frame, lightColor, Projectile.rotation, frame.Size() * 0.5f, Projectile.scale, 0);

        return false;
    }
}