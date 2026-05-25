using GuardiansOfRedemption.Buffs;
using GuardiansOfRedemption.General.Global;
using GuardiansOfRedemption.Items.Other.Materials;
using OrchidMod;
using OrchidMod.Content.Guardian;
using Redemption;
using Redemption.BaseExtension;
using Redemption.Globals;
using Redemption.Items.Materials.PreHM;
using Redemption.Rarities;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;


namespace GuardiansOfRedemption.Items.Guardian.Armor.BasanArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class BasanHelm : OrchidModGuardianEquipable
    {
        //public static LocalizedText SetBonusText { get; private set; }

        public override void SetStaticDefaults()
        {
            ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
        }  

        public override void SafeSetDefaults()
        {
            Item.width = 22;
            Item.height = 24;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 10;
        }

        public override void UpdateEquip(Player player)
        {
            OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
            player.GetDamage<GuardianDamageClass>() += 0.08f;
            modPlayer.GuardianGuardMax++;
            player.aggro += 300;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<BasanChest>() && legs.type == ModContent.ItemType<BasanLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            RedemptionGuardian modPlayer = player.GetModPlayer<RedemptionGuardian>();
            player.GetJumpState<BasanExtraJump>().Enable();
            modPlayer.GuardianBasan = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.AshWood, 25)
                .AddIngredient(ItemID.Bone, 25)
                .AddIngredient(ModContent.ItemType<BasanMaterial>(), 6)
                .AddTile(TileID.Hellforge)
                .Register();
        }
        public class BasanExtraJump : ExtraJump
        {
            public override Position GetDefaultPosition() => new After(BlizzardInABottle);

            public override float GetDurationMultiplier(Player player)
            {
                // Use this hook to set the duration of the extra jump
                // The XML summary for this hook mentions the values used by the vanilla extra jumps
                return 1f;
            }

            public override void UpdateHorizontalSpeeds(Player player)
            {
                // Use this hook to modify "player.runAcceleration" and "player.maxRunSpeed"
                // The XML summary for this hook mentions the values used by the vanilla extra jumps
                player.runAcceleration *= 1.75f;
                player.maxRunSpeed *= 2f;
            }

            public override void OnStarted(Player player, ref bool playSound)
            {
                // Use this hook to trigger effects that should appear at the start of the extra jump
                // This example mimics the logic for spawning the puff of smoke from the Cloud in a Bottle
                int offsetY = player.height;
                if (player.gravDir == -1f)
                    offsetY = 0;

                offsetY -= 16;

                for (int i = 0; i < 10; i++)
                {
                    Dust dust = Dust.NewDustDirect(player.position + new Vector2(-34f, offsetY), 102, 32, DustID.Smoke, -player.velocity.X * 0.5f, player.velocity.Y * 0.5f, 100, Color.Orange, 1.5f);
                    dust.velocity = dust.velocity * 0.5f - player.velocity * new Vector2(0.1f, 0.3f);
                }
            }
        }            
    }
}
