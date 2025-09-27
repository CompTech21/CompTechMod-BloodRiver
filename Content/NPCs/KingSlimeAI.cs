using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;

namespace CompTechMod.Content.NPCs
{
    public class KingSlimeAI : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private int frostTimer;
        private int slimeSpawnTimer;

        public override void AI(NPC npc)
        {
            if (npc.type != NPCID.KingSlime)
                return;

            Player target = Main.player[npc.target];
            if (!target.active || target.dead)
                return;

            // ============================
            // 1 + 2. Спавн слизней
            // ============================
            slimeSpawnTimer++;
            if (slimeSpawnTimer >= 60) // каждую секунду пытается спавнить
            {
                slimeSpawnTimer = 0;

                int slimeCount = Main.npc.Count(n => n.active && (
                    n.type == NPCID.SlimeSpiked ||
                    n.type == NPCID.SpikedIceSlime ||
                    n.type == NPCID.RainbowSlime
                ));

                if (slimeCount < 7) // ограничение
                {
                    int[] slimeTypes = new int[]
                    {
                        NPCID.SlimeSpiked,
                        NPCID.SpikedIceSlime,
                        NPCID.RainbowSlime,
                        NPCID.BlueSlime,
                    };

                    int chosen = Main.rand.Next(slimeTypes.Length);
                    NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, slimeTypes[chosen]);
                }
            }

            // ============================
            // 3. FrostBlast веером каждые 3,2 сек
            // ============================
            frostTimer++;
            if (frostTimer >= 200)
            {
                frostTimer = 0;

                Vector2 direction = target.Center - npc.Center;
                direction.Normalize();

                int numberProjectiles = 6;
                float rotation = MathHelper.ToRadians(25f);

                for (int i = 0; i < numberProjectiles; i++)
                {
                    Vector2 perturbedSpeed = direction.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (float)(numberProjectiles - 1)));
                    perturbedSpeed *= 8f;

                    Projectile.NewProjectile(
                        npc.GetSource_FromAI(),
                        npc.Center,
                        perturbedSpeed,
                        ProjectileID.FrostBlastHostile,
                        20,
                        2f,
                        Main.myPlayer
                    );
                }
            }

            // ============================
            // 4. Супер прыжки и быстрое падение
            // ============================
            if (npc.velocity.Y == 0f && Main.rand.NextBool(100)) // когда стоит на земле — шанс подпрыгнуть выше
            {
                npc.velocity.Y = -15f; // супер прыжок (ваниль около -8)
            }
            if (npc.velocity.Y > 0f) // при падении
            {
                npc.velocity.Y += 0.4f; // быстрее ускоряется вниз
            }

            // ============================
            // 5. Фаза при малом HP
            // ============================
            if (npc.life < npc.lifeMax * 0.2f) // <20% HP
            {
                if (npc.velocity.Y == 0f && Main.rand.NextBool(5)) // очень часто мелкие прыжки
                {
                    npc.velocity.Y = -6f; // маленький быстрый прыжок
                }
                if (npc.velocity.Y > 0f)
                {
                    npc.velocity.Y += 0.6f; // падение ещё быстрее
                }
                npc.knockBackResist = 0f; // почти не отбрасывается
            }
        }
    }
}
