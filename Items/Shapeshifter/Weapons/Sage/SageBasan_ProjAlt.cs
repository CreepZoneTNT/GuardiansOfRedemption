using OrchidMod;
using OrchidMod.Content.Shapeshifter;
using GuardiansOfRedemption.Buffs.Debuffs;
using Redemption.Globals;
using Redemption.Particles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using XPT.Core.Audio.MP3Sharp.Decoding.Decoders.LayerIII;

namespace GuardiansOfRedemption.Items.Shapeshifter.Weapons.Sage
{
    public class SageBasan_ProjAlt : OrchidModShapeshifterProjectile
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
            Projectile.scale = 0.4f;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 3;
            Projectile.timeLeft = 90;
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.6f, 0.3f, 0f);
            Projectile.scale += 0.01f;

            //RedeParticleManager.CreateEmberParticle(RedeHelper.RandAreaInEntity(Projectile), Projectile.velocity * 0.5f, Projectile.scale * 0.6f, Main.rand.Next(90, 121), 10);
            RedeParticleManager.CreateEmberBurstParticle(RedeHelper.RandAreaInEntity(Projectile), Projectile.velocity * 0.5f, Projectile.scale * 1f, Main.rand.Next(12, 17), .9f);
            
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.OnFire3, 600);
        }
        public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidShapeshifter shapeshifter)
        {
            target.AddBuff(ModContent.BuffType<BasanBurnDebuff>(), 300);
        }
    }
}
