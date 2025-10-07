using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System;

namespace CompTechMod.Common.Systems
{
    public class DontDoThisEffects : ModSystem
    {
        private static readonly Dictionary<int, int> guardianSpawnCooldown = new();
        private static bool guardiansSpawnedOnJoin = false;

        public override void OnWorldLoad() => guardiansSpawnedOnJoin = false;

        public override void PostUpdateWorld()
        {
            if (!CompWorld.DontDoThisMode) return;

            if (!Main.dayTime)
                Main.bloodMoon = true;

            if (!guardiansSpawnedOnJoin)
            {
                guardiansSpawnedOnJoin = true;
                foreach (Player player in Main.player)
                {
                    if (player.active)
                        SpawnGuardians(player, 6);
                }
            }
        }

        public override void PostUpdatePlayers()
        {
            if (!CompWorld.DontDoThisMode) return;

            foreach (Player player in Main.player)
            {
                if (!player.active) continue;

                var modPlayer = player.GetModPlayer<DontDoThisGlobalPlayer>();

                // "Бесконечная смерть"
                if (modPlayer.endlessDeath)
                {
                    if (!player.dead)
                        player.KillMe(PlayerDeathReason.ByCustomReason($"{player.name} shouldn't have returned..."), 9999.0, 0, false);
                    continue;
                }

                player.statDefense *= 0.5f;
                player.GetDamage(DamageClass.Generic) *= 0.5f;
                player.moveSpeed *= 0.4f;
                if (player.extraAccessorySlots > 0)
                    player.extraAccessorySlots--;

                if (!guardianSpawnCooldown.ContainsKey(player.whoAmI))
                    guardianSpawnCooldown[player.whoAmI] = 0;

                if (guardianSpawnCooldown[player.whoAmI] > 0)
                {
                    guardianSpawnCooldown[player.whoAmI]--;
                    continue;
                }

                if (Main.rand.NextFloat() < 0.05f)
                {
                    SpawnGuardians(player, 8);
                    guardianSpawnCooldown[player.whoAmI] = 720000;
                }
            }
        }

        private void SpawnGuardians(Player player, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Vector2 spawnPos = player.Center + new Vector2(Main.rand.Next(-400, 400), Main.rand.Next(-400, 400));
                int npcIndex = NPC.NewNPC(null, (int)spawnPos.X, (int)spawnPos.Y, NPCID.DungeonGuardian);
                if (npcIndex >= 0)
                {
                    Main.npc[npcIndex].target = player.whoAmI;
                    Main.npc[npcIndex].friendly = false;
                }
            }
        }
    }

    public class DontDoThisGlobalNPC : GlobalNPC
    {
        // флаг для проверки, что хп уже увеличено
        private bool scaled = false;

        public override bool InstancePerEntity => true; // ✅ Исправление

        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (!CompWorld.DontDoThisMode) return;
            if (scaled) return;
            scaled = true;

            // Боссы ×3
            if (npc.boss)
            {
                npc.lifeMax = (int)(npc.lifeMax * 3f);
                npc.life = npc.lifeMax;
            }
            // Обычные враждебные мобы ×5
            else if (!npc.friendly && !npc.townNPC && npc.lifeMax > 5)
            {
                npc.lifeMax = (int)(npc.lifeMax * 5f);
                npc.life = npc.lifeMax;
            }
        }

        public override void AI(NPC npc)
        {
            if (!CompWorld.DontDoThisMode) return;

            if (npc.townNPC)
            {
                npc.friendly = false;
                npc.target = npc.FindClosestPlayer();
            }
        }
    }

    public class DontDoThisGlobalPlayer : ModPlayer
    {
        private readonly int[] tombstoneProjectiles = new int[]
        {
            ProjectileID.Headstone,
            ProjectileID.CrossGraveMarker,
            ProjectileID.GraveMarker,
            ProjectileID.Obelisk
        };

        public bool tombstonesSpawned = false;
        public bool endlessDeath = false;

        public override void OnEnterWorld()
        {
            if (!CompWorld.DontDoThisMode) return;

            if (Main.rand.NextFloat() < 0.01f)
            {
                endlessDeath = true;
                Main.NewText($"{Player.name} brought a curse upon himself...", Color.Red);
            }
        }

        public override void PreUpdate()
        {
            if (!CompWorld.DontDoThisMode) return;

            if (endlessDeath)
            {
                if (!Player.dead)
                    Player.KillMe(PlayerDeathReason.ByCustomReason($"{Player.name} can't live."), 9999.0, 0, false);
                return;
            }

            if (Player.dead && !tombstonesSpawned)
            {
                SpawnTombstones();
                tombstonesSpawned = true;
            }

            if (!Player.dead && tombstonesSpawned)
                tombstonesSpawned = false;
        }

        public void SpawnTombstones()
        {
            float radius = 128f;
            for (int i = 0; i < 5; i++)
            {
                float angle = MathHelper.TwoPi * i / 5f + Main.rand.NextFloat(-0.2f, 0.2f);
                Vector2 offset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * Main.rand.NextFloat(64f, radius);
                Vector2 pos = Player.Center + offset;

                int projType = tombstoneProjectiles[Main.rand.Next(tombstoneProjectiles.Length)];

                Projectile.NewProjectile(
                    Player.GetSource_Death(),
                    pos,
                    Vector2.Zero,
                    projType,
                    0,
                    0f,
                    Player.whoAmI
                );
            }
        }
    }

    public class DontDoThisGlobalTile : GlobalTile
    {
        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (!CompWorld.DontDoThisMode) return;

            if (type == TileID.Tombstones)
            {
                Player player = Main.LocalPlayer;
                if (player.active && !player.dead)
                {
                    player.KillMe(PlayerDeathReason.ByCustomReason($"{player.name} потревожил покой мёртвых."), 9999.0, 0, false);
                    player.GetModPlayer<DontDoThisGlobalPlayer>().SpawnTombstones();
                }
            }
        }
    }
}
