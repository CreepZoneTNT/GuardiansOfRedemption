using GuardiansOfRedemption.General.Global;
using Microsoft.Xna.Framework;
using OrchidMod;
using OrchidMod.Content.Guardian;
using Redemption.BaseExtension;
using Redemption.Globals;
using Redemption.Items.Armor.PreHM.CommonGuard;
using Redemption.Items.Materials.PreHM;
using System;
using System.Collections.Generic;
using GuardiansOfRedemption.General;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Content.Guardian.Armor.CommonGuard
{
    [AutoloadEquip(EquipType.Head)]
    public class CommonGuardGreathelm : ModItem
    {
    
        public static LocalizedText SetBonusText { get; private set; }
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Common Guard Bucket Helm");
            // Tooltip.SetDefault("+1 increased melee damage");
            ArmorIDs.Head.Sets.DrawHead[EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head)] = false;

            SetBonusText = this.GetLocalization("SetBonus");
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 26;
            Item.sellPrice(silver: 30);
            Item.rare = ItemRarityID.Green;
            Item.defense = 5;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<CommonGuardPlateMail>() && legs.type == ModContent.ItemType<CommonGuardGreaves>();
        }

        public override void UpdateEquip(Player player)
        {
            OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
            modPlayer.GuardianGuardMax++;
        }

        public override void UpdateArmorSet(Player player)
        {
            RedemptionGuardian modPlayer = player.RedemptionGuardian();

            if (!Main.dedServ)
            {
                string str = $"[{Language.GetTextValue("Mods.Redemption.Keybinds.SpecialAbilityKey.DisplayName")}]";
                foreach (string assignedKey in Redemption.Redemption.RedeSpecialAbility.GetAssignedKeys())
                    str = assignedKey;
                player.setBonus = SetBonusText.Format(str);
            }
            player.statDefense += 4;
            player.GetDamage<GuardianDamageClass>() += 0.05f;
            modPlayer.GuardianCommonGuard = true;
            player.RedemptionPlayerBuff().commonGuardBonus = true;
            player.RedemptionPlayerBuff().MetalSet = true;
            

            if (Main.rand.NextBool(10) && Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y) > 1f && !player.rocketFrame)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(player.position.X - player.velocity.X * 2f, player.position.Y - 2f - player.velocity.Y * 2f), player.width, player.height, DustID.Web);
                dust.noGravity = true;
                dust.velocity -= player.velocity * 0.5f;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<GraveSteelAlloy>(), 10)
                .AddIngredient(ItemID.Silk, 2)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (Main.keyState.PressingShift())
            {   
                TooltipLine line = new(Mod, "Lore", Language.GetTextValue("Mods.GuardiansOfRedemption.SpecialTooltips.CommonGuardGreathelm"))
                {
                    OverrideColor = Color.LightGray
                };
                tooltips.Add(line);
            }
            else
            {
                TooltipLine line = new(Mod, "HoldShift", Language.GetTextValue("Mods.Redemption.SpecialTooltips.Viewer"))
                {
                    OverrideColor = Color.Gray
                };
                tooltips.Add(line);
            }
        }
    }
}