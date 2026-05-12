using GuardiansOfRedemption.Items.Other.Materials;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod;
using OrchidMod.Content.Guardian;
using OrchidMod.Content.Shapeshifter;
using OrchidMod.Content.Shapeshifter.Misc;
using Redemption;
using Redemption.Globals;
using Redemption.Items.Materials.HM;
using Redemption.Items.Materials.PostML;
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

        public bool LeftClickAttacking = false;
        public bool RightClickAttacking = false;

        public int Jumps = 0;

        public int FlameCharge = 0;
        public bool FlameCue = true;

        public override void SafeSetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = CustomSounds.ChickenCluck;
            Item.useTime = 40;
            Item.shootSpeed = 8f;
            Item.knockBack = 5f;
            Item.damage = 60;
            ShapeshiftWidth = 20;
            ShapeshiftHeight = 40;
            ShapeshiftType = ShapeshifterShapeshiftType.Sage;
            ShapeshiftTypeUI = ShapeshifterShapeshiftTypeUI.List;
            MeleeSpeedRight = true;
            GroundedWildshape = true;
        }
        public override void ShapeshiftGetUIInfo(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter, ref int uiCount, ref int uiCountMax)
        {
            uiCount = 0;
            uiCountMax = 3;
            if (anchor.ai[2] > 0)
            { // Flame on
                uiCount = (int)anchor.ai[2];
            }
        }
        public override void ShapeshiftAnchorOnShapeshift(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
        {

            anchor.Frame = 1;
            anchor.Timespent = 0;
            if (FlameCue)
                anchor.ai[2] = 3;

            projectile.direction = player.direction;
            projectile.spriteDirection = player.direction;

            LateralMovement = false;
            Jumps = 0;
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
            anchor.LeftCLickCooldown = Item.useTime;
            LeftClickAttacking = true;
            projectile.ai[0] = 30;

            projectile.ai[1] = (Main.MouseWorld.X < projectile.Center.X ? -1f : 1f);

            anchor.NeedNetUpdate = true;
        }
       
        public override void ShapeshiftOnRightClick(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
        {
            if(FlameCue && IsGrounded(projectile, player, 8))
                SoundEngine.PlaySound(SoundID.DD2_BetsyFlameBreath, projectile.Center);
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
            if (RightClickAttacking)
                player.noKnockback = true;
        }

        public override void ShapeshiftAnchorAI(Projectile projectile, ShapeshifterShapeshiftAnchor anchor, Player player, OrchidShapeshifter shapeshifter)
        {
            // ai[0] is used as a timer for attack animations
            // ai[1] is used to flip the sprite in the correct direction while attacking
            // ai[2] is used as a cooldown for the dash (jump)

            // anchor.ai[0] is used for animation for left click
            // anchor.ai[1]
            // anchor.ai[2] is used for the UI
            // anchor.ai[3] is used for time inbetween attacks of right click.
            // anchor.ai[4]

            // checking for if attacking

            bool grounded = IsGrounded(projectile, player, 8f);
            float speedMult = GetSpeedMult(player, shapeshifter, anchor, grounded);
            GravityMult = 0.7f;
            if (anchor.IsInputDown) GravityMult += 0.3f;
            if (anchor.IsInputUp) GravityMult -= 0.3f;

            projectile.ai[0]--;

            if (anchor.RightCLickCooldown > 0)
            {
                anchor.RightCLickCooldown--;
            }

            // ANIMATION

            if (!grounded && projectile.velocity.Y < 0 && !LeftClickAttacking)
            { // moving up frame
                anchor.Timespent = 0;
                anchor.Frame = 10;
            }
            else if (!grounded && projectile.velocity.Y > 0 && !LeftClickAttacking)
            {
                anchor.Timespent = 0;
                anchor.Frame = 11;
            }
            else if (LateralMovement && grounded)
            {
                if (anchor.Timespent % 8 == 0 && anchor.Timespent > 0)
                {
                    if (anchor.Frame > 10)
                    {
                        anchor.Frame = 0;
                    }

                    anchor.Frame++;

                    if (anchor.Frame == 10)
                    {
                        anchor.Frame = 1;
                    }
                }
            }
            else if (!LateralMovement && grounded)
            { // idle frame
                anchor.Timespent = 0;
                anchor.Frame = 0;
            }

            // Attacks
            if (anchor.Projectile.ai[0] >= 0f)
            {
                if (anchor.Projectile.ai[0] == 0)
                {
                    anchor.Frame = 1;
                }
                else if (anchor.Projectile.ai[0] >= 17)
                {
                    anchor.Frame = 12;
                }
                else if (anchor.Projectile.ai[0] < 17 && anchor.Projectile.ai[0] > 0)
                {
                    anchor.Frame = 13;
                }
            }

            if (LeftClickAttacking)
            {
                if (anchor.Projectile.ai[0] == 17)
                {
                    int projectileType = ModContent.ProjectileType<SageBasan_Proj>();
                    Vector2 velocity = Vector2.Normalize(Main.MouseWorld - projectile.Center) * Item.shootSpeed;
                    ShapeshifterNewProjectile(shapeshifter, projectile.Center, velocity, projectileType, Item.damage / 2, Item.crit, Item.knockBack, player.whoAmI);
                    SoundEngine.PlaySound(SoundID.DD2_FlameburstTowerShot, projectile.Center);
                }

                if (anchor.Projectile.ai[0] == 17)
                    LeftClickAttacking = false;
            }
            if (anchor.IsRightClick && grounded && FlameCharge <= 180 && FlameCue)
            {
                RightClickAttacking = true;
                FlameCharge++;
                anchor.ai[3]++;

                anchor.Frame = 15;
                int projectileType = ModContent.ProjectileType<SageBasan_ProjAlt>();
                Vector2 velocity = Vector2.Normalize(Main.MouseWorld - projectile.Center) * (Item.shootSpeed / 3);
                if (FlameCharge % 6 == 0)
                {
                    //CombatText.NewText(player.getRect(), Color.Black, (int)(anchor.ai[3]));
                    ShapeshifterNewProjectile(shapeshifter, projectile.Center, velocity, projectileType, (Item.damage + (FlameCharge / 9)) / 3, Item.crit, Item.knockBack, player.whoAmI);
                    if (FlameCharge % 60 == 0 && FlameCharge != 0)
                    {
                        anchor.ai[2] -= 1;
                        SoundEngine.PlaySound(SoundID.LiquidsWaterLava, projectile.Center);
                    }
                }

            }
            else if (FlameCharge >= 180 && FlameCue)
            {
                anchor.ai[2] = 0;
                SoundEngine.PlaySound(SoundID.AbigailAttack, projectile.Center);
                FlameCue = false;
            }

            else if (!anchor.IsRightClick && grounded && FlameCharge >= 0)
            {
                //Recharging
                RightClickAttacking = false;
                anchor.ai[3]--;
                if (anchor.ai[3] <= 0)
                {
                    anchor.ai[3] = 0;
                    FlameCharge--;

                    if(FlameCharge % 60 == 0 && FlameCharge != 180 && FlameCharge != 0)
                    {
                        anchor.ai[2]++;
                        SoundEngine.PlaySound(SoundID.LiquidsWaterLava, projectile.Center);   
                    }
                    // Finished Recharging
                    if (FlameCharge == 0 && !FlameCue)
                    {
                        anchor.ai[2] = 3;
                        FlameCharge = 0;
                        FlameCue = true;
                        anchor.Blink(true);
                        SoundEngine.PlaySound(SoundID.Item20, projectile.Center);
                    }
                }
            }

            // MOVEMENT

            if (anchor.Projectile.ai[2] == 0)
            { // Normal movement, not dashing or hooked
                Vector2 intendedVelocity = projectile.velocity;
                GravityCalculations(ref intendedVelocity, player, shapeshifter);

                if (anchor.JumpWithControlRelease(player) && Jumps > 0 && !RightClickAttacking)
                { // Jump
                    Jumps--;
                    TryJump(ref intendedVelocity, 6.5f, player, shapeshifter, anchor, false);
                    JumpAttack(projectile, anchor, player, shapeshifter);
                    SoundEngine.PlaySound(SoundID.Item32, projectile.Center);
                }

                // Normal movement
                if ((anchor.IsInputLeft || anchor.IsInputRight) && !RightClickAttacking)
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
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<ShapeshifterBlankEffigy>())
                .AddIngredient(ModContent.ItemType<BasanMaterial>(), 10)
                .AddTile(TileID.Hellforge)
                .Register();
        }
    }
}