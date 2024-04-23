using System;
using System.Numerics;
using System.Collections.Generic;

namespace geostorm.core
{
    class BlackHole : Entity
    {
        private float lifeTimer;
        private float spawnTimer;

        private int range = 200;
        private int explosionRange = 300;

        public float getLifeTimer() { return lifeTimer; }

        public int getExplosionRange() { return explosionRange; }


        public bool IsSpawned() { return spawnTimer < 0; }

        public BlackHole(Vector2 _position, float _rotation, float lifeTimer)
            : base(_position, _rotation)
        {
            this.lifeTimer = lifeTimer;
            spawnTimer = 5;
        }

        public override void Draw(Graphics graphics)
        {
            graphics.DrawBlackHole(this);
        }

        private void Attract(in GameInput input, GameData data, List<Event> events)
        {
            foreach (Entity entity in data.Entities)
            {
                if (entity == this)
                    continue;

                switch (entity)
                {
                    case Enemy:
                        Enemy enemy = entity as Enemy;
                        if (!enemy.IsSpawned())
                            break;

                        if (Vector2.Distance(this.position, enemy.position) < 10)
                        {
                            enemy.isDead = true;
                            lifeTimer -= 0.5f;
                            events.Add(new Events.EnemyDie(enemy));
                        }
                        if (Vector2.Distance(this.position, enemy.position) < range)
                        {
                            float angle = MathF.Atan2(entity.position.Y - position.Y, entity.position.X - position.X);
                            enemy.position.X -= (float)Math.Cos(angle) * (10 * Vector2.Distance(this.position, enemy.position) / range);
                            enemy.position.Y -= (float)Math.Sin(angle) * (10 * Vector2.Distance(this.position, enemy.position) / range);
                        }
                        break;

                    case Player:
                        Player player = entity as Player;
                        if (Vector2.Distance(this.position, player.position) < 10)
                        {
                            player.life = 0;
                        }
                        if (Vector2.Distance(this.position, player.position) < range)
                        {
                            float angle = MathF.Atan2(player.position.Y - position.Y, player.position.X - position.X);
                            player.position.X -= (float)Math.Cos(angle) * (10 * Vector2.Distance(this.position, player.position) / range);
                            player.position.Y -= (float)Math.Sin(angle) * (10 * Vector2.Distance(this.position, player.position) / range);
                        }
                        break;

                    case Bullet:
                        Bullet bullet = entity as Bullet;
                        if (Vector2.Distance(this.position, bullet.position) < 10)
                        {
                            bullet.isDead = true;
                            lifeTimer -= 0.5f;
                            events.Add(new Events.BulletHit(bullet));
                        }
                        if (Vector2.Distance(this.position, bullet.position) < range)
                        {
                            float angle = MathF.Atan2(bullet.position.Y - position.Y, bullet.position.X - position.X);
                            bullet.position.X -= (float)Math.Cos(angle) * (10 * Vector2.Distance(this.position, bullet.position) / range);
                            bullet.position.Y -= (float)Math.Sin(angle) * (10 * Vector2.Distance(this.position, bullet.position) / range);

                        }
                        break;

                    default:
                        break;
                }
            }
        }

        private void Explode(in GameInput input, GameData data, List<Event> events)
        {
            isDead = true;
            events.Add(new Events.BlackHoleExplode(this));
            foreach (Entity entity in data.Entities)
            {
                if (entity == this)
                    continue;

                switch (entity)
                {
                    case Enemy:
                        Enemy enemy = entity as Enemy;
                        if (!enemy.IsSpawned())
                            break;

                        if (Vector2.Distance(this.position, enemy.position) < explosionRange && enemy.IsSpawned())
                        {
                            enemy.isDead = true;
                            events.Add(new Events.EnemyDie(enemy));
                        }
                        break;

                    case Player:
                        Player player = entity as Player;
                        if (Vector2.Distance(player.position, this.position) < explosionRange)
                        {
                            if (player.canTakeDamage())
                            {
                                player.life--;
                                player.getHit();
                            }
                        }
                        break;

                    case Bullet:
                        Bullet bullet = entity as Bullet;
                        if (Vector2.Distance(this.position, bullet.position) < explosionRange)
                        {
                            bullet.isDead = true;
                            events.Add(new Events.BulletHit(bullet));
                        }
                        break;

                    case BlackHole:
                        BlackHole blackHole = entity as BlackHole;
                        if (Vector2.Distance(this.position, blackHole.position) < explosionRange && blackHole.IsSpawned())
                        {
                            if (!blackHole.isDead)  blackHole.Explode(input, data, events);
                        }
                        break;


                    default:
                        break;
                }
            }
        }

        public override void Update(in GameInput input, GameData data, List<Event> events)
        {
            if (spawnTimer < 0)
            {
                Attract(input, data, events);

                lifeTimer -= input.deltaTime;
                if (lifeTimer < 0)
                {
                    Explode(input, data, events);
                }
            }
            else
            {
                spawnTimer -= input.deltaTime;
            }
        }
    }
}
