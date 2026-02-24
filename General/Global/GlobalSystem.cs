using System;
using System.Reflection;
using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour;
using OrchidMod;
using OrchidMod.Content.Guardian;
using OrchidMod.Content.Guardian.Projectiles.Misc;
using Redemption.Globals;
using Redemption.NPCs.Bosses.Cleaver;
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
    private Hook _ResetStandardsHook;
    

    private delegate void orig_OnBlockProjectile(OrchidGuardian self, Projectile anchor, Projectile blockedProjectile, bool parry = false);
    private delegate void orig_ResetStandards(OrchidGuardian self, bool forceReset = false);
    
    public override void Load()
    {
        MethodInfo onBlockProjectile = typeof(OrchidGuardian).GetMethod("OnBlockProjectile", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
        if (onBlockProjectile != null)
            _onBlockProjectileHook = new Hook(onBlockProjectile, Detour_OnBlockProjectile);
        MethodInfo resetStandards = typeof(OrchidGuardian).GetMethod("ResetStandards", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
        if (resetStandards != null)
            _ResetStandardsHook = new Hook(resetStandards, Detour_ResetStandards);
            
    }
    private void Detour_OnBlockProjectile(orig_OnBlockProjectile orig, OrchidGuardian self, Projectile anchor, Projectile blockedProjectile, bool parry = false)
    {
        
        Player player = self.Player;
        
        // CombatText.NewText(player.getRect(), Color.Wheat, "test");
        
        int damage = Math.Max(self.GetGuardianDamage(player.statDefense * self.GuardianSpikeDamage), 1);
        if (self.GuardianShieldSpikeReflect > 0 && self.GuardianSpikeDamage > 0 && anchor.ModProjectile is GuardianShieldAnchor)
        {
            if (player.GetModPlayer<RedemptionGuardian>().GuardianSpikeNuclear)
            {
                int type = ModContent.ProjectileType<PlutoniumBeam>();
                Vector2 dir = Vector2.Normalize(anchor.Center - player.Center);
                Projectile projectile = Projectile.NewProjectileDirect(anchor.GetSource_FromAI(), anchor.Center + dir * 2f, dir, type, damage, 1f, player.whoAmI);
                projectile.CritChance = (int)(player.GetCritChance<GuardianDamageClass>() + player.GetCritChance<GenericDamageClass>());
                SoundEngine.PlaySound(SoundID.Item91.WithPitchOffset(0.2f).WithVolumeScale(0.6f), anchor.Center);
                SoundEngine.PlaySound(SoundID.Item68.WithPitchOffset(0.6f).WithVolumeScale(0.5f), anchor.Center);
            }
        }
        orig(self, anchor, blockedProjectile, parry);
    }
    
    private void Detour_ResetStandards(orig_ResetStandards orig, OrchidGuardian self, bool forceReset = false)
    {
        RedemptionGuardian addonGuardian = self.Player.GetModPlayer<RedemptionGuardian>();
        if (forceReset || !self.GuardianStandardBuffer)
        {
            addonGuardian.GuardianIVDripStandard = false;
        }
        else orig(self, forceReset);
    }

    public override void Unload()
    {
        _onBlockProjectileHook?.Dispose();
        _onBlockProjectileHook = null;
        
        _ResetStandardsHook?.Dispose();
        _ResetStandardsHook = null;
    }
}