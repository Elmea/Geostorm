using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace geostorm.core
{
    class ColisionSystem : Isystem
    {
        public void Update(in GameInput input, GameData data, List<Event> events)
        {
            foreach (Enemy enemy in data.Enemies)
            {
                if (!enemy.IsSpawned())
                    continue;

                if (data.player.canTakeDamage())
                {
                    if (Entity.CheckColision(enemy, data.player))
                    {
                        data.player.life--;
                        enemy.isDead = true;
                        data.player.getHit();
                    }
                }

                foreach (Bullet bullet in data.Bullets)
                {
                    if (Entity.CheckColision(enemy, bullet))
                    {
                        enemy.life--;
                        bullet.isDead = true;

                        if (enemy.life <= 0)
                        {
                            enemy.isDead = true;
                            events.Add(new Events.EnemyKilled(enemy, bullet));
                            events.Add(new Events.BulletHit(bullet));
                        }
                    }
                }
            }

            foreach (Bullet bullet in data.Bullets)
            {
                if (bullet.position.X < 0 || bullet.position.X > input.screenSize.X || bullet.position.Y < 0 || bullet.position.Y > input.screenSize.Y)
                {
                    bullet.isDead = true;
                    events.Add(new Events.BulletHit(bullet));
                }
            }
        }
    }
}
