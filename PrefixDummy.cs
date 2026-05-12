using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption;

public class PrefixDummy : ModItem
{
    public override string Texture => "GuardiansOfRedemption/icon_small";

    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 28;
        Item.value = 0;
        Item.rare = ItemRarityID.Green;
        
        Item.accessory = true;
        Item.useTime = 10;
        Item.useAnimation = 10;
        Item.damage = 10;
        Item.shoot = ProjectileID.PurificationPowder;
        Item.shootSpeed = 10f;
        Item.mana = 10;
        Item.defense = 1;
        Item.crit = 4;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.DamageType = ModContent.GetInstance<PrefixDummyDamageClass>();
    }

    // public override int ChoosePrefix(UnifiedRandom rand) => rand.Next(1, PrefixLoader.PrefixCount);
    // public override bool AllowPrefix(int pre) => true;

    public override void UpdateInventory(Player player) => Item.TurnToAir();
    
    public override void PostUpdate() => Item.TurnToAir();
}

public class PrefixDummyDamageClass : DamageClass
{
    public override bool UseStandardCritCalcs => true;

    
    public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
    {
        return damageClass == Generic ? StatInheritanceData.Full : StatInheritanceData.None;
    }    
    // public override StatInheritanceData GetModifierInheritance(DamageClass damageClass) => StatInheritanceData.Full;

    public override bool GetPrefixInheritance(DamageClass damageClass) => true;

    public override bool GetEffectInheritance(DamageClass damageClass) => false;

    public override void SetDefaultStats(Player player) {}
}