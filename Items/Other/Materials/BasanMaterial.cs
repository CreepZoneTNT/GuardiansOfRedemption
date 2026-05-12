using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.GameContent.UI;
using Terraria.ID;

namespace GuardiansOfRedemption.Items.Other.Materials
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
