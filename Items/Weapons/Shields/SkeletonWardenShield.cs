using OrchidMod.Content.Guardian;
using Terraria;
using Terraria.ID;

namespace GuardiansOfRedemption.Items.Weapons.Shields;

public class SkeletonWardenShield : OrchidModGuardianShield
{
    public override void SafeSetDefaults()
    {
        Item.value = Item.sellPrice(0, 0, 80);
        Item.width = 34;
        Item.height = 42;
        Item.knockBack = 7.5f;
        Item.damage = 42;
        Item.rare = ItemRarityID.Blue;
        Item.useTime = 30;
        distance = 30f;
        slamDistance = 75f;
        blockDuration = 140;
        shouldFlip = true; 
    }
}