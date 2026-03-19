using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.Audio;
using Terraria.GameContent;
using ReLogic.Content;
using OrchidMod;
using OrchidMod.Content.Guardian;
using Redemption.BaseExtension;
using Redemption.Buffs.NPCBuffs;
using Redemption.Globals;
using Redemption.Globals.Players;
using Redemption.NPCs.Lab.Janitor;
using GuardiansOfRedemption.General;

namespace GuardiansOfRedemption.Items.Weapons.Quarterstaves;
public class JanitorQuarterstaff : OrchidModGuardianQuarterstaff {

    public Vector2 tip;
    public bool TipTouchingSurface;

    /// <summary>
    /// A timer that increases up to 600 while the tip of the quarterstaff is submerged in water.
    /// </summary>
    public int Sogginess;

    public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(ElementID.WaterS, ElementID.PsychicS);


    public override void SetStaticDefaults() {
    }

    public override void SafeSetDefaults()
    {
        Item.width = 42;
        Item.height = 40;
        Item.value = Item.sellPrice(0, 4);
        Item.rare = ItemRarityID.Pink;
        Item.useTime = 30;
        Item.knockBack = 3f;
        Item.damage = 69;
        Item.shootSpeed = 10f;
        GuardStacks = 1;
        ParryDuration = 100;
        JabStyle = 2;
        JabDamage = 1.0f;

        Sogginess = 0;
        TipTouchingSurface = false;
    }

    public override void ExtraAIQuarterstaff(Player player, OrchidGuardian guardian, Projectile projectile)
    {
        tip = (projectile.ModProjectile as GuardianQuarterstaffAnchor).GetQuarterstaffTip(0.4f);

        if (player.velocity.X > 0) // This part allows moving the flag based on player movement
        {
            projectile.localAI[1] += Math.Abs(projectile.localAI[1] - 1) * 0.1f;
        }
        else if (player.velocity.X < 0)
        {
            projectile.localAI[1] -= Math.Abs(-1 - projectile.localAI[1]) * 0.1f;
        }
        else projectile.localAI[1] *= 0.95f;

        
        if (Collision.WetCollision(tip, 24, 24) && Framing.GetTileSafely(tip).LiquidType == LiquidID.Water) {
            Sogginess++;
            if (Sogginess > 600) Sogginess = 600;
        }
        else if (Framing.GetTileSafely(tip) == null) {
            if (Sogginess > 0 && Main.rand.NextBool(11 - (int)Math.Ceiling(Sogginess / 60f))) 
            {
                Dust.NewDustPerfect(tip + Vector2.UnitY * 6f, Dust.dustWater(), Vector2.UnitY * 4f);
                SoundEngine.PlaySound(SoundID.Drip with {Volume = 0.1f, PitchRange = (-0.8f, -0.2f)}, tip);
            }
            Sogginess--;
            if (Sogginess < 0) Sogginess = 0;
        }

    } 

    public override void ExtraAIQuarterstaffJabbing(Player player, OrchidGuardian guardian, Projectile projectile)
    { 
        foreach (var gore in Main.gore) {
            if (gore.active && !ChildSafety.SafeGore[gore.type] && Collision.CheckAABBvAABBCollision(gore.position, gore.AABBRectangle.Size(), tip, new(12, 12))) {
                gore.active = false;
                for (int i = 0; i < 5; i++) Dust.NewDustDirect(tip, 24, 24, Dust.dustWater(), Main.rand.NextFloat(-8f, 8f), -4f);
                SoundEngine.PlaySound(SoundID.Item21 with {Volume = 0.6f}, gore.position);
            }   
        }
        if (Sogginess > 0 && projectile.ai[0] is > -30f and < -10f && Framing.GetTileSafely(tip) == null && Main.rand.NextBool(11 - (int)Math.Ceiling(Sogginess / 60f))) {
            Dust.NewDustPerfect(tip, Dust.dustWater(), player.Center.DirectionTo(tip) * 6f, Scale: 1.5f);
            SoundEngine.PlaySound(SoundID.Drip with {Volume = 0.1f, PitchRange = (-0.4f, 0.2f)}, tip);
            Sogginess--;
        }
    }

    public override void OnAttack(Player player, OrchidGuardian guardian, Projectile projectile, bool jabAttack, bool counterAttack)
    {
        if (!jabAttack && !counterAttack) {
            Projectile bucketProj = Projectile.NewProjectileDirect(
                projectile.GetSource_FromAI(),
                player.Center,
                Vector2.UnitX.RotatedBy((Main.MouseWorld - tip).ToRotation()) * Item.shootSpeed,
                ModContent.ProjectileType<BucketSplash>(),
                guardian.GetGuardianDamage(Item.damage * 0.5f),
                20f,
                projectile.owner
            );
            bucketProj.hostile = false;
            bucketProj.friendly = true;
            SoundEngine.PlaySound(SoundID.NPCDeath19, tip);
        }
    }

    public override void QuarterstaffModifyHitNPC(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, ref NPC.HitModifiers modifiers, bool jabAttack, bool counterAttack, bool firstHit)
    {
        if (Collision.CheckAABBvAABBCollision(target.Center, target.Hitbox.Size(), tip, new(24, 24))) {
            
            // There's seemingly no way to manually override elemental damage on hit, so I have to bullshit the effects 
            if (NPCLists.Robotic.Contains(target.type) && Main.rand.NextBool(4)) {
                target.AddBuff(ModContent.BuffType<ElectrifiedDebuff>(), 120);
                RedeQuest.SetBonusDiscovered(RedeQuest.Bonuses.Water);
            }
            target.RedemptionGuard().IgnoreArmour = true;
            ElementalNPC elementalNPC = target.GetGlobalNPC<ElementalNPC>();
            float npcMult = elementalNPC.elementDmg[ElementID.Water] * elementalNPC.elementDmg[ElementID.Psychic];

            if (npcMult >= 1.1f)
                CombatText.NewText(target.getRect(), Color.CornflowerBlue, npcMult + "x", true, true);
            else if (npcMult <= 0.9f)
                CombatText.NewText(target.getRect(), Color.IndianRed, npcMult + "x", true, true);

            BuffPlayer buffPlayer = player.RedemptionPlayerBuff();
            float playerMult = (1 + buffPlayer.ElementalDamage[ElementID.Water]) * (1 + buffPlayer.ElementalDamage[ElementID.Psychic]);

            modifiers.NonCritDamage *= npcMult * playerMult;
            for (int i = 0; i < 5; i++) Dust.NewDustPerfect(target.Center, Dust.dustWater(), Main.rand.NextVector2Circular(8, 8));
            SoundEngine.PlaySound(SoundID.NPCDeath19, tip);
            target.AddBuff(BuffID.Wet, 240);
        }
    }

    public override void PostDrawQuarterstaff(SpriteBatch spriteBatch, Projectile projectile, Player player, Color lightColor)
    {
        // Drawing code borrowed from Orchid, GuardianStandardAnchor.cs (credits to Verveine and the Orchid team)
        Texture2D textureAir = ModContent.Request<Texture2D>(QuarterstaffTexture + "_Mop", AssetRequestMode.ImmediateLoad).Value;
        Texture2D textureContact = ModContent.Request<Texture2D>(QuarterstaffTexture + "_MopContact", AssetRequestMode.ImmediateLoad).Value;

        float drawRotation = projectile.ai[1];
        Vector2 posproj = (projectile.ModProjectile as GuardianQuarterstaffAnchor).GetQuarterstaffTip(0.4f);
        if (player.gravDir == -1) {
            drawRotation = MathHelper.PiOver2 - drawRotation;
            posproj.Y = (player.Bottom.Floor() + player.position.Floor()).Y - posproj.Y + (posproj.Y - player.Center.Floor().Y) * 2f;
        }
		
        var effect = projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

        var drawPosition = Vector2.Transform(posproj - Main.screenPosition + Vector2.UnitY * player.gfxOffY, Main.GameViewMatrix.EffectMatrix);

        if (!TipTouchingSurface) {
            float windSpeed = 0.25f * Main.windSpeedCurrent;
            float mopRotation = MathHelper.PiOver4 * projectile.localAI[1] * player.gravDir * 0.5f  + (float)Math.Sin((projectile.ModProjectile as GuardianQuarterstaffAnchor).TimeSpent * (Math.Abs(windSpeed) > 0.05f ? windSpeed : 0.05f)) * 0.1f;
            Vector2 mopOffset = Vector2.UnitX.RotatedBy(drawRotation) * -1.5f * projectile.localAI[1];

            spriteBatch.Draw(textureAir, drawPosition + mopOffset, null, Color.Lerp(Color.White, Color.SlateBlue, Sogginess / 600f), drawRotation + mopRotation * 0.5f, textureAir.Size() * 0.5f, projectile.scale, effect, 0f);
        }
    }
}