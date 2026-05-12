using GuardiansOfRedemption.General;
using GuardiansOfRedemption.Items.Guardian.Weapons.Runes;
using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Content.Guardian;
using rail;
using Redemption.Dusts;
using Redemption.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Items.Guardian.Projectiles.Runes
{
    public class MoonflareRune_Projectile : GuardianRuneProjectile
    {
        public Player owner;
        public List<Vector2> OldPosition;
        public List<float> OldRotation;
        public Texture2D texture;

        public override void SetStaticDefaults()
        {
            ElementID.ProjFire[Type] = true;
            ElementID.ProjNature[Type] = true;
            ElementID.ProjArcane[Type] = true;
        }
        public override void RuneSetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.aiStyle = 0;
            Projectile.scale = 1f;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Main.projFrames[Projectile.type] = 8;
            OldPosition = new List<Vector2>();
            OldRotation = new List<float>();

        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void FirstFrame()
        {
            OldPosition = new List<Vector2>();
            OldRotation = new List<float>();
            texture = TextureAssets.Projectile[this.Type].Value;
            if (MoonflareRune.Boosted)
            {
                Projectile.scale = 1.3f;
            }

        }

        public override bool SafeAI()
        {
            Player owner = Owner;
            Vector2 pointToPlayer = Owner.Center - Projectile.Center;
            float rotation = pointToPlayer.ToRotation();
            Spin(1f);
            SetDistance(150f);

            Projectile.rotation = rotation;
            OldPosition.Add(Projectile.Center);
            OldRotation.Add(Projectile.rotation);

            float damageMult = Projectile.frame switch
            {
                0 => 0.5f,
                1 or 7 => 0.75f,
                3 or 5 => 1.25f,
                4 => 1.5f,
                _ => 1f,
            };

            Projectile.damage = Owner.Guardian().GetGuardianDamage(baseDamage * damageMult);

            if (++Projectile.frameCounter >= 180)
            {
                Projectile.frameCounter = 0;
                Projectile.ai[1] = -270f;
                //SpinSpeed *= -1;
                SoundEngine.PlaySound(SoundID.Item130);
                DustHelper.DrawCircle(Projectile.Center, ModContent.DustType<MoonflareDust>(), 3, 2, 2, nogravity: true);

                if (++Projectile.frame >= Main.projFrames[Type])
                    Projectile.frame = 0;
            }

            return true;
        }
    }
}
