using GuardiansOfRedemption.Items.Guardian.Projectiles.Shields;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mono.Cecil;
using MonoMod.RuntimeDetour;
using OrchidMod;
using OrchidMod.Content.Guardian;
using OrchidMod.Content.Guardian.Misc;
using OrchidMod.Content.Guardian.Projectiles.Misc;
using Redemption.Globals;
using Redemption.NPCs.Bosses.Cleaver;
using Redemption.Projectiles.Melee;
using Redemption.Projectiles.Ranged;
using System;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static System.Net.Mime.MediaTypeNames;

namespace GuardiansOfRedemption.General.Global;

public class GoRHooks : ModSystem
{
    bool ProtectiveAmuletTriggered = false;
    private Hook _onBlockFirstHook;
    private Hook _onBlockProjectileHook;
    private Hook _DoParryItemParry;
    private Hook _ResetStandardsHook;


    private delegate void orig_OnBlockAnyFirst(OrchidGuardian self, Projectile anchor, ref int toAdd, bool parry = false);
    private delegate void orig_OnBlockProjectile(OrchidGuardian self, Projectile anchor, Projectile blockedProjectile, bool parry = false);
    private delegate void orig_DoParryItemParry(OrchidGuardian self, Entity aggressor);
    private delegate void orig_ResetStandards(OrchidGuardian self, bool forceReset = false);
    
    public override void Load()
    {
        MethodInfo onBlockAnyFirst = typeof(OrchidGuardian).GetMethod("OnBlockAnyFirst", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
        if (onBlockAnyFirst != null)
            _onBlockFirstHook = new Hook(onBlockAnyFirst, Detour_OnBlockAnyFirst);

        MethodInfo onBlockProjectile = typeof(OrchidGuardian).GetMethod("OnBlockProjectile", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
        if (onBlockProjectile != null)
            _onBlockProjectileHook = new Hook(onBlockProjectile, Detour_OnBlockProjectile);

        MethodInfo DoParryItemParry = typeof(OrchidGuardian).GetMethod("DoParryItemParry", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
        if (DoParryItemParry != null)
            _DoParryItemParry = new Hook(DoParryItemParry, Detour_DoParryItemParry);

        MethodInfo resetStandards = typeof(OrchidGuardian).GetMethod("ResetStandards", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
        if (resetStandards != null)
            _ResetStandardsHook = new Hook(resetStandards, Detour_ResetStandards);
            
    }

    private void Detour_OnBlockAnyFirst(orig_OnBlockAnyFirst orig, OrchidGuardian self, Projectile anchor, ref int toAdd, bool parry = false)
    {

        Player player = self.Player;

        // CombatText.NewText(player.getRect(), Color.Wheat, "test");
        if(player.GetModPlayer<RedemptionGuardian>().GuardianProtectiveAmulet)
        {
           player.Heal(3);
        }

        if(player.GetModPlayer<RedemptionGuardian>().GuardianBasan)
        {
            for (int i = 0; i < 5; i++)
            {
                Vector2 position = Main.MouseWorld;
                Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, position.DirectionTo(Main.MouseWorld), ProjectileID.BloodArrow, 30, 0, player.whoAmI);
            }
        }

        orig(self, anchor, ref toAdd, parry);
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

    private void Detour_DoParryItemParry(orig_DoParryItemParry orig, OrchidGuardian self, Entity aggressor)
    {
        Player player = self.Player;

        // CombatText.NewText(player.getRect(), Color.Wheat, "test");
        if (player.GetModPlayer<RedemptionGuardian>().GuardianProtectiveAmulet)
        {
            if (!ProtectiveAmuletTriggered)
            {
                player.Heal(3);
                ProtectiveAmuletTriggered = true;
            }
        }

        for (int i = 0; i < 5; i++)
        {
            Vector2 position = Main.MouseWorld;
            Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, position.DirectionTo(Main.MouseWorld), ProjectileID.BloodArrow, 30, 0, player.whoAmI);
        }
        orig(self, aggressor);
    }
    private void Detour_ResetStandards(orig_ResetStandards orig, OrchidGuardian self, bool forceReset = false)
    {
        RedemptionGuardian addonGuardian = self.Player.GetModPlayer<RedemptionGuardian>();
        if (forceReset || !self.GuardianStandardBuffer)
        {
            addonGuardian.GuardianChickenStandard = false;
            addonGuardian.GuardianPureIronStandard = false;
            addonGuardian.GuardianPureIronStandardCooldown = 0;
            addonGuardian.GuardianDragonLeadStandard = false;
            addonGuardian.GuardianDragonLeadStandardCooldown = 0;
            addonGuardian.GuardianIVDripStandard = false;
        }
        else orig(self, forceReset);
    }

    public override void Unload()
    {
        _onBlockFirstHook?.Dispose();
        _onBlockFirstHook = null;

        _onBlockProjectileHook?.Dispose();
        _onBlockProjectileHook = null;

        _DoParryItemParry?.Dispose();
        _DoParryItemParry = null;
        
        _ResetStandardsHook?.Dispose();
        _ResetStandardsHook = null;
    }
}