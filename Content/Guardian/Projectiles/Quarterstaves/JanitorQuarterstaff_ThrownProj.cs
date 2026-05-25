
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using OrchidMod.Content.Guardian;
using Redemption;
using Redemption.BaseExtension;
using Redemption.NPCs.Lab.Janitor;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Redemption.Globals;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Utilities;
using GuardiansOfRedemption.Content.Guardian.Weapons.Quarterstaves;

namespace GuardiansOfRedemption.Content.Guardian.Projectiles.Quarterstaves;
public class JanitorQuarterstaff_ThrownProj : OrchidModGuardianProjectile {
    
    public Item JanitorQuarterstaff;

    public IEntitySource Source;

    private static Texture2D TextureMain;
    public List<Vector2> OldPosition;
    public List<float> OldRotation;

    public override string Texture => "GuardiansOfRedemption/Items/Guardian/Weapons/Quarterstaves/JanitorQuarterstaff";
    
    public override void SafeSetDefaults() {
        Projectile.CloneDefaults(ModContent.ProjectileType<JanitorMop_Proj>());
        Projectile.friendly = true;
        Projectile.hostile = false;
        TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
        OldPosition = [];
        OldRotation = [];
    }

    public override void OnSpawn(IEntitySource source)
    {
        if (source is EntitySource_Parent parent && parent.Entity is Projectile parentProj && parentProj.ModProjectile is GuardianQuarterstaffAnchor anchor && anchor.QuarterstaffItem.ModItem is JanitorQuarterstaff mop){
            mop.MopYeeted = true;
            parentProj.netImportant = true;
            Source = source;
        }
    }

    public override void AI() {
        if (++Projectile.ai[0] > 3) Projectile.ai[0] = 2;
        Projectile.tileCollide = (Projectile.ai[0] == 2);

        Projectile.velocity.Y += 0.2f;
        if (Projectile.velocity.Y > 16f) Projectile.velocity.Y = 16f;
        Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;

        OldPosition.Add(Projectile.Center);
        OldRotation.Add(Projectile.rotation);

        if (OldPosition.Count > 10)
        {
            OldPosition.RemoveAt(0);
            OldRotation.RemoveAt(0);
        }
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        for (int i = 0; i < 4; i++)
        {
            int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.WoodFurniture, 0f, 0f, 100, default, 2.0f);
            Main.dust[dustIndex].velocity *= 2f;
        }
        if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X;
        if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y;
        Projectile.velocity *= 0.95f;
        
        return false;
    }

    public override void SafeModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {
        if (target.type == ModContent.NPCType<JanitorBot>() && target.ai[0] != 4 && target.ai[0] != 5)
        {
            target.RedemptionGuard().GuardPoints = 0;
            target.ai[0] = 4;
            target.ai[1] = 0;
            target.ai[2] = 0;
            target.RedemptionGuard().GuardBreakCheck(target, DustID.Electric, CustomSounds.GuardBreak, 10, 1, 1000);
            target.netUpdate = true;
        }
        Projectile.Kill();
    }
    public override void OnKill(int timeLeft)
    {
        SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
        for (int i = 0; i < 8; i++)
            Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.WoodFurniture, -Projectile.velocity.X * 0.2f,
                -Projectile.velocity.Y * 0.2f);

        if (Source is EntitySource_Parent parent && parent.Entity is Projectile parentProj && parentProj.ModProjectile is GuardianQuarterstaffAnchor anchor && anchor.QuarterstaffItem.ModItem is JanitorQuarterstaff mop){
            mop.MopYeeted = false;
            RedeDraw.SpawnRing(mop.tip, Color.White, 0.02f, 1.0f);
            SoundEngine.PlaySound(CustomSounds.Bell with {Volume = 0.2f, Pitch = 0.1f}, mop.tip);
            parentProj.netImportant = true;
        }
        if (!Main.dedServ) {
            Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity, ModContent.Find<ModGore>("Redemption/JanitorMopGore1").Type, 1);
            Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position + new Vector2(6, 22), Projectile.velocity, ModContent.Find<ModGore>("Redemption/JanitorMopGore2").Type, 1);
        }
    }

    public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			for (int i = 0; i < OldPosition.Count; i++)
			{
				Vector2 drawPosition = OldPosition[i] - Main.screenPosition;
				spriteBatch.Draw(TextureMain, drawPosition, null, Color.White * ((OldPosition.Count - i) / OldPosition.Count), OldRotation[i], TextureMain.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);
			}

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);
			return true;
		}
}