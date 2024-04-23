using System;

namespace geostorm.core.Events
{
    class EnemyKilled : Event
    {
        public Enemy enemy;
        public Bullet bullet;
        public int frameCount;

        public EnemyKilled(Enemy enemy, Bullet bullet)
        {
            this.enemy = enemy;
            this.bullet = bullet;
        }
    }
}
