using System.Collections.Generic;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace GuardiansOfRedemption.General;

public class ClientConfig : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ClientSide;
    
    public enum ClassTagModes
    {
        Show,
        Hide,
        FollowOrchid
    }
    
    [Header("ClientTweaks")]
    [Cycle]
    [DefaultValue(ClassTagModes.FollowOrchid)]
    public ClassTagModes ClassTooltipMode { get; set; }
}

public class ServerConfig : ModConfig
{


    public override ConfigScope Mode => ConfigScope.ServerSide;
    
    [Header("OpinionatedChanges")]
    [ReloadRequired]
    [DefaultValue(false)]
    public bool ReforgesGoBeyondPurple { get; set; }
    
    // [ReloadRequired]  
    [DefaultValue(false)]
    public bool VanillaGoesBeyondPurple { get; set; }
    
    // [ReloadRequired]
    public Dictionary<string, bool> ModsToGoBeyondPurple { get; set; } = new()
    {
        { "OrchidMod", true },
        { "Redemption", true },
        { "GuardiansOfRedemption", true }
    };
    
    [ReloadRequired]
    [DefaultValue(true)]
    public bool EnableCoralRarityForReforges { get; set; }
    
    public enum BeyondRarityModes
    {
        Soft,
        Hard
    }
    [ReloadRequired]
    [DefaultValue(BeyondRarityModes.Soft)]
    public BeyondRarityModes SetModeForBeyondPurple { get; set; } 
    
    [ReloadRequired]
    public bool EnableRecipeBrowserCompat { get; set; }
    
    [ReloadRequired]
    public ItemDefinition RecipeBrowserCompatIcon { get; set; } = new("OrchidMod", "HellWarhammer");
}