using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod;
using OrchidMod.Content.Guardian;
using OrchidMod.Utilities;
using Redemption;
using Redemption.BaseExtension;
using Redemption.Buffs.Debuffs;
using Redemption.Buffs.NPCBuffs;
using Redemption.Dusts;
using Redemption.Globals;
using Redemption.Projectiles.Melee;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace GuardiansOfRedemption.Items.Weapons.Warhammers;

public class WastelandWarhammer : OrchidModGuardianHammer
{
    

    public override void SetStaticDefaults()
    { 
        ElementID.ItemThunder[Type] = true;
    }

    public override void SafeSetDefaults()
    {
        Item.width = 40;
        Item.height = 30;
        Item.value = Item.sellPrice(0, 2);
        Item.rare = ItemRarityID.LightRed;
        Item.UseSound = SoundID.DD2_MonkStaffSwing;
        Item.knockBack = 5f;
        Item.shootSpeed = 16f;
        Item.damage = 300;
        Item.useTime = 30;
        Range = 80;
        SwingChargeGain = 1.2f;
        BlockDuration = 180;
    }

    

    public override void ExtraAI(Player player, OrchidGuardian guardian, Projectile projectile)
    {
        
    }

    public override void OnMeleeHit(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, float knockback, bool crit, bool FullyCharged)
    {
        target.AddBuff(ModContent.BuffType<GreenRashesDebuff>(), 240);
    }
}