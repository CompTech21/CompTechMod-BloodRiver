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
        private static bool hardmodeActivated = false;

        public override void OnWorldLoad()
        {
            guardiansSpawnedOnJoin = false;
            hardmodeActivated = false;
        }

        public override void PostUpdateWorld()
        {
            if (!CompWorld.DontDoThisMode) return;

            if (!hardmodeActivated)
            {
                Main.hardMode = true;
                hardmodeActivated = true;
            }

            if (!Main.dayTime && Main.rand.NextFloat() < 0.15f)
                Main.bloodMoon = true;

            if (!guardiansSpawnedOnJoin)
            {
                guardiansSpawnedOnJoin = true;
                foreach (Player p in Main.player)
                    if (p.active)
                        SpawnGuardians(p, 6);
            }
        }

        public override void PostUpdatePlayers()
        {
            if (!CompWorld.DontDoThisMode) return;

            foreach (Player p in Main.player)
            {
                if (!p.active) continue;

                var mp = p.GetModPlayer<DontDoThisGlobalPlayer>();

                if (mp.endlessDeath)
                {
                    if (!p.dead)
                        p.KillMe(PlayerDeathReason.ByCustomReason($"{p.name} shouldn't have returned..."), 9999, 0);
                    continue;
                }

                p.pickSpeed *= 2f;

                if (!guardianSpawnCooldown.ContainsKey(p.whoAmI))
                    guardianSpawnCooldown[p.whoAmI] = 0;

                if (guardianSpawnCooldown[p.whoAmI] > 0)
                {
                    guardianSpawnCooldown[p.whoAmI]--;
                    continue;
                }

                if (Main.rand.NextFloat() < 0.05f)
                {
                    SpawnGuardians(p, 8);
                    guardianSpawnCooldown[p.whoAmI] = 720000;
                }
            }
        }

        private void SpawnGuardians(Player player, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Vector2 pos = player.Center + Main.rand.NextVector2Circular(400, 400);
                int npc = NPC.NewNPC(null, (int)pos.X, (int)pos.Y, NPCID.DungeonGuardian);
                if (npc >= 0)
                    Main.npc[npc].target = player.whoAmI;
            }
        }
    }

    public class DontDoThisGlobalNPC : GlobalNPC
    {
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (!CompWorld.DontDoThisMode) return;
            if (!npc.boss) return;

            int sameBossCount = 0;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC other = Main.npc[i];
                if (other.active && other.type == npc.type)
                    sameBossCount++;
            }

            if (sameBossCount >= 2)
                return;

            NPC.NewNPC(
                source,
                (int)npc.Center.X + Main.rand.Next(-120, 120),
                (int)npc.Center.Y,
                npc.type
            );

            foreach (Player p in Main.player)
            {
                if (!p.active) continue;

                int[] buffs =
                {
                    BuffID.Heartreach,
                    BuffID.Ironskin,
                    BuffID.NightOwl,
                    BuffID.Regeneration
                };

                for (int i = 0; i < 3; i++)
                    p.AddBuff(buffs[Main.rand.Next(buffs.Length)], 36000);
            }
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo info)
        {
            if (npc.boss)
                target.AddBuff(70, 600);
        }
    }

    public class DontDoThisGlobalPlayer : ModPlayer
    {
        private readonly int[] tombstones =
        {
            ProjectileID.Headstone,
            ProjectileID.CrossGraveMarker,
            ProjectileID.GraveMarker,
            ProjectileID.Obelisk
        };

        public bool tombstonesSpawned;
        public bool endlessDeath;

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
                    Player.KillMe(PlayerDeathReason.ByCustomReason($"{Player.name} can't live."), 9999, 0);
                return;
            }

            if (Player.dead && !tombstonesSpawned)
            {
                SpawnTombstones();
                tombstonesSpawned = true;
            }

            if (!Player.dead)
                tombstonesSpawned = false;
        }

        public void SpawnTombstones()
        {
            for (int i = 0; i < 3; i++)
            {
                Vector2 pos = Player.Center + Main.rand.NextVector2Circular(128, 128);
                Projectile.NewProjectile(
                    Player.GetSource_Death(),
                    pos,
                    Vector2.Zero,
                    tombstones[Main.rand.Next(tombstones.Length)],
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
                Player p = Main.LocalPlayer;
                if (p.active && !p.dead)
                {
                    p.KillMe(PlayerDeathReason.ByCustomReason($"{p.name} disturbed the peace of the dead."), 9999, 0);
                    p.GetModPlayer<DontDoThisGlobalPlayer>().SpawnTombstones();
                }
            }
        }
    }
}
