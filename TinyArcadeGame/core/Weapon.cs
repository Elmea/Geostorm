using System;
using System.Numerics;
using System.Collections.Generic;

namespace geostorm.core
{
    class Weapon
    {
        public static int levelMax = 6;

        private int level;

        private float frequency;
        private float timer;
        private float shootAngle;

        public float getShootAngle() { return shootAngle; }

        public Weapon()
        {
            level = 1;
            timer = 0;
            frequency = 0.20f;
        }

        public void levelUp() { level++; }
        public int getLevel() { return level; }

        private void level1Shoot(in GameInput input, GameData data, List<Event> events, in Player owner) 
        {
            if (timer > frequency)
            {
                Bullet bullet = new Bullet(Calc.PlanRotation(new Vector2(25, 0), shootAngle) + owner.position, shootAngle);
                data.AddBulletDelayed(bullet);
                events.Add(new Events.Shoot(bullet));
                timer = 0;
            }
        }

        private void level2Shoot(in GameInput input, GameData data, List<Event> events, in Player owner)
        {
            if (timer > frequency)
            {
                Bullet bullet = new Bullet(Calc.PlanRotation(new Vector2(25, 0), shootAngle) + owner.position, shootAngle);
                data.AddBulletDelayed(bullet);
                events.Add(new Events.Shoot(bullet));
                timer = 0;
            }
        }

        private void level3Shoot(in GameInput input, GameData data, List<Event> events, in Player owner)
        {
            if (timer > frequency)
            {
                Bullet bullet = new Bullet(Calc.PlanRotation(new Vector2(25, 0), shootAngle) + owner.position, shootAngle);
                data.AddBulletDelayed(bullet);
                events.Add(new Events.Shoot(bullet));
                Bullet bulletLeft = new Bullet(Calc.PlanRotation(new Vector2(25, 0), shootAngle) + owner.position, shootAngle + MathF.PI / 24f);
                data.AddBulletDelayed(bulletLeft);
                events.Add(new Events.Shoot(bulletLeft));
                Bullet bulletRight = new Bullet(Calc.PlanRotation(new Vector2(25, 0), shootAngle) + owner.position, shootAngle - MathF.PI / 24f);
                data.AddBulletDelayed(bulletRight);
                events.Add(new Events.Shoot(bulletRight));

                timer = 0;
            }
        }

        private void level5Shoot(in GameInput input, GameData data, List<Event> events, in Player owner)
        {
            if (timer > frequency)
            {
                Bullet bullet = new Bullet(Calc.PlanRotation(new Vector2(25, 0), shootAngle) + owner.position, shootAngle);
                data.AddBulletDelayed(bullet);
                events.Add(new Events.Shoot(bullet));

                Bullet bullet1 = new Bullet(Calc.PlanRotation(new Vector2(25, 0), shootAngle) + owner.position, shootAngle + MathF.PI / 12f);
                data.AddBulletDelayed(bullet1);
                events.Add(new Events.Shoot(bullet1));
                Bullet bullet2 = new Bullet(Calc.PlanRotation(new Vector2(25, 0), shootAngle) + owner.position, shootAngle - MathF.PI / 12f);
                events.Add(new Events.Shoot(bullet2));
                data.AddBulletDelayed(bullet2);

                Bullet bulletLeft = new Bullet(Calc.PlanRotation(new Vector2(25, 0), shootAngle) + owner.position, shootAngle + MathF.PI / 24f);
                data.AddBulletDelayed(bulletLeft);
                events.Add(new Events.Shoot(bulletLeft));
                Bullet bulletRight = new Bullet(Calc.PlanRotation(new Vector2(25, 0), shootAngle) + owner.position, shootAngle - MathF.PI / 24f);
                data.AddBulletDelayed(bulletRight);

                events.Add(new Events.Shoot(bulletRight));

                timer = 0;
            }
        }


        public void Update(in GameInput input, GameData data, List<Event> events, in Player owner)
        {
            if (input.playerInputs.isGamepadAvailable)
            {
                shootAngle = MathF.Atan2(input.playerInputs.getShootAxis().Y, input.playerInputs.getShootAxis().X);
            }
            else
            {
                shootAngle = MathF.Atan2(input.playerInputs.getTarget().Y - owner.position.Y, input.playerInputs.getTarget().X - owner.position.X);
            }

            if (input.playerInputs.shoot)
            {
                switch (level)
                {
                    case 1:
                        frequency = 0.20f;
                        level1Shoot(input, data, events, owner);
                        break;

                    case 2:
                        frequency = 0.10f;
                        level2Shoot(input, data, events, owner);
                        break;

                    case 3:
                        frequency = 0.15f;
                        level3Shoot(input, data, events, owner);
                        break;

                    case 4:
                        frequency = 0.10f;
                        level3Shoot(input, data, events, owner);
                        break;

                    case 5:
                        frequency = 0.15f;
                        level5Shoot(input, data, events, owner);
                        break;

                    case 6:
                        frequency = 0.10f;
                        level5Shoot(input, data, events, owner);
                        break;

                    default:
                        break;
                }
            }

            timer += input.deltaTime;
        }
    }
}
