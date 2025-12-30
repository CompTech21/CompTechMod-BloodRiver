using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Content.Projectiles
{
    public class BloodyChildMinion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionTargettingFeature[Type] = true;
            ProjectileID.Sets.MinionSacrificable[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.netImportant = true;

            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.minionSlots = 2f;
            Projectile.penetrate = -1;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.DamageType = DamageClass.Summon;
            Projectile.timeLeft = 18000;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!player.active || player.dead ||
                !player.HasBuff(ModContent.BuffType<Buffs.BloodyChildBuff>()))
            {
                Projectile.Kill();
                return;
            }

            Projectile.timeLeft = 2;

            NPC target = FindTarget();

            if (target != null)
            {
                Vector2 dir = target.Center - Projectile.Center;
                dir.Normalize();

                Projectile.velocity = Vector2.Lerp(
                    Projectile.velocity,
                    dir * 14f,
                    0.2f
                );
            }
            else
            {
                // 🧠 РАССЧЁТ ИНДЕКСА МИНЬОНА
                int index = 0;
                int total = 0;

                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile p = Main.projectile[i];
                    if (p.active && p.owner == Projectile.owner && p.type == Type)
                    {
                        if (p.whoAmI == Projectile.whoAmI)
                            index = total;

                        total++;
                    }
                }

                // 📐 РАСПОЛОЖЕНИЕ РЯДОМ
                float spacing = 50f;
                Vector2 idlePos = player.Center
                    + new Vector2((index - (total - 1) / 2f) * spacing, -80f);

                Projectile.velocity = (idlePos - Projectile.Center) * 0.12f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // ☣ ACID VENOM
            target.AddBuff(70, 300);
        }

        private NPC FindTarget()
        {
            NPC target = null;
            float maxDist = 900f;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.CanBeChasedBy(this))
                {
                    float dist = Vector2.Distance(npc.Center, Projectile.Center);
                    if (dist < maxDist)
                    {
                        maxDist = dist;
                        target = npc;
                    }
                }
            }
            return target;
        }
    }
}
