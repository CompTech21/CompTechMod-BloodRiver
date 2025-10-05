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

            // Ночь чуть быстрее
            if (!Main.dayTime)
            {
                Main.bloodMoon = true;
                Main.time += 0.7;
            }

            // Гарантированный спавн Dungeon Guardian при входе
            if (!guardiansSpawnedOnJoin)
            {
                guardiansSpawnedOnJoin = true;
                foreach (Player player in Main.player)
                    if (player.active)
                        SpawnGuardians(player, 6);
            }
        }

        public override void PostUpdatePlayers()
        {
            if (!CompWorld.DontDoThisMode) return;

            foreach (Player player in Main.player)
            {
                if (!player.active) continue;

                player.statDefense *= 0.5f;
                player.GetDamage(DamageClass.Generic) *= 0.5f;
                player.moveSpeed *= 0.5f;
                player.pickSpeed *= 0.2f;
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

    public class DontDoThisItemGlobal : GlobalItem
    {
        public override bool CanUseItem(Item item, Player player)
        {
            if (!CompWorld.DontDoThisMode) return true;

            if (item.DamageType != DamageClass.Default || item.pick > 0 || item.axe > 0 || item.hammer > 0)
                if (Main.rand.NextFloat() < 0.02f)
                {
                    item.TurnToAir();
                    return false;
                }

            return true;
        }
    }

    public class DontDoThisGlobalNPC : GlobalNPC
    {
        public override bool AppliesToEntity(NPC npc, bool lateInstantiation) => npc.townNPC;

        public override void AI(NPC npc)
        {
            if (!CompWorld.DontDoThisMode) return;
            npc.friendly = false;
            npc.target = npc.FindClosestPlayer();
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

        private bool tombstonesSpawned = false; // новый флаг

        public override void PreUpdate()
        {
            if (!CompWorld.DontDoThisMode) return;

            if (Player.dead && !tombstonesSpawned)
            {
                SpawnTombstones();
                tombstonesSpawned = true; // помечаем, что уже спавнили
            }

            // Сбрасываем флаг, когда игрок возрождается
            if (!Player.dead && tombstonesSpawned)
            {
                tombstonesSpawned = false;
            }
        }

        public void SpawnTombstones()
        {
            float radius = 128f; // максимальное смещение от игрока
            for (int i = 0; i < 5; i++)
            {
                // равномерное распределение по кругу
                float angle = MathHelper.TwoPi * i / 5f + Main.rand.NextFloat(-0.2f, 0.2f);
                Vector2 offset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * (Main.rand.NextFloat(64f, radius));
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
                    player.KillMe(PlayerDeathReason.ByCustomReason(""), 9999.0, 0, false);
                    player.GetModPlayer<DontDoThisGlobalPlayer>().SpawnTombstones();
                }
            }
        }
    }
}
