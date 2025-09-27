using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Content.NPCs
{
    public class BrainOfCthulhuAI : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public override void AI(NPC npc)
        {
            // =============================
            // Фаза мозга — дебаффы игроку после 50% HP
            // =============================
            if (npc.type == NPCID.BrainofCthulhu)
            {
                float hpPercent = (float)npc.life / npc.lifeMax;

                // накладываем дебаффы только если мозг ниже 50% HP
                if (hpPercent <= 0.5f)
                {
                    Player target = Main.player[npc.target];
                    if (target.active && !target.dead)
                    {
                        // накладываем дебаффы "бесконечно"
                        target.AddBuff(BuffID.Ichor, 2);          // Ихор (69)
                        target.AddBuff(BuffID.Confused, 2);       // Замешательство (31)
                    }
                }
            }

            // =============================
            // Creepers (прислужники мозга)
            // =============================
            if (npc.type == NPCID.Creeper)
            {
                // Убираем ванильный дроп руды и тканей
                npc.value = 0;

                // индивидуальный таймер для каждой проныры
                npc.localAI[0]++;

                // Каждые 2 секунды стреляют BloodShot
                if (npc.localAI[0] >= 120 && npc.HasValidTarget)
                {
                    npc.localAI[0] = 0; // сброс счётчика

                    Player target = Main.player[npc.target];
                    if (target.active && !target.dead)
                    {
                        Vector2 dir = Vector2.Normalize(target.Center - npc.Center) * 8f;
                        Projectile.NewProjectile(
                            npc.GetSource_FromAI(),
                            npc.Center,
                            dir,
                            ProjectileID.BloodShot,
                            15,
                            1f,
                            Main.myPlayer
                        );
                    }
                }
            }
        }
    }
}
