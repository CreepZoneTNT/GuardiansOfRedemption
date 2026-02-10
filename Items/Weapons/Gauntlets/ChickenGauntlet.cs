using Microsoft.Xna.Framework;
using OrchidMod.Content.Guardian;
using Terraria;
using Terraria.ID;

namespace GuardiansOfRedemption.Items.Weapons.Gauntlets;

public class ChickenGauntlet : OrchidModGuardianGauntlet
{
    public override void SafeSetDefaults()
    {
        Item.width = 26;
        Item.height = 28;
        Item.knockBack = 5f;
        Item.damage = 100;
        Item.value = Item.sellPrice(0, 0, 20);
        Item.rare = ItemRarityID.Blue;
        Item.useTime = 20;
        StrikeVelocity = 16f;
        ParryDuration = 90;
    }

    public override Color GetColor(bool offHand)
    {
        return new Color(202, 163, 95);
    }
}