using GuardiansOfRedemption.General.Global;
using Microsoft.Xna.Framework;
using OrchidMod;
using OrchidMod.Content.Guardian;
using Redemption.BaseExtension;
using Terraria;

namespace GuardiansOfRedemption.General;

public static class GoRBaseExtension
{
    
    
    public static OrchidGuardian Guardian(this Player player) => player.GetModPlayer<OrchidGuardian>();
    
    public static RedemptionGuardian RedemptionGuardian(this Player player) => player.GetModPlayer<RedemptionGuardian>();
    public static RedemptionGuardian RedemptionGuardian(this OrchidGuardian guardian) => guardian.Player.GetModPlayer<RedemptionGuardian>();
    
    
    public static Vector2 GetQuarterstaffTip(this GuardianQuarterstaffAnchor quarterstaff, float amount = 0.1f)
    {
        Projectile anchor = quarterstaff.Projectile;
        
        Vector2 tipPosition = anchor.Center - Vector2.UnitY.RotatedBy(anchor.rotation + MathHelper.PiOver4) * anchor.width * amount;
        
        return tipPosition;
    }
}