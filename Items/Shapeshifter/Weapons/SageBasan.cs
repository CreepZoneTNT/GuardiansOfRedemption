using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using OrchidMod.Content.Shapeshifter;
using Redemption;

public class SageBasan : OrchidModShapeshifterShapeshift
{
    public override void SafeSetDefaults()
    {
        Item.width = 30;
        Item.height = 30;
        Item.value = Item.sellPrice(0, 1, 55, 0);
        Item.rare = ItemRarityID.Orange;
        Item.UseSound = CustomSounds.RoosterRoar with {PitchRange = (0.4f, 0.6f)};
        Item.useTime = 35;
        Item.shootSpeed = 10f;
        Item.knockBack = 3f;
        Item.damage = 60;
        ShapeshiftWidth = 20;
        ShapeshiftHeight = 50;
        ShapeshiftType = ShapeshifterShapeshiftType.Sage;
        GroundedWildshape = true;
    }
}