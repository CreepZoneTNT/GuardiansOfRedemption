using System.Numerics;
using OrchidMod.Content.Guardian;
using OrchidMod.Content.Guardian.Misc;
using OrchidMod.Content.Guardian.Projectiles.Misc;
using OrchidMod.Content.Guardian.Weapons.Gauntlets;
using OrchidMod.Content.Guardian.Weapons.Misc;
using OrchidMod.Content.Guardian.Weapons.Quarterstaves;
using OrchidMod.Content.Guardian.Weapons.Runes;
using OrchidMod.Content.Guardian.Weapons.Shields;
using OrchidMod.Content.Guardian.Weapons.Warhammers;
using Redemption.BaseExtension;
using Redemption.Globals;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GuardiansOfRedemption
{
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
	public class GuardiansOfRedemption : Mod
	{
		public static GuardiansOfRedemption Instance { get; private set; }
		public static OrchidMod.OrchidMod OrchidMod { get; private set;}
		public static Redemption.Redemption Redemption { get; private set; }

		public override void PostSetupContent()
		{
			foreach (var modItem in ModContent.GetContent<ModItem>())
			{
					if (modItem is EnchantedPavise or SkywareShield or BijouShield or SpectreShield or MoonLordShield or PumpkingWarhammer or ThoriumDarksteelGauntlet or CrystalGauntlet or PaladinGauntlet  or ThoriumThoriumQuarterstaff or ShardQuarterstaff or SpectreQuarterstaff or EnchantedRune or RuneRune)
						ElementID.ItemArcane[modItem.Type] = true;
					if (modItem is MeteoriteShield or HellWarhammer or FlamingQuarterstaff or GoblinRune or HellRune or RuneRune)
						ElementID.ItemFire[modItem.Type] = true;
					if (modItem is TrashPavise or ThoriumIllusionistPavise or PirateWarhammer or DungeonQuarterstaff or ThoriumAquaiteQuarterstaff)
						ElementID.ItemWater[modItem.Type] = true;
					if (modItem is FrostRune)
						ElementID.ItemIce[modItem.Type] = true;
					if (modItem is MeteoriteShield or ThoriumBronzeShield or DesertWarhammer or TempleWarhammer or JewelerGauntlet or ThoriumGraniteGauntlet or CrystalGauntlet)
						ElementID.ItemEarth[modItem.Type] = true;
					if (modItem is ThoriumGrandThunderBirdWarhammer or ThoriumFeatherWarhammer)
						ElementID.ItemWind[modItem.Type] = true;
					if (modItem is ThoriumGrandThunderBirdWarhammer or MartianWarhammer or ThoriumGraniteGauntlet or MoonLordRune)
						ElementID.ItemThunder[modItem.Type] = true;
					if (modItem is ThoriumBronzeShield or HallowedShield or HorizonShield or PaladinGauntlet or ShardQuarterstaff or EmpressRune or HorizonLance)
						ElementID.ItemHoly[modItem.Type] = true;
					if (modItem is DemoniteShield or NightShield or CorruptionWarhammer or PumpkingWarhammer or ThoriumDarksteelGauntlet or CorruptionQuarterstaff or ThoriumViscountQuarterstaff or ShardQuarterstaff or GoblinRune  or HorizonPickaxe or HorizonDrill or HorizonHamaxe)
						ElementID.ItemShadow[modItem.Type] = true;
					if (modItem is ThoriumLeafShield or ChlorophyteShield or JungleWarhammer or ChlorophyteWarhammer or CactusGauntlet or GlowingMushroomGauntlet or JungleGauntlet or BeeGauntlet or VerveineQuarterstaff or LivingRune or BeeRune)
						ElementID.ItemNature[modItem.Type] = true;
					if (modItem is  JungleWarhammer or BeeGauntlet or JungleGauntlet or SpiderGauntlet or VerveineQuarterstaff or BeeRune)
						ElementID.ItemPoison[modItem.Type] = true;
					if (modItem is CrimtaneShield or CrimsonWarhammer or CrimsonQuarterstaff or ThoriumViscountQuarterstaff)
						ElementID.ItemBlood[modItem.Type] = true;
					if (modItem is ThoriumIllusionistPavise or NanitesGauntlet or MoonLordRune)
						ElementID.ItemPsychic[modItem.Type] = true;
					if (modItem is SkywareShield or HorizonShield or MoonLordShield or EmpressRune or MoonLordRune or HorizonLance or HorizonPickaxe or HorizonDrill or HorizonHamaxe)
						ElementID.ItemCelestial[modItem.Type] = true;
					if (modItem is HellWarhammer)
						ElementID.ItemExplosive[modItem.Type] = true;
			}

			foreach (var modProjectile in ModContent.GetContent<ModProjectile>())
			{
				if (modProjectile is GuardianHorizonLanceProj)
					modProjectile.Projectile.Redemption().IsHammer = false;
					
			}
		}
	}
}
