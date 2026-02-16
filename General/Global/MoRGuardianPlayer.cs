using System;
using System.Reflection;
using System.Text;
using Humanizer;
using Microsoft.Xna.Framework;
using OrchidMod;
using OrchidMod.Content.Guardian;
using OrchidMod.Content.Guardian.Projectiles.Misc;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.General.Global;

public class MoRGuardianPlayer : OrchidGuardian
{

    public bool GuardianHeavyGuard;
    
    public bool GuardianSpikeNuclear;
    
    public bool GuardianXenomiteChain;
    public bool GuardianOmegaChain;
    public bool GuardianCosmosChain;
    

    public override void ResetEffects()
    {
        GuardianHeavyGuard = false;
        
        GuardianSpikeNuclear = false;
    
        GuardianXenomiteChain = false;
        GuardianOmegaChain = false;
        GuardianCosmosChain = false;
    }

}