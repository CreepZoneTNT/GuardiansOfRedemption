using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using GuardiansOfRedemption.Items.Accessories;
using Redemption.Items.Placeable.Containers;
using Redemption.Tiles.Containers;
using GuardiansOfRedemption.Items.Weapons.Gauntlets;
using GuardiansOfRedemption.Items.Weapons.Warhammers;
using Redemption.Tiles.Furniture.Lab;
using Redemption.WorldGeneration;
using Redemption.Items.Tools.PostML;
using Redemption.Items.Materials.PostML;

namespace GuardiansOfRedemption.General.Global
{
    public class GlobalWorld : ModSystem
    {
        public override void PostWorldGen()
        {
            // This is simply generating a line of Chlorophyte halfway down the world.
            //for (int i = 0; i < Main.maxTilesX; i++)
            //{
            //	Main.tile[i, Main.maxTilesY / 2].type = TileID.Chlorophyte;
            //}

            // Place some items in Ice Chests
            

            bool GathicItem = false;
            bool LabItem = false;

            for (int chestIndex = 0; chestIndex < Main.maxChests; chestIndex++)
            {
                Chest chest = Main.chest[chestIndex];
                if (chest != null && Main.tile[chest.x, chest.y].TileType == ModContent.TileType<ElderWoodChestTile>())
                {
                    if (!GathicItem || WorldGen.genRand.NextBool(5))
                    {
                        chest.item[1].SetDefaults(ModContent.ItemType<ProtectiveAmulet>());
                        GathicItem = true;
                    }
                }
            }
            for (int chestIndex = 0; chestIndex < Main.maxChests; chestIndex++)
            {
                Chest chest = Main.chest[chestIndex];
                if (chest != null && Main.tile[chest.x, chest.y].TileType == ModContent.TileType<LabChestTileLocked>())
                {
                    if (!LabItem || WorldGen.genRand.NextBool(4))
                    {
                        chest.item[0].SetDefaults(ModContent.ItemType<LaboratoryGauntlet>());
                        LabItem = true;
                    }
                }
            }
            for (int chestIndex = 0; chestIndex < Main.maxChests; chestIndex++)
            {
                Chest chest = Main.chest[chestIndex];
                if (chest != null && Main.tile[chest.x, chest.y].TileType == ModContent.TileType<LabChestTileLocked2>())
                {
                    for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                    {
                        if (chest.item[0].type != ModContent.ItemType<NanoPickaxe>())
                        {
                            chest.item[4].SetDefaults(ModContent.ItemType<PZWarhammer>());
                            chest.item[5].SetDefaults(ModContent.ItemType<RawXenium>());
                            chest.item[5].stack = Main.rand.Next(140, 159);
                            chest.item[6].SetDefaults(ItemID.LunarOre);
                            chest.item[6].stack = Main.rand.Next(140, 159);
                            // Alternate approach: Random instead of cyclical: chest.item[inventoryIndex].SetDefaults(Main.rand.Next(itemsToPlaceInIceChests));
                            break;
                        }
                    }
                }
            }
        }
    }
}
