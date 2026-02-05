using Terraria.ModLoader;

namespace GuardiansOfRedemption.General.Global;

public class MoRGuardianPlayer : ModPlayer
{
    public bool CosmosChainUpdates;

    public override void ResetEffects() =>  CosmosChainUpdates = false;
}