using OrchidMod.Content.Guardian;
using Terraria;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Content.Guardian.Buffs;

public class ZephosWarhammerBuff : ModBuff
{
    public override void Update(Player player, ref int buffIndex)
    {
        if (player.HeldItem.ModItem is OrchidModGuardianHammer)
        {
            player.GetDamage<GuardianDamageClass>() *= 1.25f;
            player.GetAttackSpeed(DamageClass.Melee) += 0.25f;
        }
    }
}