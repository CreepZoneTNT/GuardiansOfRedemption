using Terraria.ModLoader;
using Terraria;
using Terraria.ID;

namespace GuardiansOfRedemption.Content.Other.Materials
{
    public class BasanMaterial : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
        }
        public override void SetDefaults()
        {
            Item.height = 30;
            Item.width = 26;

            Item.rare = ItemRarityID.Blue;
            Item.buyPrice(0, 0, 5, 0);
            
        }
    }
}
