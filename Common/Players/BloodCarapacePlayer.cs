using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CompTechMod.Common.Keybinds;
using CompTechMod.Content.Buffs;

namespace CompTechMod.Common.Players
{
    public class BloodCarapacePlayer : ModPlayer
    {
        public bool hasCarapaceEquipped;

        public bool carapaceActive;
        private int carapaceTimer;

        private Vector2 frozenPosition;

        public override void ResetEffects()
        {
            hasCarapaceEquipped = false;
        }

        public override void ProcessTriggers(Terraria.GameInput.TriggersSet triggersSet)
        {
            if (!hasCarapaceEquipped)
                return;

            if (Player.HasBuff(ModContent.BuffType<BloodBarrierDebuff>()))
                return;

            if (CompTechKeybinds.BloodCarapaceKey.JustPressed)
            {
                if (!carapaceActive)
                    ActivateCarapace();
                else
                    DeactivateCarapace();
            }
        }

        private void ActivateCarapace()
        {
            carapaceActive = true;
            carapaceTimer = 20 * 60;

            frozenPosition = Player.Center;

            Player.velocity = Vector2.Zero;
            Player.gravDir = 0f;
            Player.fallStart = (int)(Player.position.Y / 16f);
        }

        private void DeactivateCarapace()
        {
            carapaceActive = false;
            carapaceTimer = 0;

            Player.gravDir = 1f;

            Player.AddBuff(ModContent.BuffType<BloodBarrierDebuff>(), 40 * 60);
        }

        // 🔒 БЛОК ВСЕГО УПРАВЛЕНИЯ (РАННИЙ ХУК)
        public override void PreUpdate()
        {
            if (!carapaceActive)
                return;

            // таймер
            carapaceTimer--;
            if (carapaceTimer <= 0)
            {
                DeactivateCarapace();
                return;
            }

            // ПОЛНЫЙ СТОП
            Player.velocity = Vector2.Zero;
            Player.position = frozenPosition - new Vector2(Player.width / 2f, Player.height / 2f);

            // ❌ ДВИЖЕНИЕ
            Player.controlLeft = false;
            Player.controlRight = false;
            Player.controlUp = false;
            Player.controlDown = false;
            Player.controlJump = false;
            Player.jump = 0;

            // ❌ АТАКИ И ПРЕДМЕТЫ
            Player.controlUseItem = false;
            Player.controlUseTile = false;
            Player.controlHook = false;
            Player.itemAnimation = 0;
            Player.itemTime = 0;
            Player.reuseDelay = 0;

            // ❌ ГРАВИТАЦИЯ И ФИЗИКА
            Player.gravity = 0f;
            Player.maxFallSpeed = 0f;

            // ✅ АБСОЛЮТНАЯ НЕУЯЗВИМОСТЬ
            Player.immune = true;
            Player.immuneTime = 2;

            // 🔴 КРАСНОЕ СВЕЧЕНИЕ (СУПЕР ЯРКО)
            Lighting.AddLight(Player.Center, 4.5f, 0f, 0f);

            // 🩸 ЭФФЕКТЫ
            if (Main.rand.NextBool(2))
            {
                Dust.NewDust(
                    Player.position,
                    Player.width,
                    Player.height,
                    DustID.Blood,
                    0f, 0f, 150, Color.DarkRed, 1.6f
                );
            }
        }

        // ❌ ПОЛНЫЙ БЛОК ИСПОЛЬЗОВАНИЯ ПРЕДМЕТОВ
        public override bool CanUseItem(Item item)
        {
            if (carapaceActive)
                return false;

            return base.CanUseItem(item);
        }

        // ❌ БЛОК ПОЛУЧЕНИЯ УРОНА (ДАЖЕ DOT)
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (carapaceActive)
            {
                modifiers.FinalDamage *= 0f;
            }
        }
    }
}
