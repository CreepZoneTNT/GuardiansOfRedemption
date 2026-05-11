using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Core.Utils;
using OrchidMod;
using OrchidMod.Content.Shapeshifter;
using OrchidMod.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuardiansOfRedemption.Buffs.Debuffs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using GuardiansOfRedemption.General.Global;
using Steamworks;

namespace GuardiansOfRedemption.Items.Shapeshifter.Weapons.Sage
{
    internal class SageBasan_Proj : OrchidModShapeshifterProjectile
    {
        private static Texture2D TextureMain;
        public List<Vector2> OldPosition;
        public List<float> OldRotation;
        public int Timespent = 0;

        public override void SafeSetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 20;
            Projectile.alpha = 96;
            Projectile.scale = 1.8f;
            Main.projFrames[Projectile.type] = 2;

            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 96;
            Projectile.penetrate = -1;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 81;

            OldPosition = new List<Vector2>();
            OldRotation = new List<float>();
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 90;
        }
        public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidShapeshifter shapeshifter)
        {
            int additiveDuration = target.GetGlobalNPC<GlobalNPCs>().BasanDebuffDuration;

            if (target.HasBuff(ModContent.BuffType<BasanBurnDebuff>()))
                target.AddBuff(ModContent.BuffType<BasanBurnDebuff>(), additiveDuration + 80);
        }

        public override void AI()
        {
            OldPosition.Add(Projectile.Center);
            OldRotation.Add(Projectile.rotation);

            if (!Initialized)
            {
                Initialized = true;

                for (int i = 0; i < 15; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Torch);
                    dust.scale = Main.rand.NextFloat(1.5f, 2f);
                    dust.noGravity = true;
                    dust.velocity *= 0.5f;
                    dust.velocity += Vector2.Normalize(Projectile.velocity).RotatedByRandom(MathHelper.ToRadians(30f)) * Main.rand.NextFloat(5f, 8f);
                }

                for (int i = 0; i < 5; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Torch);
                    dust.scale = Main.rand.NextFloat(1.5f, 2f);
                    dust.noGravity = true;
                    dust.velocity *= 0.5f;
                    dust.velocity += Vector2.Normalize(Projectile.velocity).RotatedByRandom(MathHelper.ToRadians(20f)) * Main.rand.NextFloat(10f, 15f);
                }
            }

            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.velocity *= 0.94574f;
            Projectile.alpha -= Timespent;


            if (OldPosition.Count > 10)
            {
                OldPosition.RemoveAt(0);
                OldRotation.RemoveAt(0);
            }

            if (Main.rand.NextBool(4 - (int)Projectile.ai[1]))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch);
                dust.velocity *= 0.25f;
                dust.velocity.Y -= 1f;
                dust.velocity += Projectile.velocity * 0.2f;
                dust.noLight = true;
            }

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch);
                dust.velocity *= 0.25f;
                dust.velocity += Projectile.velocity * 0.3f;
                dust.scale = Main.rand.NextFloat(1f, 1.5f);
                dust.noGravity = true;
            }
        }
    }
}