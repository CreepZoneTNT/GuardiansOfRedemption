using GuardiansOfRedemption.General.Global;
using Microsoft.Xna.Framework;
using OrchidMod;
using OrchidMod.Content.Guardian;
using Terraria;
using Terraria.Utilities;

namespace GuardiansOfRedemption.General;

public static class GoRBaseExtension
{
    
    
    public static RedemptionGuardian RedemptionGuardian(this Player player) => player.GetModPlayer<RedemptionGuardian>();
    public static RedemptionGuardian RedemptionGuardian(this OrchidGuardian guardian) => guardian.Player.GetModPlayer<RedemptionGuardian>();

    
    public static Vector2 NextVector2RectangleEdge(this UnifiedRandom random, int x, int y, float width, float height) {
        Vector2 pos = random.Next(4) switch {
            0 => new(x + (width / 2f), y + random.NextFloat(-(height / 2f), height / 2f)),
            1 => new(x + random.NextFloat(-(width / 2f), width / 2f), y + (height / 2f)),
            2 => new(x - (width / 2f), y + random.NextFloat(-(height / 2f), height / 2f)),
            3 => new(x + random.NextFloat(-(width / 2f), width / 2f), y - (height / 2f)),
            _ => new(x + random.NextFloat(-(width / 2f)), y + random.NextFloat(-(height / 2f), height / 2f))
        };
        return pos;
    }

    public static Vector2 NextVector2RectangleEdge(this UnifiedRandom random, Vector2 center, Vector2 size) => random.NextVector2RectangleEdge((int)center.X, (int)center.Y, size.X, size.Y);

    public static Vector2 NextVector2RectangleEdge(this UnifiedRandom random, Rectangle rect) => random.NextVector2RectangleEdge(rect.X + (rect.Width / 2), rect.Y + (rect.Height / 2), rect.Width, rect.Height);
    
    public static Vector2 GetQuarterstaffTip(this GuardianQuarterstaffAnchor quarterstaff, float amount = 0.1f)
    {
        Projectile anchor = quarterstaff.Projectile;
        
        Vector2 tipPosition = anchor.Center - Vector2.UnitY.RotatedBy(anchor.rotation + MathHelper.PiOver4) * anchor.width * amount;
        
        return tipPosition;
    }
}