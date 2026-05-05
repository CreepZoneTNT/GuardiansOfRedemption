using GuardiansOfRedemption.General;
using GuardiansOfRedemption.Projectiles.Shields;
using Microsoft.Xna.Framework;
using OrchidMod;
using OrchidMod.Content.Guardian;
using OrchidMod.Content.Guardian.Projectiles.Shields;
using Redemption.Items.Materials.HM;
using Redemption.Items.Materials.PostML;
using Redemption.Tiles.Furniture.Lab;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption.Items.Weapons.Shields;

public class WastelandShield : OrchidModGuardianShield
{
    public int timer = 0;
    public override void SafeSetDefaults()
    {
        Item.value = Item.sellPrice(0, 10, 0 ,0);
        Item.width = 60;
        Item.height = 74;
        Item.knockBack = 10f;
        Item.damage = 175;
        Item.rare = ItemRarityID.LightRed;
        Item.useTime = 50;
        Item.shootSpeed = 18f;
        distance = 32f;
        slamDistance = 10f;
        blockDuration = 480;
        shouldFlip = true;
    }
    public override void Slam(Player player, Projectile shield)
    {
        timer = 30;
    }
    public override void ExtraAIShield(Projectile shield) 
    {
        if (shield.owner == Main.myPlayer && shield.ModProjectile is GuardianShieldAnchor anchor)
        {
            OrchidGuardian guardian = Main.LocalPlayer.Guardian();

            if (timer % 10 == 0 && timer > 0)
            {
                SoundEngine.PlaySound(SoundID.Item11);
                Vector2 dir = Vector2.Normalize(Main.MouseWorld - Main.LocalPlayer.Center) * Item.shootSpeed;
                Projectile newProjectile = Projectile.NewProjectileDirect(Item.GetSource_FromThis(), shield.Center, dir, ModContent.ProjectileType<WastelandShield_Proj>(), guardian.GetGuardianDamage(shield.damage / 2.5f), Item.knockBack / 5, Main.LocalPlayer.whoAmI);
                newProjectile.rotation = (dir.ToRotation());
                newProjectile.CritChance = (int)(Main.LocalPlayer.GetCritChance<GuardianDamageClass>() + Main.LocalPlayer.GetCritChance<GenericDamageClass>() + Item.crit);
            }

            if (--timer <= 0) timer = 0;
        }
    }
}