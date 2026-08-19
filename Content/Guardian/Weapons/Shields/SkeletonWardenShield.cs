using GuardiansOfRedemption.Content.Guardian.Projectiles.Shields;
using GuardiansOfRedemption.General;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.Guardian;
using OrchidMod.Content.Guardian.Projectiles.Shields;
using OrchidMod.Utilities;
using Redemption;
using Redemption.Items.Materials.PreHM;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpriteBatchSnapshot = OrchidMod.Utilities.SpriteBatchSnapshot;

namespace GuardiansOfRedemption.Content.Guardian.Weapons.Shields;

public class SkeletonWardenShield : OrchidModGuardianShield
{
    public int DamageState = 0;
    public override void SafeSetDefaults()
    {
        Item.value = Item.sellPrice(0, 0, 80);
        Item.width = 34;
        Item.height = 42;
        Item.knockBack = 12f;
        Item.damage = 60;
        Item.rare = ItemRarityID.Blue;
        Item.useTime = 50;
        distance = 50f;
        slamDistance = 100f;
        ShieldFrames = 3;
        blockDuration = 140;
        slamAutoReuse = false;
        shouldFlip = true;
    }

    static void ResetState(Projectile shield)
    {
        GuardianShieldAnchor anchor = shield.ModProjectile as GuardianShieldAnchor;
        shield.friendly = false;
        shield.ai[1] = shield.ai[0] = anchor.isSlamming = 0;
        anchor.NeedNetUpdate = true;
    }

    void ShieldBreak(Player player, Projectile shield, float baseSpeed = 0, int quantity = 3, bool fanOut = true)
    {
        OrchidGuardian guardian = Main.LocalPlayer.Guardian();
        Terraria.Audio.SoundEngine.PlaySound(CustomSounds.GuardBreak, shield.position);
        float dir = (Main.MouseWorld - player.Center).ToRotation();
        Vector2 spread = new Vector2(0, 1).RotatedBy(dir);
        for (int i = 0; i < quantity; i++)
        {
            float side = (i - ((quantity - 1) / 2f)) * -shield.direction;
            Projectile newProjectile = Projectile.NewProjectileDirect(Item.GetSource_FromThis(), shield.Center, new Vector2(baseSpeed, 0).RotatedBy(dir) + spread.RotatedByRandom(Main.rand.NextFloat()) * side, ModContent.ProjectileType<SkeletonWardenShield_BrokenShieldProj>(), 0, Item.knockBack, player.whoAmI);
            if (!fanOut) side = 0;
            newProjectile.damage = guardian.GetGuardianDamage(Item.damage * 0.8f);
            newProjectile.CritChance = (int)(player.GetCritChance<GuardianDamageClass>() + player.GetCritChance<GenericDamageClass>() + Item.crit);
            newProjectile.position += newProjectile.velocity * 4;
            int ironDust = Dust.NewDust(shield.Center - new Vector2(4, 4), 0, 0, DustID.Iron);
            int woodDust = Dust.NewDust(shield.Center - new Vector2(4, 4), 0, 0, DustID.t_BorealWood);
        }
    }

    public override void ExtraAIShield(Player player, Projectile shield)
    {
        if (shield.owner == Main.myPlayer && shield.ModProjectile is GuardianShieldAnchor anchor)
        {
            shield.frame = DamageState;
        }
    }
    public override void SlamHitFirst(Player player, Projectile shield, NPC npc, bool WeakSlam)
    {
        if (shield.owner == Main.myPlayer && shield.ModProjectile is GuardianShieldAnchor anchor && !WeakSlam)
        {
            OrchidGuardian guardian = Main.LocalPlayer.Guardian();

            if (DamageState < 2)
            {
                DamageState++;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item127, shield.position);
                CombatText.NewText(shield.getRect(), new Color(150, 20, 54), (DamageState));
            }
            else if (DamageState == 2)
            {
                DamageState = 0;
                ShieldBreak(player, shield, 4f);
                ResetState(shield);                
            }
        }
    }
    public override void PaviseModifyHitNPC(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, ref NPC.HitModifiers modifiers, bool firstHit)
    {
        float damageMult = DamageState switch
        {
            0 => 1f,
            1 => 0.6f,
            2 => 0.2f,
            _ => 1f,
        };

        projectile.damage = (int)(Item.damage * damageMult);
    }
}