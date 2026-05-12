using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GuardiansOfRedemption.General.Global;
using OrchidMod;
using OrchidMod.Content.Guardian;
using ReLogic.Content;
using Redemption.BaseExtension;
using Redemption.Globals;
using Redemption.Items.Armor.HM.Hardlight;
using Redemption.Items.Materials.HM;
using Redemption.NPCs.Lab.Janitor;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;


namespace GuardiansOfRedemption.Items.Guardian.Armor.Hardlight;

[AutoloadEquip(EquipType.Head)]
public class HardlightGreathelm : OrchidModGuardianEquipable
{
	public static LocalizedText SetBonusText { get; private set; }

	public static Texture2D GlowTexture;

	public override void SetStaticDefaults()
	{
		SetBonusText = this.GetLocalization("SetBonus");
	}

	public override void SafeSetDefaults()
	{
		Item.width = 22;
		Item.height = 26;
		Item.value = Item.sellPrice(0, 0, 75);
		Item.rare = ItemRarityID.LightPurple;
		Item.defense = 24;
		GlowTexture ??= ModContent.Request<Texture2D>(Item.ModItem.Texture + "_Head_Glow", AssetRequestMode.ImmediateLoad).Value;
	}

	public override void UpdateEquip(Player player)
	{
	
		player.GetDamage<GuardianDamageClass>() += 0.13f;
		player.GetCritChance<GuardianDamageClass>() += 5;
		
		OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
		modPlayer.GuardianGuardMax++;
		modPlayer.GuardianSlamMax++;
		player.aggro += 100;
	}

	public override bool IsArmorSet(Item head, Item body, Item legs)
	{
		return body.type == ModContent.ItemType<HardlightPlate>() && legs.type == ModContent.ItemType<HardlightBoots>();
	}

	public override void UpdateArmorSet(Player player)
	{
		RedemptionGuardian modPlayer = player.GetModPlayer<RedemptionGuardian>();
		// Code borrowed from Redemption
		player.setBonus = Language.GetTextValue("Mods.Redemption.GenericTooltips.ArmorSetBonus.Hardlight.Keybind");
		if (!Main.dedServ)
		{
			foreach (string assignedKey in Redemption.Redemption.RedeSpecialAbility.GetAssignedKeys())
				player.setBonus = Language.GetTextValue("Mods.Redemption.GenericTooltips.ArmorSetBonus.Hardlight.Press") + assignedKey + Language.GetTextValue("Mods.Redemption.GenericTooltips.ArmorSetBonus.Hardlight.Support") + SetBonusText;
		}
		modPlayer.GuardianHardlight = true;
		player.RedemptionPlayerBuff().MetalSet = true;
	}

	public override void AddRecipes()
	{
		CreateRecipe()
		.AddIngredient<CyberPlating>(8)
		.AddTile(TileID.MythrilAnvil)
		.Register();
	}
}

public class HardlightGreathelmGlowmask : PlayerDrawLayer
{
    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        Player drawPlayer = drawInfo.drawPlayer;
		if (drawInfo.drawPlayer.dead) return;
		
		if (drawPlayer.armor[10].type == ModContent.ItemType<HardlightGreathelm>() || (drawPlayer.armor[10].type == ItemID.None && drawPlayer.armor[0].type == ModContent.ItemType<HardlightGreathelm>()))
		{
			Color color = drawPlayer.GetImmuneAlphaPure(Color.White, drawInfo.shadow);
			Vector2 drawPos = drawInfo.Position - Main.screenPosition + new Vector2((drawPlayer.width - drawPlayer.bodyFrame.Width) * 0.5f, drawPlayer.height - drawPlayer.bodyFrame.Height + 4f) + drawPlayer.headPosition;

			DrawData drawData = new(HardlightGreathelm.GlowTexture, drawPos.Floor() + drawInfo.headVect, drawPlayer.bodyFrame, color, drawPlayer.headRotation, drawInfo.headVect, 1f, drawInfo.playerEffect, 0);

			drawInfo.DrawDataCache.Add(drawData);
		}
    }
}
