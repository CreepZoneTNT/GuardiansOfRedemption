using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod;
using OrchidMod.Common;
using OrchidMod.Common.UIs;
using OrchidMod.Content.Guardian;
using OrchidMod.Content.Guardian.UI;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace GuardiansOfRedemption.Items.Weapons.Gauntlets;

public class LaboratoryGauntlet : OrchidModGuardianGauntlet
{

    public float BonusCharge;
    public bool SuperCharged;
    public bool UberCharged;
    
    Texture2D textureGauntletSubOn = ModContent.Request<Texture2D>("GuardiansOfRedemption/General/UI/GauntletSubOn", AssetRequestMode.ImmediateLoad).Value;
    Texture2D textureGauntletSubOff = ModContent.Request<Texture2D>("GuardiansOfRedemption/General/UI/GauntletSubOff", AssetRequestMode.ImmediateLoad).Value;
    Texture2D textureGauntletSubReady = ModContent.Request<Texture2D>("GuardiansOfRedemption/General/UI/GauntletSubReady", AssetRequestMode.ImmediateLoad).Value;
    
    public override void SafeSetDefaults()
    {
        Item.width = 40;
        Item.height = 42;
        Item.knockBack = 8f;
        Item.damage = 240;
        Item.value = Item.sellPrice(0, 7, 50);
        Item.rare = ItemRarityID.Purple;
        Item.useTime = 30;
        StrikeVelocity = 20f;
        ParryDuration = 120;
        
    }

    public override Color GetColor(bool offHand)
    {
        return new Color(46, 178, 164);
    }

    public override void ExtraAIGauntlet(Player player, OrchidGuardian guardian, Projectile anchor, bool offHandGauntlet)
    {
        if (anchor.owner == Main.myPlayer && anchor.owner == player.whoAmI)
        {
            GuardianGauntletAnchor gauntlet = anchor.ModProjectile as GuardianGauntletAnchor;
            if (guardian.GuardianItemCharge >= 180f)
            {
                BonusCharge += 30f / Item.useTime * (player.GetTotalAttackSpeed(DamageClass.Melee) * 2f - 1f);
                
                if ((int)BonusCharge % 10 == 0) CombatText.NewText(player.getRect(), Color.White, (int)BonusCharge);
                
                if (BonusCharge > 360f)
                {
                    if (!UberCharged)
                    {
                        if (ModContent.GetInstance<OrchidClientConfig>().GuardianAltChargeSounds) SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot, player.Center);
                        else SoundEngine.PlaySound(SoundID.MaxMana with { Pitch = 0.04f }, player.Center);
                        UberCharged = true;
                        for (int i = 0; i < 5; i++) Dust.NewDustDirect(player.MountedCenter - Vector2.UnitY * player.height * 0.5f, 24, 24, DustID.AncientLight, Main.rand.NextFloat(-1f, 1f), -1f);
                    }
                    BonusCharge = 360f;
                }
                else if (BonusCharge > 180f)
                {
                    if (!SuperCharged)
                    {
                        if (ModContent.GetInstance<OrchidClientConfig>().GuardianAltChargeSounds) SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot, player.Center);
                        else SoundEngine.PlaySound(SoundID.MaxMana with { Pitch = 0.02f }, player.Center);
                        SuperCharged = true;
                    }
                    if (Main.rand.NextBool(3))
                    {
                        Dust dust = Dust.NewDustDirect(anchor.Center, 24, 24, DustID.Electric);
                        dust.noGravity = true;
                    }
                } 
                else
                {
                    if (Main.rand.NextBool(6))
                    {
                        Dust dust = Dust.NewDustDirect(anchor.Center, 24, 24, DustID.Electric);
                        dust.noGravity = true;
                    }
                }
            }
            else BonusCharge = 0;
        }
    }

    public override bool OnPunch(Player player, OrchidGuardian guardian, Projectile projectile, bool offHandGauntlet, bool manuallyFullyCharged, ref bool charged, ref int damage)
    {
        if (manuallyFullyCharged)
        {
            if (UberCharged)
            {
                StrikeVelocity = 45f;
                damage = guardian.GetGuardianDamage(Item.damage * 1.8f);
                CombatText.NewText(player.getRect(), Color.RoyalBlue, "UberCharged!");
            }
            else if (SuperCharged) 
            {
                StrikeVelocity = 32.5f;
                damage = guardian.GetGuardianDamage(Item.damage * 1.4f);
                CombatText.NewText(player.getRect(), Color.DodgerBlue, "SuperCharged!");
            }
            else{
                StrikeVelocity = 20f;
                CombatText.NewText(player.getRect(), Color.SkyBlue, "Normal!");
            }
                
        }
        BonusCharge = 0f;
        SuperCharged = false;
        UberCharged = false;
        return true;
    }

    public override void PostDrawGauntlet(SpriteBatch spriteBatch, Projectile projectile, Player player, bool offHandGauntlet, Color lightColor)
    {
        // Trying to draw the extra indicators next to the charge icon, so I'm borrowing the base draw code from Orchid (is that okay Verveine?)
        
        Texture2D textureGauntletOn = ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/GauntletOn", AssetRequestMode.ImmediateLoad).Value;
        
        OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
        if (!player.dead && modPlayer.GuardianDisplayUI > 0)
        {
        
            bool minHoldTimer = modPlayer.ChargeHoldTimer > ModContent.GetInstance<OrchidClientConfig>().GuardianMinHoldTimer;
            bool maxHoldTimer = modPlayer.ChargeHoldTimer > ModContent.GetInstance<OrchidClientConfig>().GuardianMaxHoldTimer;
            
            bool drawAtCursor = ModContent.GetInstance<OrchidClientConfig>().GuardianChargeCursor;
            Vector2 position = (player.position + new Vector2(player.width * 0.5f, player.height + player.gfxOffY + 12)).Floor() - Main.screenPosition;;
            if (player.gravDir < 0) position.Y -= 81;
            SpriteEffects effect = player.gravDir > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;
            
            Texture2D chargeTextureOn = null;
            Texture2D chargeTextureOff = null;
            Texture2D chargeTextureReady = null;
            
            
            if (player.HeldItem.ModItem is OrchidModGuardianGauntlet gauntlet && (maxHoldTimer || (minHoldTimer && modPlayer.GuardianItemCharge > (70 * player.GetTotalAttackSpeed(DamageClass.Melee) - (player.HeldItem.useTime * gauntlet.ChargeSpeedMultiplier)) / 2.5f)))
            {
                chargeTextureOn = textureGauntletSubOn;
                chargeTextureOff = textureGauntletSubOff;
                chargeTextureReady = textureGauntletSubReady;
            }
            
            if (chargeTextureOn != null)
            {
                
                int offSet = textureGauntletSubOn.Height + 2;
                
                
                if (drawAtCursor)
                {
                    
                    for (int i = 0; i < 2; i++)
                    {
                    
                        int val = chargeTextureOn.Height;
                        if (BonusCharge % 180f < 180f)
                        {
                            float charge = BonusCharge % 180f;
                            while (charge < 180f)
                            {
                                charge += 7.5f;
                                val--;
                            }
                        }

                        Rectangle rectangle = chargeTextureOn.Bounds;
                        rectangle.Height = val;
                        rectangle.Y = chargeTextureOn.Height - val;
                        
                        Vector2 drawpos = new Vector2(Main.MouseScreen.X + 18 + textureGauntletSubOn.Width, Main.MouseScreen.Y + 18 - offSet + (textureGauntletSubOn.Height + 2) * i) + textureGauntletOn.Size() * 0.5f;
                        
                        if ((int)Math.Floor(BonusCharge / 180f) - 1 >= i)
                        {
                            spriteBatch.Draw(chargeTextureReady, drawpos - new Vector2(2, 2), null, Color.White * 0.8f, 0f, Vector2.Zero, 1f, effect, 0f);
                        }
                        spriteBatch.Draw(chargeTextureOff, drawpos, null, Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
                        
                        if ((int)Math.Floor(BonusCharge / 180f) - 1 >= i)
                        {
                            spriteBatch.Draw(chargeTextureOn, drawpos, null, Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
                        }
                        else
                        {
                            drawpos.Y += chargeTextureOn.Height - val;
                            spriteBatch.Draw(chargeTextureOn, drawpos, rectangle, Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 2; i++)
                    {
                        Vector2 drawpos;
                        int val = chargeTextureOn.Height;
                        if (BonusCharge % 180f < 180f)
                        {
                            float charge = BonusCharge % 180f;
                            while (charge < 180f)
                            {
                                charge += 7.5f;
                                val--;
                            }
                        }

                        Rectangle rectangle = chargeTextureOn.Bounds;
                        rectangle.Height = val;
                        rectangle.Y = chargeTextureOn.Height - val;
                        
                        if ((int)Math.Floor(BonusCharge / 180f) - 1 >= i)
                        {
                            drawpos = new Vector2(position.X - 11, position.Y - 96 * player.gravDir + 5f * (player.gravDir - 1) - offSet + (textureGauntletSubOn.Height + 2) * i) + textureGauntletOn.Size() * 0.5f;
                            spriteBatch.Draw(chargeTextureReady, drawpos, null, Color.White * 0.8f, 0f, Vector2.Zero, 1f, effect, 0f);
                        }

                        drawpos = new Vector2(position.X - 9, position.Y - 94 * player.gravDir + 3f * (player.gravDir - 1) - offSet + (textureGauntletSubOn.Height + 2) * i) + textureGauntletOn.Size() * 0.5f;
                        spriteBatch.Draw(chargeTextureOff, drawpos, null, Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
                        drawpos = new Vector2(position.X - 9, position.Y - 94 * player.gravDir + chargeTextureOn.Height - val + 3f * (player.gravDir - 1) - offSet + (textureGauntletSubOn.Height + 2) * i) + textureGauntletOn.Size() * 0.5f;
                        if (player.gravDir < 0) drawpos.Y -= (chargeTextureOn.Height - rectangle.Height);
                        if ((int)Math.Floor(BonusCharge / 180f) - 1 >= i)
                        {
                            spriteBatch.Draw(chargeTextureOn, drawpos, null, Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
                        }
                        else
                        {
                            drawpos.Y += chargeTextureOn.Height - val;
                            spriteBatch.Draw(chargeTextureOn, drawpos, rectangle, Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
                        }
                    }
                }
            }
        }
    }
}