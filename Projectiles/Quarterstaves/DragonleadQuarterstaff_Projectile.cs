using Microsoft.Xna.Framework;
using OrchidMod;
using OrchidMod.Content.Guardian;
using ParticleLibrary;
using Redemption.BaseExtension;
using Redemption.Buffs.NPCBuffs;
using Redemption.Globals;
using Redemption.Particles;
using Redemption.Projectiles.Magic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Projectiles.Quarterstaves
{
    public class DragonleadQuarterstaff_Projectile : OrchidModGuardianProjectile
    {
        public override string Texture => "Redemption/Empty";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Flames");
            ElementID.ProjArcane[Type] = true;
            ElementID.ProjFire[Type] = true;
        }
        public override void SafeSetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 3;
            Projectile.timeLeft = 120;
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.6f, 0.3f, 0f);
            Projectile.velocity.Y -= 0.01f;
            if (Projectile.ai[0]++ > 0)
            {
                float scale = 1f;
                float velIncrease = 1f;
                if (Projectile.ai[0] == 1)
                    scale = 0.25f;
                else if (Projectile.ai[0] == 2)
                    scale = 0.5f;
                else if (Projectile.ai[0] == 3)
                    scale = 0.75f;

                if (Main.rand.NextBool(3))
                {
                    scale *= 0.5f;
                    velIncrease *= 2f;
                }
                if (!Main.rand.NextBool(6))
                    scale *= 1.5f;

                if (Main.rand.NextBool(Projectile.ai[0] > 70 ? 40 : 20))
                    RedeParticleManager.CreateEmberParticle(RedeHelper.RandAreaInEntity(Projectile), Projectile.velocity * 0.5f * velIncrease, scale * 0.6f, Main.rand.Next(90, 121), 10);
                if (Main.rand.NextBool(Projectile.ai[0] > 70 ? 4 : 2))
                    RedeParticleManager.CreateEmberBurstParticle(RedeHelper.RandAreaInEntity(Projectile), Projectile.velocity * 0.5f * velIncrease, scale * 1f, Main.rand.Next(12, 17), .9f);
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.OnFire, 200);
        }
        public override void SafeModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Projectile.ModProjectile is DragonleadQuarterstaff_Projectile && NPCLists.Dragonlike.Contains(target.type))
                modifiers.FinalDamage *= 4;
        }
        public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidGuardian guardian)
        {
            target.AddBuff(BuffID.OnFire, 200);

            if (Projectile.ModProjectile is DragonleadQuarterstaff_Projectile && Main.player[Projectile.owner].RedemptionPlayerBuff().dragonLeadBonus)
                target.AddBuff(ModContent.BuffType<DragonblazeDebuff>(), 300);
        }
        public override void OnKill(int timeLeft)
        {
            RedeParticleManager.CreateEmberBurstParticle(RedeHelper.RandAreaInEntity(Projectile), Projectile.velocity * 4, 5f, Main.rand.Next(20, 30), .9f);
        }
    }
}
