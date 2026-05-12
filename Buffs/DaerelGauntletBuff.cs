using GuardiansOfRedemption.Items.Weapons.Gauntlets;
using OrchidMod.Content.Guardian;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Buffs;

public class DaerelGauntletBuff : ModBuff
{
    public override void SetStaticDefaults()
    {
        Main.buffNoTimeDisplay[Type] = false;
        Main.buffNoSave[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex)
    {
        player.moveSpeed += 0.25f;
        if (player.HeldItem != null && player.HeldItem.ModItem is DaerelGauntlet) player.GetAttackSpeed(DamageClass.Melee) += 0f;
    }
}