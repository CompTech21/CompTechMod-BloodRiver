using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CompTechMod.Content.Buffs;
using CompTechMod.Content.Projectiles;
using Terraria.DataStructures;
using Terraria.GameContent;

namespace CompTechMod.Common.Players
{
    public class PyrotechnicPlayer : ModPlayer
    {
        public float Heat;
        private int noShootTimer;
        private bool shootingThisTick;
        public int explosionFlashTimer;

        public bool CanUsePyroWeapons => !Player.HasBuff(ModContent.BuffType<ColdBuff>());

        public override void ResetEffects()
        {
            shootingThisTick = false;
        }

        public void AddHeat(bool fullShot)
        {
            if (!fullShot || !CanUsePyroWeapons) return;

            shootingThisTick = true;
            noShootTimer = 0;

            Heat += 2.5f;
            if (Heat > 100f) Heat = 100f;

            if (Heat >= 100f && !Player.HasBuff(ModContent.BuffType<BoomBuff>()))
                Player.AddBuff(ModContent.BuffType<BoomBuff>(), 60 * 4);
        }

        public float GetDamageMultiplier()
        {
            float t = Heat / 100f;
            return 1f + t * 2f;
        }

        public override void PostUpdate()
        {
            if (!shootingThisTick) noShootTimer++;

            if (noShootTimer > 180 && Heat > 0f)
            {
                Heat -= 0.4f;
                if (Heat < 0f) Heat = 0f;
            }

            if (Player.HasBuff(ModContent.BuffType<BoomBuff>()) && shootingThisTick)
                TriggerExplosion();

            if (explosionFlashTimer > 0) explosionFlashTimer--;
        }

        private void TriggerExplosion()
        {
            if (Player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(
                    Player.GetSource_FromThis(),
                    Player.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<OverheatExplosion>(),
                    0,
                    0f,
                    Player.whoAmI
                );
            }

            int damage = Player.statLifeMax2 / 2;
            Player.Hurt(PlayerDeathReason.ByCustomReason($"{Player.name} exploded."), damage, 0);
            Player.velocity += new Vector2(0, -6f);
            Player.AddBuff(BuffID.OnFire, 60 * 2);

            explosionFlashTimer = 20;

            Player.ClearBuff(ModContent.BuffType<BoomBuff>());
            Player.AddBuff(ModContent.BuffType<ColdBuff>(), 60 * 10);

            Heat = 0f;
        }

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (item.DamageType == ModContent.GetInstance<DamageClasses.PyrotechnicDamageClass>())
                damage *= GetDamageMultiplier();
        }

        // Рисуем текст над игроком через PlayerDrawSet
        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
        {
            if (Player.HeldItem.DamageType != ModContent.GetInstance<DamageClasses.PyrotechnicDamageClass>() || Player.dead)
                return;

            string text = $"{(int)Heat}%";
            float t = Heat / 100f;
            Color color = Color.Lerp(Color.Blue, Color.Red, t);

            // Правильная позиция над игроком
            Vector2 worldPos = Player.Top + new Vector2(Player.width / 2, 75); // центр над головой
            Vector2 screenPos = worldPos - Main.screenPosition; // смещение на экран

            Utils.DrawBorderString(
                Main.spriteBatch,
                text,
                screenPos,
                color,
                1.2f,
                0.5f,
                0.5f
            );
        }
    }
}
