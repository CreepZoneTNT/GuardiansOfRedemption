  using System.Collections.Generic;
using System.Reflection;
using System.Text;
using GuardiansOfRedemption.Items.Weapons.Gauntlets;
using GuardiansOfRedemption.Items.Weapons.Standards;
using GuardiansOfRedemption.Projectiles.Armor;
using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour;
using OrchidMod;
using OrchidMod.Content.Guardian;
using OrchidMod.Content.Guardian.Misc;
using Redemption;
using Redemption.Buffs.Cooldowns;
using Redemption.Buffs.Debuffs;
using Redemption.Buffs.NPCBuffs;
using Redemption.Globals;
using Redemption.Projectiles.Magic;
using Redemption.Projectiles.Misc;
using Redemption.Projectiles.Ranged;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.General.Global;

public class RedemptionGuardian : ModPlayer
{

    public NPC EaglecrestShieldTarget;
    public int EaglecrestShieldHitCount;

    public int HardlightParryCooldown;

    public bool GuardianChickenStandard;
    public bool GuardianPureIronStandard;
    public int GuardianPureIronStandardCooldown;
    public bool GuardianDragonLeadStandard;
    public int GuardianDragonLeadStandardCooldown;
    public bool GuardianIVDripStandard;

    public bool GuardianHeavyGuard;
    public bool GuardianHardlight;
    public bool GuardianCommonGuard;

    public bool GuardianProtectiveAmulet;

    public bool GuardianSpikeNuclear;
    
    public bool GuardianXenomiteChain;
    public bool GuardianOmegaChain;
    public bool GuardianCosmosChain;

    public OrchidGuardian Guardian => Player.GetModPlayer<OrchidGuardian>();

    public override void SetStaticDefaults()
    {
        
    }

    public override void ResetEffects()
    {
        if (EaglecrestShieldTarget != null && !EaglecrestShieldTarget.active) EaglecrestShieldTarget = null;

        GuardianPureIronStandardCooldown--;
        if (GuardianPureIronStandardCooldown < 0) GuardianPureIronStandardCooldown = 0;
        GuardianDragonLeadStandardCooldown--;
        if (GuardianDragonLeadStandardCooldown < 0) GuardianDragonLeadStandardCooldown = 0;

        HardlightParryCooldown--;
        if (HardlightParryCooldown < 0) HardlightParryCooldown = 0;

        GuardianHeavyGuard = false;
        GuardianHardlight = false;

        GuardianProtectiveAmulet = false;

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
        
        // bool hasLabGauntletBoost = false;
        // foreach (var proj in Main.projectile)
        //     if (proj.ModProjectile is LaboratoryGauntletProjectile && proj.owner == Main.myPlayer) hasLabGauntletBoost = true;
        // Player.controlDown = hasLabGauntletBoost;
    }

    public override void PostUpdateEquips()
    {
        if (GuardianSpikeNuclear)
        {
            Guardian.GuardianSpikeTemple = false;
            Guardian.GuardianSpikeMech = false;
            Guardian.GuardianSpikeDungeon = false;
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
                if (Player.HeldItem.ModItem is OrchidModGuardianParryItem) Guardian.DoParryItemParry(entity);
                else Player.SetImmuneTimeForAllTypes((Player.longInvince ? 30 : 15) + Guardian.ParryInvincibilityBonus);
                modifiers.Cancel();
            }
        }
    }

    public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright) {
        // if (GuardianPureIronStandard) {
        //     r = 0.6f;
        //     g = 0.6f;
        //     b = 0.95f;
        // }
        // if (GuardianDragonLeadStandard) {
        //     r = 0.95f;
        //     g = 0.6f;
        //     b = 0.6f;
        // }
    }
    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {
        if (GuardianPureIronStandard) {
            if (Main.rand.NextBool(3)) target.AddBuff(ModContent.BuffType<PureChillDebuff>(), 180);
        }
        if (GuardianDragonLeadStandard) {
            if (Main.rand.NextBool(3)) target.AddBuff(ModContent.BuffType<DragonblazeDebuff>(), 180);
        }
    }

    public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)
    {
        if (GuardianPureIronStandard && GuardianPureIronStandardCooldown == 0)
            SpawnPureIronStandardCrystals(target, Guardian.GetGuardianDamage(item.damage * 0.4f));
        if (GuardianDragonLeadStandard && GuardianDragonLeadStandardCooldown == 0)
            SpawnDragonLeadStandardFire(target, Guardian.GetGuardianDamage(item.damage * 0.4f));
    }

    public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
    {
        if (GuardianPureIronStandard && GuardianPureIronStandardCooldown == 0 && proj.owner == Player.whoAmI && proj.ModProjectile is not PureIronStandard_IceShardProj)
            SpawnPureIronStandardCrystals(target, Guardian.GetGuardianDamage(proj.damage * 0.4f));
        if (GuardianDragonLeadStandard && GuardianDragonLeadStandardCooldown == 0 && proj.owner == Player.whoAmI && proj.ModProjectile is not DragonSkullFlames_Proj)
            SpawnDragonLeadStandardFire(target, Guardian.GetGuardianDamage(proj.damage * 0.4f));
    }

    public void SpawnPureIronStandardCrystals(NPC target, int damage) {
        DustHelper.DrawCircle(target.Center, DustID.IceTorch, 1, 4, 4, nogravity: true);
        for (int i = 0; i < 3; i++) 
        Projectile.NewProjectileDirect(
            target.GetSource_FromAI(),
            target.Top,
            Vector2.UnitY.RotatedBy(Main.rand.NextFloat(-MathHelper.Pi/5, MathHelper.Pi/5)) * -6f, 
            ModContent.ProjectileType<PureIronStandard_IceShardProj>(), 
            damage, 
            4, 
            Player.whoAmI
        );
        SoundEngine.PlaySound(SoundID.Item28, target.Center);
        GuardianPureIronStandardCooldown = 12;
    }

    public void SpawnDragonLeadStandardFire(NPC target, int damage) {
        DustHelper.DrawCircle(target.Center, DustID.Torch, 1, 4, 4, nogravity: true);
        for (int i = 0; i < 3; i++) 
        Projectile.NewProjectileDirect(
            target.GetSource_FromAI(),
            target.Top,
            Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) * 6f, 
            ModContent.ProjectileType<DragonBreath>(), 
            damage, 
            4, 
            Player.whoAmI
        );
        SoundEngine.PlaySound(SoundID.Item34, target.Center);
        GuardianDragonLeadStandardCooldown = 12;
    }
    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        if (Redemption.Redemption.RedeSpecialAbility.JustPressed && Player.active && !Player.dead)
        {
            if (GuardianHardlight)
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
                        Vector2 spawnPos = new(Player.Center.X + Main.rand.Next(-200, 201), Player.Center.Y - 800f);
                        Projectile drone = Projectile.NewProjectileDirect(Player.GetSource_FromThis(), spawnPos, Vector2.Zero, projType, -1, 0, Main.myPlayer);
                        drone.timeLeft = ContentSamples.ProjectilesByType[projType].timeLeft + i;
                    }
                }
                if (GuardianCommonGuard)
                {
                    if (GuardianCommonGuard && !Player.HasBuff<CommonGuardFlagCooldown>())
                    {
                        Player.AddBuff(ModContent.BuffType<CommonGuardFlagCooldown>(), 600);

                        foreach (Projectile proj in Main.ActiveProjectiles)
                        {
                            if (proj.type != ModContent.ProjectileType<CommonGuardFlag_Proj>() || proj.owner != Player.whoAmI)
                                continue;
                            proj.timeLeft = 2;
                        }

                        Projectile.NewProjectileDirect(Player.GetSource_FromThis(), Player.Center - new Vector2(0, 96), Vector2.Zero, ModContent.ProjectileType<CommonGuardFlag_Proj>(), 0, 0, Main.myPlayer);
                    }
                }                  
            }
        }
    }
}
