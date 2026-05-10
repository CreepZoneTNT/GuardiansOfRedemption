using Microsoft.Xna.Framework;
using OrchidMod;
using OrchidMod.Content.Shapeshifter;
using OrchidMod.Content.Shapeshifter.Projectiles.Sage;
using Redemption;
using Redemption.Globals;
using Redemption.NPCs.FowlMorning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Items.Shapeshifter.Weapons.Sage
{
    public class SageBasan : OrchidModShapeshifterShapeshift
    {
        public float WalkFrameCounter;
        public bool LateralMovement = false;
        public int JumpCount = 0;

        public override void SafeSetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = CustomSounds.ChickenCluck;
            Item.useTime = 40;
            Item.shootSpeed = 2f;
            Item.knockBack = 5f;
            Item.damage = 90;
            ShapeshiftWidth = 20;
            ShapeshiftHeight = 40;
            ShapeshiftType = ShapeshifterShapeshiftType.Sage;
            ShapeshiftTypeUI = ShapeshifterShapeshiftTypeUI.List;
            MeleeSpeedRight = true;
            GroundedWildshape = true;
        }

        public override void ShapeshiftAnchorOnShapeshift(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
        {
            anchor.Frame = 1;
            anchor.Timespent = 0;
            projectile.direction = player.direction;
            projectile.spriteDirection = player.direction;
            LateralMovement = false;

            player.position.Y -= 12;

            for (int i = 0; i < 5; i++)
            {
                Dust.NewDustDirect(projectile.Center, 0, 0, DustID.Smoke).velocity *= 0.5f;
            }

            for (int i = 0; i < 10; i++)
            {
                Dust.NewDustDirect(projectile.Center, 0, 0, DustID.Torch).velocity *= 0.75f;
            }
        }

        public override void OnKillAnchor(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDustDirect(projectile.Center, 0, 0, DustID.Smoke).velocity *= 0.5f;
            }

            for (int i = 0; i < 8; i++)
            {
                Dust.NewDustDirect(projectile.Center, 0, 0, DustID.Torch).velocity *= 0.75f;
            }
        }

        public override void ShapeshiftOnLeftClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
        {

            SoundEngine.PlaySound(CustomSounds.ChickenCluck, projectile.Center);
        }

        public override bool ShapeshiftCanRightClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) => IsGrounded(projectile, player, 8f) && anchor.CanRightClick;

        public override void ShapeshiftOnRightClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
        {
            SoundEngine.PlaySound(CustomSounds.ChickenCluck, projectile.Center);
            // if (anchor.CanRightClick)
            // {

                anchor.ai[0] += 40f;
                anchor.RightCLickCooldown = 30;
                anchor.NeedNetUpdate = true;
            // }
            
        }

        public override bool ShapeshiftCanJump(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter) => projectile.ai[2] != 0 && player.controlJump;

        public override void ShapeshiftBuffs(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
        {
            player.fireWalk = true;
        }

        public override void ShapeshiftAnchorAI(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
        {
            // ai[0] is used as a timer for attack animations
            // ai[1] is used to flip the sprite in the correct direction while attacking
            // ai[2] is used as a cooldown for the dash (jump)
            // anchor.ai[0] is used as an exhaustion for right click attack
            // anchor.ai[1]
            // anchor.ai[2]
            // anchor.ai[3]
            // anchor.ai[4]

            int jumps = 0;
            bool grounded = IsGrounded(projectile, player, 8f);
            float speedMult = GetSpeedMult(player, shapeshifter, anchor, grounded);

            if (anchor.RightCLickCooldown > 0)
            { // Right click cd is set to 10 seconds when used, this makes it to touching the ground "resets" it
                speedMult *= 0.5f;

                switch (anchor.RightCLickCooldown)
                {
                    case > 20:
                        anchor.Frame = 12;
                        break;
                    case 20:
                        anchor.Frame = 13;
                        Vector2 dir = Vector2.UnitX * projectile.direction;
                        Projectile wave = Projectile.NewProjectileDirect(projectile.GetSource_FromAI(), projectile.Center + dir * 24f, dir * Item.shootSpeed, ModContent.ProjectileType<Basan_HeatWave>(), Item.damage, 4f);
                        wave.friendly = true;
                        break;

                }
            }

            // ANIMATION

            if (!grounded)
            { // moving up frame
                anchor.Timespent = 0;
                if (projectile.velocity.Y < 0)
                    anchor.Frame = 10;
                else if (projectile.velocity.Y > 0)
                    anchor.Frame = 11;
            }
            else if (LateralMovement)
            { // Player is moving left or right, cycle through frames
                if (anchor.Frame < 1)
                    anchor.Frame = 1;

                WalkFrameCounter += projectile.velocity.X * 0.75f;
                if (WalkFrameCounter is >= 5 or <= -5)
                {
                    WalkFrameCounter = 0;
                    anchor.Frame ++;
                    if (anchor.Frame > 9)
                        anchor.Frame = 1;
                }
            }
            else
            { // idle frame
                anchor.Timespent = 0;
                anchor.Frame = 0;
            }

            // MOVEMENT

            if (anchor.Projectile.ai[2] == 0)
            { // Normal movement, not dashing or hooked
                Vector2 intendedVelocity = projectile.velocity;
                GravityCalculations(ref intendedVelocity, player, shapeshifter);

                if (anchor.IsInputJump && ++jumps < 3)
                { // Jump
                    TryJump(ref intendedVelocity, 9f, player, shapeshifter, anchor, true);
                }

                // Normal movement
                if (anchor.IsInputLeft || anchor.IsInputRight)
                { // Player is inputting a movement key
                    float acceleration = speedMult;
                    if (!grounded) acceleration *= 0.5f;

                    if (anchor.IsInputLeft && !anchor.IsInputRight)
                    { // Left movement
                        TryAccelerate(ref intendedVelocity, shapeshifter, -3f, speedMult, 0.3f, acceleration);
                        projectile.direction = -1;
                        projectile.spriteDirection = -1;
                        LateralMovement = true;
                    }
                    else if (anchor.IsInputRight && !anchor.IsInputLeft)
                    { // Right movement
                        TryAccelerate(ref intendedVelocity, shapeshifter, 3f, speedMult, 0.3f, acceleration);
                        projectile.direction = 1;
                        projectile.spriteDirection = 1;
                        LateralMovement = true;
                    }
                    else
                    { // Both keys pressed = no movement
                        LateralMovement = false;
                        TrySlowDown(ref intendedVelocity, 0.7f, player, shapeshifter, projectile);
                    }
                }
                else
                { // no movement input
                    LateralMovement = false;
                    TrySlowDown(ref intendedVelocity, 0.7f, player, shapeshifter, projectile);
                }

                FinalVelocityCalculations(ref intendedVelocity, projectile, player, true);
            }

            // POSITION AND ROTATION VISUALS

            anchor.OldPosition.Add(projectile.Center);
            anchor.OldRotation.Add(projectile.rotation);
            anchor.OldFrame.Add(anchor.Frame);

            for (int i = 0; i < 2; i++)
            {
                if (anchor.OldPosition.Count > (projectile.ai[0] < 0 ? 6 : 4))
                {
                    anchor.OldPosition.RemoveAt(0);
                    anchor.OldRotation.RemoveAt(0);
                    anchor.OldFrame.RemoveAt(0);
                }
            }
        }
    }
}