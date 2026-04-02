using System.Text;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.General.Global;

public class MoRGuardianPlayer : ModPlayer
{
    public bool GuardianXenomiteChain;
    public bool GuardianOmegaChain;
    public bool GuardianCosmosChain;
    public bool GuardianHeavyGuard;

    public override void ResetEffects()
    {
        GuardianHeavyGuard = false;
    
        GuardianXenomiteChain = false;
        GuardianOmegaChain = false;
        GuardianCosmosChain = false;
    }
}