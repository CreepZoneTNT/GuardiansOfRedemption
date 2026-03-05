using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using GuardiansOfRedemption.Projectiles.Armor;
using Humanizer;
using Microsoft.Xna.Framework;
using OrchidMod;
using OrchidMod.Content.Guardian;
using OrchidMod.Content.Guardian.Projectiles.Misc;
using Redemption;
using Redemption.BaseExtension;
using Redemption.Buffs.Cooldowns;
using Redemption.Buffs.Debuffs;
using Redemption.Globals;
using Redemption.Items.Armor.HM.Hardlight;
using Redemption.Items.Weapons.HM.Magic;
using Redemption.NPCs.Bosses.Cleaver;
using Redemption.NPCs.Bosses.Gigapora;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.General.Global;

public class RedemptionGuardian : ModPlayer
{

    public NPC EaglecrestShieldTarget;
    public int EaglecrestShieldHitCount;

    public int HardlightParryCooldown;

    public bool GuardianIVDripStandard;

    public bool GuardianHeavyGuard;
    public bool GuardianHardlight;
    
    public bool GuardianSpikeNuclear;
    
    public bool GuardianXenomiteChain;
    public bool GuardianOmegaChain;
    public bool GuardianCosmosChain;

    public OrchidGuardian guardian => Player.GetModPlayer<OrchidGuardian>();
    public override void SetStaticDefaults()
    {
    }

    public override void ResetEffects()
    {
        GuardianHeavyGuard = false;
        GuardianHardlight = false;
        
        GuardianSpikeNuclear = false;
    
        GuardianXenomiteChain = false;
        GuardianOmegaChain = false;
        GuardianCosmosChain = false;
    }

    public override void PostUpdateMiscEffects()
    {
    
        if (EaglecrestShieldTarget != null && !EaglecrestShieldTarget.active) EaglecrestShieldTarget = null;
        HardlightParryCooldown--;
        if (HardlightParryCooldown < 0) HardlightParryCooldown = 0;
    }

    public override void PostUpdateEquips()
    {
        if (GuardianSpikeNuclear)
        {
            guardian.GuardianSpikeTemple = false;
            guardian.GuardianSpikeMech = false;
            guardian.GuardianSpikeDungeon = false;
        }
    }

    public override void ModifyHurt(ref Player.HurtModifiers modifiers)
    {
        if (GuardianHardlight)
        {
            List<Projectile> activeDrones = [];
            
            foreach (var drone in Main.projectile)
                if (drone.owner == Player.whoAmI && drone.type == ModContent.ProjectileType<Hardlight_ParryDrone>() && drone.active && drone.localAI[0] == 0 && !Player.dead) activeDrones.Add(drone);
            
            if (activeDrones.Count > 0 && !Player.immune)
            {
                modifiers.DamageSource.TryGetCausingEntity(out Entity entity);
                DustHelper.DrawCircle(Player.Center, DustID.Vortex, 4f, dustSize: 1.5f, nogravity: true);
                activeDrones[0].Kill();
                if (Player.HeldItem.ModItem is OrchidModGuardianParryItem) guardian.DoParryItemParry(entity);
                else Player.SetImmuneTimeForAllTypes((Player.longInvince ? 30 : 15) + guardian.ParryInvincibilityBonus);
                modifiers.Cancel();
            }
        }
    }

    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        if (Redemption.Redemption.RedeSpecialAbility.JustPressed && Player.active && !Player.dead)
        {
            int count = 0;
            foreach (var drone in Main.projectile)
                if (drone.owner == Player.whoAmI && drone.ModProjectile is Hardlight_ParryDrone && drone.active && !Player.dead) count++;
            if (GuardianHardlight && !Player.HasBuff<HardlightCooldown>() && count == 0)
            {
                Player.AddBuff(ModContent.BuffType<HardlightCooldown>(), 3600);
                if (!Main.dedServ) SoundEngine.PlaySound(CustomSounds.Alarm2, Player.position);
                int projType = ModContent.ProjectileType<Hardlight_ParryDrone>();
                for (int i = 0; i < 3; i++)
                {
                    Vector2 spawnPos = new Vector2(Player.Center.X + Main.rand.Next(-200, 201), Player.Center.Y - 800f);
                    Projectile drone = Projectile.NewProjectileDirect(Player.GetSource_FromThis(), spawnPos, Vector2.Zero, projType, -1, 0, Main.myPlayer);
                    drone.timeLeft = ContentSamples.ProjectilesByType[projType].timeLeft + i;
                }
            }
        }
    }
}