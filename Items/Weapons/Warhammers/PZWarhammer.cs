using GuardiansOfRedemption.Buffs;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod;
using OrchidMod.Content.Guardian;
using Redemption;
using Redemption.BaseExtension;
using Redemption.Buffs.NPCBuffs;
using Redemption.Globals;
using Redemption.Rarities;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace GuardiansOfRedemption.Items.Weapons.Warhammers;

public class PZWarhammer : OrchidModGuardianHammer
{
    public override void SetStaticDefaults()
    {
        ElementID.ItemPoison[Type] = true;
    }

    public override void SafeSetDefaults()
    {
        Item.width = 62;
        Item.height = 50;
        Item.value = Item.sellPrice(20, 0);
        Item.rare = ModContent.RarityType<TurquoiseRarity>();
        Item.UseSound = SoundID.Item1;
        Item.knockBack = 20f;
        Item.shootSpeed = 20f;
        Item.damage = 770;
        Item.useTime = 50;
        Range = 60;
        SlamStacks = 2;
        ReturnSpeed = 0.6f;
        BlockDuration = 480;
        // Item.Redemption().TechnicallyHammer = true;
        Item.Redemption().CanSwordClash = true;
    }

    public override void OnThrowHitFirst(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, float knockback, bool crit, bool Weak)
    {
        if (!Weak)
        {
            Projectile.NewProjectileDirect(projectile.GetSource_FromAI(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<PZWarhammer_Cyst>(), guardian.GetGuardianDamage(projectile.damage * 0.5f), 0, projectile.owner, target.whoAmI);
        }
        
    }
    public override void OnMeleeHit(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, float knockback, bool crit, bool FullyCharged)
    {
        //target.AddBuff(ModContent.BuffType<ViralityDebuff>(), 90);
        Projectile.NewProjectileDirect(projectile.GetSource_FromAI(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<PZWarhammer_Cyst>(), guardian.GetGuardianDamage(projectile.damage * 0.25f), 0, projectile.owner, target.whoAmI);
    }
    public override void ExtraAI(Player player, OrchidGuardian guardian, Projectile projectile)
    {
        if (projectile.ModProjectile is GuardianHammerAnchor anchor)

            if (anchor.BlockDuration % 15 == 0 && anchor.BlockDuration != 0)
            {
                Projectile.NewProjectile(projectile.GetSource_FromAI(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<PZWarhammer_Cloud>(),guardian.GetGuardianDamage(projectile.damage * 0.1f), 0, projectile.owner);
            }
    }

public class PZWarhammer_Cloud : ModProjectile
{
    public override string Texture => "Redemption/Textures/IceMist";
    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("Virulent Gas");
    }
    public override void SetDefaults()
    {
        Projectile.DamageType = ModContent.GetInstance<GuardianDamageClass>();
        Projectile.width = 150;
        Projectile.height = 150;
        Projectile.penetrate = -1;
        Projectile.hostile = false;
        Projectile.friendly = true;
        Projectile.tileCollide = false;
        Projectile.alpha = 255;
        Projectile.timeLeft = 240;
        Projectile.scale = Main.rand.NextFloat(1, 1.5f);
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 120;
            //Projectile.rotation = RedeHelper.RandomRotation();
        }
    public override void AI()
    {
        if (Projectile.localAI[0] == 0)
            Projectile.localAI[0] = Main.rand.Next(1, 3);

        if (Projectile.localAI[0] == 1)
            Projectile.rotation -= 0.003f;
        else if (Projectile.localAI[0] == 2)
            Projectile.rotation += 0.003f;

        if (Projectile.timeLeft < 80)
        {
            Projectile.alpha += 20;
            if (Projectile.alpha >= 255)
                Projectile.Kill();
        }
        else
        {
            Projectile.alpha -= 5;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC target = Main.npc[i];
                if (!target.active || !target.CanBeChasedBy())
                    continue;

                if (!Projectile.Hitbox.Intersects(target.Hitbox))
                    continue;

                ;
                target.AddBuff(ModContent.BuffType<ViralityDebuff>(), 200);
            }
        }
    }
    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
        Vector2 drawOrigin = new(texture.Width / 2, texture.Height / 2);
        var effects = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

        Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(Color.ForestGreen), Projectile.rotation, drawOrigin, Projectile.scale, effects, 0);
        return false;
    }
  }
    public class PZWarhammer_Cyst : ModProjectile
    {
        public Vector2 OffsetAmount;
        public Vector2 Offset;

        public Texture2D DrawTexture;

        private static string TexturePath = "GuardiansOfRedemption/Items/Weapons/Warhammers/PZWarhammer_Cyst";
        private Texture2D TextureVar1 = ModContent.Request<Texture2D>(TexturePath + "1").Value;
        private Texture2D TextureVar2 = ModContent.Request<Texture2D>(TexturePath + "2").Value;
        private Texture2D TextureVar3 = ModContent.Request<Texture2D>(TexturePath + "3").Value;
        private Texture2D TextureVar4 = ModContent.Request<Texture2D>(TexturePath + "4").Value;
        private Texture2D TextureVar5 = ModContent.Request<Texture2D>(TexturePath + "5").Value;
        private Texture2D TextureVar6 = ModContent.Request<Texture2D>(TexturePath + "6").Value;
        private Texture2D TextureVar7 = ModContent.Request<Texture2D>(TexturePath + "7").Value;

        private int projVariant;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Virulent Gas");
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = ModContent.GetInstance<GuardianDamageClass>();
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.penetrate = -1;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.scale = Main.rand.NextFloat(0.8f, 1.2f);
            Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }
        public override void OnSpawn(IEntitySource source)
        {
            projVariant = Main.rand.Next(0, 7);

            switch (projVariant)
            {
                case 0:
                    DrawTexture = TextureVar1;
                    break;
                case 1:
                    DrawTexture = TextureVar2;
                    break;
                case 2:
                    DrawTexture = TextureVar3;
                    break;
                case 3:
                    DrawTexture = TextureVar4;
                    break;
                case 4:
                    DrawTexture = TextureVar5;
                    break;
                case 5:
                    DrawTexture = TextureVar6;
                    break;
                case 7:
                    DrawTexture = TextureVar7;
                    break;
                default:
                    DrawTexture = TextureVar1;
                    break;
            }

            SoundEngine.PlaySound(SoundID.Item95, Projectile.position);

            NPC latchedNPC = Main.npc[(int)Projectile.ai[0]];

            Offset = Main.rand.NextVector2FromRectangle(latchedNPC.Hitbox) - latchedNPC.Center;

            for (int i = 0; i < 15; i++)
                {
                    int dustIndex = Dust.NewDust(Projectile.Center - new Vector2(10), 20, 20, DustID.GreenBlood);
                }
        }
        public override void AI()
        {
            NPC latchedNPC = Main.npc[(int)Projectile.ai[0]];
            
            if (latchedNPC != null && latchedNPC.active && !latchedNPC.friendly)
            {
                Projectile.Center = latchedNPC.Center + Offset;
                //etc.
            }
            if (!latchedNPC.active)
            {
                SoundEngine.PlaySound(SoundID.NPCDeath22, Projectile.position);
                for (int i = 0; i < 25; i++)
                {
                    int dustIndex = Dust.NewDust(Projectile.Center - new Vector2(10), 20, 20, DustID.GreenBlood);
                }
                Projectile.Kill();
            }

            if (Projectile.timeLeft < 100)
            {
                SoundEngine.PlaySound(SoundID.NPCDeath22, Projectile.position);
                for (int i = 0; i < 15; i++)
                {
                    int dustIndex = Dust.NewDust(Projectile.Center - new Vector2(10), 20, 20, DustID.GreenBlood);
                }
                Projectile.Kill();
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (DrawTexture == null)
                return false;

            Vector2 drawOrigin = DrawTexture.Size() * 0.5f;

            Main.EntitySpriteDraw(DrawTexture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, drawOrigin, Projectile.scale, 0);

            return false;
        }
    }
}
 