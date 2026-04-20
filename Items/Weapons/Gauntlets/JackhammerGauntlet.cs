using GuardiansOfRedemption.General;
using GuardiansOfRedemption.Projectiles.Gauntlets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod;
using OrchidMod.Common;
using OrchidMod.Content.Guardian;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Items.Weapons.Gauntlets;

public class JackhammerGauntlet : OrchidModGuardianGauntlet
{
    public override string GauntletBackTexture => "Redemption/Empty";

    public override void SafeSetDefaults()
    {
        Item.width = 48;
        Item.height = 48;
        Item.knockBack = 8f;
        Item.damage = 300;
        Item.value = Item.sellPrice(0, 8);
        Item.rare = ItemRarityID.Yellow;
        Item.useTime = 40;
        StrikeVelocity = 20f;
        ParryDuration = 15;
        hasArm = true;
        hasShoulder = true;
        hasBackGauntlet = true;
    }

    public override Color GetColor(bool offHand)
    {
        return new Color(255, 32, 32);
    }

    // public override bool CanUseItem(Player player)
    // {
    //     int[] anchors = GetAnchors(player);
    //     if (anchors != null)
    //     {
    //         if (Main.projectile[anchors[1]].ModProjectile is GuardianGauntletAnchor anchor) return (!anchor.Blocking && !anchor.Slamming);
    //     }
    //     return false;
    // }

    public override void ExtraAIGauntlet(Player player, OrchidGuardian guardian, Projectile anchor, bool offHandGauntlet)
    {
        player.SetCompositeArmBack(false, Player.CompositeArmStretchAmount.None, 0);
    }

    public override bool OnPunch(Player player, OrchidGuardian guardian, Projectile projectile, bool offHandGauntlet, bool manuallyFullyCharged, ref bool charged, ref int damage)
    {
        for (int i = 0; i < 3; i++) Dust.NewDustDirect(projectile.Center, projectile.width, projectile.height, DustID.Hay);
        return true;
    }

    // public override bool PreDrawGauntlet(SpriteBatch spriteBatch, Projectile projectile, Player player, bool offHandGauntlet, ref Color lightColor)
    // {
    //     if (offHandGauntlet) lightColor *= 0;
    //     return true;
    // }

    public override void PostDrawGauntlet(SpriteBatch spriteBatch, Projectile projectile, Player player, bool offHandGauntlet, Color lightColor)
    {
        if (!offHandGauntlet && projectile.ModProjectile is GuardianGauntletAnchor anchor)
        {
            Texture2D plungerTexture = ModContent.Request<Texture2D>(GauntletTexture + "_Plunger", AssetRequestMode.ImmediateLoad).Value;
            Texture2D outerTexture = ModContent.Request<Texture2D>(GauntletTexture + "Outer", AssetRequestMode.ImmediateLoad).Value;
        
            var effect = SpriteEffects.None;
            if (player.direction != 1)
            {
                if (player.velocity.X != 0 && !anchor.Blocking || (player.GetModPlayer<OrchidGuardian>().GuardianItemCharge > 0 && projectile.ai[2] != 0) || anchor.Slamming) effect = SpriteEffects.FlipVertically;
                else effect = SpriteEffects.FlipHorizontally;
            }

            Vector2 posproj = projectile.Center;
            if (player.gravDir == -1)
            {
                posproj.Y = (player.Bottom + player.position).Y - posproj.Y + (posproj.Y - player.Center.Y) * 2f;
                if (effect == SpriteEffects.FlipVertically)
                {
                    effect = SpriteEffects.None;
                }
                else if (effect == SpriteEffects.FlipHorizontally)
                {
                    effect = SpriteEffects.None;
                }
                else if (effect == SpriteEffects.None)
                {
                    effect = SpriteEffects.FlipVertically;
                }
            }

            var drawPosition = Vector2.Transform(posproj - Main.screenPosition + Vector2.UnitY * player.gfxOffY, Main.GameViewMatrix.EffectMatrix);
        
            OrchidGuardian guardian = player.Guardian();
            spriteBatch.Draw(plungerTexture, posproj - Main.screenPosition + Vector2.UnitY * player.gfxOffY - Vector2.UnitY.RotatedBy(projectile.rotation - MathHelper.PiOver4) * float.Lerp(0, 12f, guardian.GuardianItemCharge / 180f), null, lightColor, projectile.rotation, plungerTexture.Size() * 0.5f, projectile.scale, effect, 0);
            spriteBatch.Draw(outerTexture, drawPosition, null, lightColor, projectile.rotation, outerTexture.Size() * 0.5f, projectile.scale, effect, 0);

        }
        
    }
}