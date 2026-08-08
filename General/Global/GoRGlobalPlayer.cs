using Terraria.ModLoader;

namespace GuardiansOfRedemption.General.Global;

public class GoRGlobalPlayer : ModPlayer
{
    
    public bool BasanDebuff = false;
    public int BasanDebuffDuration = 0;

    public override void ResetEffects()
    {
        BasanDebuff = false;
        BasanDebuffDuration = 0;
    }

    public override void UpdateBadLifeRegen()
    {
        if (BasanDebuff)
        {            
            if (Player.lifeRegen > 0)
                Player.lifeRegen = 0;
            
            Player.lifeRegenTime = 0;
            Player.lifeRegen -= 8 + (BasanDebuffDuration / 4);

            if (BasanDebuffDuration > 480)
            {
                BasanDebuffDuration = 480;
            }
        }
    }
}