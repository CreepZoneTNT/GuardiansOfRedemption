using GuardiansOfRedemption.General;
using Microsoft.Xna.Framework;
using Redemption.Rarities;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Rarities;

public class CoralRarity : ModRarity
{
    public override bool IsLoadingEnabled(Mod mod) => ModContent.GetInstance<ServerConfig>().EnableCoralRarityForReforges;
    
    public override Color RarityColor => new(248, 131, 121);

    public override int GetPrefixedRarity(int offset, float valueMult)
    {
        return offset switch
        {
            -2 => ItemRarityID.Purple,
            -1 => ModContent.RarityType<TurquoiseRarity>(),
            _ => Type
        };
    }
}