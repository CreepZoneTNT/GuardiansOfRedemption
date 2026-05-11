using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using OrchidMod;
using OrchidMod.Content.Shapeshifter;
using Redemption;
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
    internal class SageBasan : OrchidModShapeshifterShapeshift
    {
        public bool LateralMovement = false;
        public bool Attacking = false;
        public int Jumps = 0;

        public override void SafeSetDefaults()
        {
            Item.width = 36;
            Item.height = 38;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = CustomSounds.ChickenCluck;
            Item.useTime = 40;
            Item.shootSpeed = 16f;
            Item.knockBack = 5f;
            Item.damage = 60;
            ShapeshiftWidth = 40;
            ShapeshiftHeight = 40;
            ShapeshiftType = ShapeshifterShapeshiftType.Sage;
            GroundedWildshape = true;
        }

        public override void ShapeshiftAnchorOnShapeshift(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
        {
            anchor.Frame = 1;
            anchor.Timespent = 0;
            projectile.direction = player.direction;
            projectile.spriteDirection = player.direction;

            LateralMovement = false;
            Jumps = 0;

            for (int i = 0; i < 5; i++)
            {
                Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.5f;
            }

            for (int i = 0; i < 10; i++)
            {
                Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Torch)].velocity *= 0.75f;
            }
        }

        public override void OnKillAnchor(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
        {
            for (int i = 0; i < 5; i++)
            {
                Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.5f;
            }

            for (int i = 0; i < 8; i++)
            {
                Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Torch)].velocity *= 0.75f;
            }
        }

        public override void ShapeshiftOnLeftClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
        {
            int projectileType = ModContent.ProjectileType<SageBasan_Proj>();
            Vector2 velocity = Vector2.Normalize(Main.MouseWorld - projectile.Center) * Item.shootSpeed;
            ShapeshifterNewProjectile(shapeshifter, projectile.Center, velocity, projectileType, Item.damage / 2, Item.crit, Item.knockBack, player.whoAmI);
            
            SoundEngine.PlaySound(CustomSounds.ChickenCluck, projectile.Center);
            anchor.LeftCLickCooldown = Item.useTime;
            projectile.ai[0] = 20;

            projectile.ai[1] = (Main.MouseWorld.X < projectile.Center.X ? -1f : 1f);

            Vector2 intendedVelocity = projectile.velocity;
            TrySlowDown(ref intendedVelocity, 0.7f, player, shapeshifter, projectile);

            anchor.NeedNetUpdate = true;
        }

        public override void ShapeshiftOnRightClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
        {
            int projectileType = ModContent.ProjectileType<SageBasan_ProjAlt>();
            Vector2 velocity = Vector2.Normalize(Main.MouseWorld - projectile.Center) * Item.shootSpeed / 8;
            ShapeshifterNewProjectile(shapeshifter, projectile.Center, velocity, projectileType, Item.damage, Item.crit, Item.knockBack, player.whoAmI);

            anchor.RightCLickCooldown = Item.useTime / 4;
            projectile.ai[0] = 5;

            anchor.NeedNetUpdate = true;

            /*Vector2 position = projectile.Center;
            Vector2 offSet = Main.MouseWorld - projectile.Center;

            // Spawn the wall at the correct position, on the ground below the player cursor, up to 10 tiles away
            if (offSet.Length() > 160f)
            {
                offSet = Vector2.Normalize(offSet) * 160f;
            }

            for (int i = 0; i < 10; i++)
            {
                position += TileCollideShapeshifter(position, offSet * 0.1f, 2, 2, true, true, (int)player.gravDir);
            }

            for (int i = 0; i < 75; i++)
            {
                position += TileCollideShapeshifter(position, Vector2.UnitY * 15f, 18, 2, false, false, (int)player.gravDir);
            }

            position.Y -= 78; // half the wall height

            // Delete existing walls
            int projectileType = ModContent.ProjectileType<SageImpProjAlt>();
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.type == projectileType && proj.owner == player.whoAmI)
                {
                    proj.Kill();
                }
            }

            ShapeshifterNewProjectile(shapeshifter, position, Vector2.Zero, projectileType, Item.damage * 2f, Item.crit, 0f, player.whoAmI);

            SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, projectile.Center);

            anchor.RightCLickCooldown = 60;
            projectile.ai[0] = 15;
            projectile.ai[1] = (Main.MouseWorld.X < projectile.Center.X ? -1f : 1f);
            anchor.NeedNetUpdate = true;*/
        }
        public override void ShapeshiftOnJump(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
         {
            /*    float rotation = MathHelper.Pi * (1f + projectile.direction * 0.5f);

                // 8 dir input
                if (anchor.IsInputLeft && !anchor.IsInputRight)
                {
                    rotation = MathHelper.Pi * 1.5f; // Left
                    if (anchor.IsInputUp && !anchor.IsInputDown)
                    {
                        rotation += MathHelper.Pi * 0.25f; // Top Left
                    }
                    else if (!anchor.IsInputUp && anchor.IsInputDown)
                    {
                        rotation -= MathHelper.Pi * 0.25f; // Bottom Left
                    }
                }
                else if (!anchor.IsInputLeft && anchor.IsInputRight)
                {
                    rotation = MathHelper.Pi * 0.5f; // Right
                    if (anchor.IsInputUp && !anchor.IsInputDown)
                    {
                        rotation -= MathHelper.Pi * 0.25f; // Top Right
                    }
                    else if (!anchor.IsInputUp && anchor.IsInputDown)
                    {
                        rotation += MathHelper.Pi * 0.25f; // Bottom Right
                    }
                }
                else if (anchor.IsInputUp && !anchor.IsInputDown)
                {
                    rotation = 0f; // Up
                }
                else if (!anchor.IsInputUp && anchor.IsInputDown)
                {
                    rotation = MathHelper.Pi; // Down
                }

                anchor.LeftCLickCooldown = Item.useTime * 4f;
                anchor.NeedNetUpdate = true;
                CanDash = false;

                Vector2 position = projectile.position;
                Vector2 offSet = Vector2.UnitY.RotatedBy(rotation) * -6f * GetSpeedMult(player, shapeshifter, anchor);

                // helps with dush spawn sync in mp
                ShapeshifterNewProjectile(shapeshifter, projectile.Center, offSet, ModContent.ProjectileType<SageImpDash>(), 0, 0, 0, player.whoAmI);

                for (int i = 0; i < 32; i++)
                {
                    position += TileCollideShapeshifter(position, offSet, projectile.width, projectile.height, true, true, (int)player.gravDir);
                }

                anchor.Teleport(position + new Vector2(projectile.width, projectile.height) * 0.5f);
                projectile.position = position;
                projectile.velocity = offSet;
                projectile.velocity *= 0.75f;
                anchor.NeedNetUpdate = true;
                anchor.LeftCLickCooldown = Item.useTime;
                projectile.ai[2] = 30;
                SetCameraLerp(player, 0.1f, 5);*/
        }

        public override void ShapeshiftOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
        {
            base.ShapeshiftOnHitNPC(target, hit, damageDone, projectile, anchor, player, shapeshifter);
        }

        public void JumpAttack(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
        {
            // Checks if has flapped attack so it doesn't spam a bunch of projectiles

            if (!IsGrounded(projectile, player, 8f))
            {
                int projectileType = ModContent.ProjectileType<SageBasan_Proj>();
                Vector2 velocity = new Vector2(0, 8);
                ShapeshifterNewProjectile(shapeshifter, new Vector2(projectile.Center.X, projectile.Center.Y - 6), velocity, projectileType, Item.damage / 3, Item.crit, Item.knockBack, player.whoAmI);
            }
        }

        public override void ShapeshiftBuffs(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
        {
            player.fireWalk = true;
            player.noFallDmg = true;
        }

        public override void ShapeshiftAnchorAI(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
        {
            // ai[0] is used as a timer for attack animations
            // ai[1] is used to flip the sprite in the correct direction while attacking
            // ai[2] is used as a cooldown for the dash (jump)

            // checking for if attacking

            bool grounded = IsGrounded(projectile, player, 8f);
            float speedMult = GetSpeedMult(player, shapeshifter, anchor, grounded);

            GravityMult = 0.7f;
            if (anchor.IsInputDown) GravityMult += 0.3f;
            if (anchor.IsInputUp) GravityMult -= 0.3f;

            // ANIMATION

            if (anchor.Projectile.ai[0] < 0)
            { // dashing
                anchor.Timespent = 0;
                anchor.Frame = 11;

                projectile.direction = (int)anchor.Projectile.ai[1];
                projectile.spriteDirection = projectile.direction;
            }
            else if (anchor.Projectile.ai[0] > 0)
            { // Override animation during left click attack
                anchor.Projectile.ai[0]--;

                if (anchor.Projectile.ai[2] != 0)
                { // Is hooked to a target
                    projectile.direction = (Main.npc[(int)projectile.ai[1]].Center.X - projectile.Center.X) > 0 ? 1 : -1;
                    anchor.Frame = (anchor.Projectile.ai[0] > 5 ? 10 : 11);
                }
                else
                { // Is not hooked
                    projectile.direction = (int)anchor.Projectile.ai[1];
                    anchor.Frame = (anchor.Projectile.ai[0] > 5 ? 8 : 9);
                }

                if (anchor.Projectile.ai[0] < 0)
                {
                    anchor.Projectile.ai[0] = 0;
                }

                if (anchor.Projectile.ai[0] == 0)
                { // Puts the animation back on track
                    anchor.Frame = 0;
                }

                projectile.spriteDirection = projectile.direction;
            }
            else if (anchor.Projectile.ai[2] != 0)
            { // Is hooked to a target & not attacking
                anchor.Frame = 10;
                anchor.Timespent = 0;
                projectile.direction = (Main.npc[(int)projectile.ai[1]].Center.X - projectile.Center.X) > 0 ? 1 : -1;
                projectile.spriteDirection = projectile.direction;
            }
            else if (!grounded && projectile.velocity.Y < 0)
            { // moving up frame
                anchor.Timespent = 0;
                anchor.Frame = 10;
            }
            else if (!grounded && projectile.velocity.Y > 0)
            { // falling frame
                anchor.Timespent = 0;
                anchor.Frame = 11;
            }
            else if (LateralMovement)
            { // Player is moving left or right, cycle through frames
                if (anchor.Timespent % 6 == 0 && anchor.Timespent > 0)
                {
                    if (anchor.Frame > 10)
                    {
                        anchor.Frame = 1;
                    }

                    anchor.Frame++;

                    if (anchor.Frame == 10)
                    {
                        anchor.Frame = 1;
                    }
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

                if (anchor.JumpWithControlRelease(player) && Jumps > 0)
                { // Jump
                    Jumps--;
                    TryJump(ref intendedVelocity, 6.5f, player, shapeshifter, anchor, false);
                    JumpAttack(projectile, anchor, player, shapeshifter);
                    SoundEngine.PlaySound(SoundID.Item32, projectile.Center);
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

                if (IsGrounded(projectile, player, 8f, anchor.IsInputDown, anchor.IsInputDown))
                {
                    Jumps = 6;
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