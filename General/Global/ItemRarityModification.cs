using System;
using System.Collections.Generic;
using GuardiansOfRedemption.Rarities;
using Microsoft.Xna.Framework;
using Redemption.Rarities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.General.Global;

public class ItemRarityModification : GlobalItem
{ 
    
    public override bool IsLoadingEnabled(Mod mod) => ModContent.GetInstance<ServerConfig>().ReforgesGoBeyondPurple;
    
    public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.CanHavePrefixes();

    public static Dictionary<int, int> DeltaRarityTable;

    public override void SetStaticDefaults()
    {
        DeltaRarityTable = new Dictionary<int, int>();
        
        int refItemType = ModContent.ItemType<PrefixDummy>();
        for (int i = 0; i < PrefixLoader.PrefixCount; i++)
        {
            Item refModItem = new Item(refItemType, prefix: i);
            Item refBaseItem = new Item(refItemType);
            int deltaRarity = refModItem.rare - refBaseItem.rare;
	
            DeltaRarityTable.TryAdd(i, deltaRarity);
        }
        
    }

    // We can do this the easy way...
    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {
        var config = ModContent.GetInstance<ServerConfig>();
        if (config.SetModeForBeyondPurple == ServerConfig.BeyondRarityModes.Soft)
        {
            if (item.ModItem != null) Console.Out.WriteLine(item.AffixName() + ": modded item, " + (config.ModsToGoBeyondPurple != null && config.ModsToGoBeyondPurple.TryGetValue(item.ModItem.Mod.Name, out bool test) && test));
            else Console.Out.WriteLine(item.AffixName() + ": vanilla item, " + ModContent.GetInstance<ServerConfig>().VanillaGoesBeyondPurple);
        
            if ((item.ModItem == null && ModContent.GetInstance<ServerConfig>().VanillaGoesBeyondPurple) || (item.ModItem != null && config.ModsToGoBeyondPurple != null && config.ModsToGoBeyondPurple.TryGetValue(item.ModItem.Mod.Name, out bool value) && value))
            {
                foreach (TooltipLine line in tooltips)
                {
                    if (line.Mod == "Terraria" && line.Name == "ItemName")
                    {
                        
                        int deltaRarity = CalculateDeltaRarity(item, item.prefix, out int baseRare);
                        // Console.Out.WriteLine(refModItem.AffixName() + ", unmodified " + refBaseItem.rare + ", modified " + refModItem.rare);
                        // Console.Out.WriteLine(deltaRarity);
                        
                        Console.Out.WriteLine(item.AffixName() + ", unmodified " + baseRare + ", modified " + item.rare + ", with delta " + (baseRare + deltaRarity));
                        
                        Color? nameColor;
                        if (ModContent.GetInstance<ServerConfig>().EnableCoralRarityForReforges && ((baseRare <= ItemRarityID.Purple && baseRare + deltaRarity == 13) || (baseRare == ModContent.RarityType<TurquoiseRarity>() && deltaRarity > 0)))
                        {
                            nameColor = ModContent.GetInstance<CoralRarity>().RarityColor;
                            Console.Out.WriteLine("Should set to Coral");
                        }
                        else if ((baseRare <= ItemRarityID.Purple && baseRare + deltaRarity == 12) || (baseRare == ModContent.RarityType<TurquoiseRarity>() && deltaRarity == 0))
                        {
                            nameColor = ModContent.GetInstance<TurquoiseRarity>().RarityColor;
                            Console.Out.WriteLine("Should set to Turquoise");
                        }
                        else if (baseRare == ModContent.RarityType<TurquoiseRarity>() && deltaRarity == -1)
                        {
                            nameColor = Colors.RarityDarkPurple;
                            Console.Out.WriteLine("Should set to Purple");
                        }
                        else if (baseRare == ModContent.RarityType<TurquoiseRarity>() && deltaRarity == -2)
                        {
                            nameColor = Colors.RarityDarkRed;
                            Console.Out.WriteLine("Should set to Red");
                        }
                        else
                        {
                            nameColor = null;
                            Console.Out.WriteLine("Should leave as is");
                        }
                        
                        Console.Out.WriteLine();

                        line.OverrideColor = nameColor;
                    }
                }
            }
        }
    }

    public override void ApplyPrefix(Item item, int pre)
    {
        var config = ModContent.GetInstance<ServerConfig>();
        if (config.SetModeForBeyondPurple == ServerConfig.BeyondRarityModes.Hard)
        {
            if (item.ModItem != null) Console.Out.WriteLine(item.AffixName() + ": modded item, " + (config.ModsToGoBeyondPurple != null && config.ModsToGoBeyondPurple.TryGetValue(item.ModItem.Mod.Name, out bool test) && test));
            else Console.Out.WriteLine(item.AffixName() + ": vanilla item, " + ModContent.GetInstance<ServerConfig>().VanillaGoesBeyondPurple);
        
            if ((item.ModItem == null && ModContent.GetInstance<ServerConfig>().VanillaGoesBeyondPurple) || (item.ModItem != null && config.ModsToGoBeyondPurple != null && config.ModsToGoBeyondPurple.TryGetValue(item.ModItem.Mod.Name, out bool value) && value))
            {
                int deltaRarity = CalculateDeltaRarity(item, pre, out int baseRare);
                Console.Out.WriteLine(item.Name + " with prefix " + Lang.prefix[pre].Value + ": unmodified " + baseRare + ", modified " + item.rare + ", with delta " + (baseRare + deltaRarity));
                
                if (ModContent.GetInstance<ServerConfig>().EnableCoralRarityForReforges && ((baseRare <= ItemRarityID.Purple && baseRare + deltaRarity == 13) || (baseRare == ModContent.RarityType<TurquoiseRarity>() && deltaRarity > 0)))
                {
                    item.rare = ModContent.RarityType<CoralRarity>();
                    Console.Out.WriteLine("Should set to Coral");
                }
                else if ((baseRare <= ItemRarityID.Purple && baseRare + deltaRarity == 12) || (baseRare == ModContent.RarityType<TurquoiseRarity>() && deltaRarity == 0))
                {
                    item.rare = ModContent.RarityType<TurquoiseRarity>();
                    Console.Out.WriteLine("Should set to Turquoise");
                }
                else if (baseRare == ModContent.RarityType<TurquoiseRarity>() && deltaRarity == -1)
                {
                    item.rare = ItemRarityID.Purple;
                    Console.Out.WriteLine("Should set to Purple");
                }
                else if (baseRare == ModContent.RarityType<TurquoiseRarity>() && deltaRarity == -2)
                {
                    item.rare = ItemRarityID.Red;
                    Console.Out.WriteLine("Should set to Red");
                }
            }
        }
    }

    public int CalculateDeltaRarity(Item item, int pre, out int baseRare)
    {
        baseRare = item.rare;
        if (!item.CanHavePrefixes() || !item.CanApplyPrefix(pre)) return 0;
        
        int deltaRarity = 0;

        if (DeltaRarityTable.TryGetValue(pre, out int deltaRare))
            deltaRarity = deltaRare;

        baseRare = ContentSamples.ItemsByType[item.type].rare;
        return deltaRarity;
    }
}