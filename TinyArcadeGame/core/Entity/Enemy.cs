using System;
using System.Collections.Generic;
using System.Numerics;

namespace geostorm.core
{
    abstract class Enemy : Entity
    {
        public int life;
        protected float spawnTimer;
        public int pointYeld;
        public int moneyYeld;

        private Vector4 color;

        public void SetColor(float r, float g, float b, float a) { color = new Vector4(r, g, b, a); }
        public Vector4 GetColor() { return color; }

        public Enemy(Vector2 position, float rotation, float spawnTimer, int pointYeld, int moneyYeld, Vector4 color, int life = 1)
            : base(position, rotation)
        {
            this.spawnTimer = spawnTimer;
            this.life = life;
            this.pointYeld = pointYeld;
            this.moneyYeld = moneyYeld;
            this.color = color;
        }

        public void killedByBomb(List<Event> events)
        {
            isDead = true;
            events.Add(new Events.EnemyKilled(this, new Bullet(new Vector2(0, 0), 0)));
        }

        private void doWallColision(in GameInput input)
        {
            if (position.X < colisionRadius)
            {
                rotation = 0;
            }

            if (position.Y < colisionRadius)
            {
                rotation = Calc.DegreesToRad(90);
            }

            if (position.X > input.screenSize.X)
            {
                rotation = Calc.DegreesToRad(180);
            }

            if (position.Y > input.screenSize.Y)
            {
                rotation = Calc.DegreesToRad(270);
            }
        }

        public sealed override void Update(in GameInput input, GameData data, List<Event> events)
        {
            if (spawnTimer < 0)
            {
                DoUpdate(input, data, events);

                foreach (Enemy enemy in data.Enemies)
                {
                    if (enemy == this)
                        continue;

                    if (enemy.spawnTimer < 0)
                    {
                        if (Vector2.Distance(enemy.position, position) < 50)
                        {
                            float repulsionAngle = MathF.Atan2(position.Y - enemy.position.Y, position.X - enemy.position.X);

                            position.X += (float)Math.Cos(repulsionAngle);
                            position.Y += (float)Math.Sin(repulsionAngle);
                        }
                    }
                }

                doWallColision(input);
            }
            else
            {
                spawnTimer -= input.deltaTime;
            }
        }

       public bool IsSpawned()
        {
            return spawnTimer < 0;
        }

        protected abstract void DoUpdate(in GameInput input, GameData data, List<Event> events);
    }
}
