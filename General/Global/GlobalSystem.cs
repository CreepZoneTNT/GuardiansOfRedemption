using System;
using System.Reflection;
using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour;
using OrchidMod;
using OrchidMod.Content.Guardian;
using OrchidMod.Content.Guardian.Projectiles.Misc;
using Redemption.Projectiles.Melee;
using Redemption.Projectiles.Ranged;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.General.Global;

public class GlobalSystem : ModSystem
{
    private Hook _onBlockProjectileHook;
    

    private delegate void orig_OnBlockProjectile(OrchidGuardian self, Projectile anchor, Projectile blockedProjectile, bool parry = false);
    
    public override void Load()
    {
    
        MethodInfo targetMethod = typeof(OrchidGuardian).GetMethod("OnBlockProjectile", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
        if (targetMethod != null)
            _onBlockProjectileHook = new Hook(targetMethod, Detour_OnBlockProjectile);
    }
    private void Detour_OnBlockProjectile(orig_OnBlockProjectile orig, OrchidGuardian self, Projectile anchor, Projectile blockedProjectile, bool parry = false)
    {
        
        Player player = self.Player;
        
        CombatText.NewText(player.getRect(), Color.Wheat, "test");
        
        int damage = Math.Max(self.GetGuardianDamage(player.statDefense * self.GuardianSpikeDamage), 1);
        if (self.GuardianShieldSpikeReflect > 0 && self.GuardianSpikeDamage > 0 && anchor.ModProjectile is GuardianShieldAnchor)
        {
            if (player.GetModPlayer<MoRGuardianPlayer>().GuardianSpikeNuclear)
            {
                int type = ModContent.ProjectileType<PlutoniumBeam>();
                Vector2 dir = Vector2.Normalize(anchor.Center - player.Center);
                Projectile projectile = Projectile.NewProjectileDirect(anchor.GetSource_FromAI(), anchor.Center + dir * 2f, dir, type, damage, 1f, player.whoAmI);
                projectile.CritChance = (int)(player.GetCritChance<GuardianDamageClass>() + player.GetCritChance<GenericDamageClass>());
                SoundEngine.PlaySound(SoundID.Item91.WithPitchOffset(0.2f).WithVolumeScale(0.6f), anchor.Center);
                SoundEngine.PlaySound(SoundID.Item68.WithPitchOffset(0.6f).WithVolumeScale(0.5f), anchor.Center);
            }
        }
        else orig(self, anchor, blockedProjectile, parry);
    }

    public override void Unload()
    {
        _onBlockProjectileHook?.Dispose();
        _onBlockProjectileHook = null;
    }
}