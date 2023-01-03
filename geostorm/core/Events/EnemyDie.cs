using System;
using System.Numerics;

namespace geostorm.core.Events
{
    class EnemyDie : Event
    {
        public Enemy enemy;

        public EnemyDie(Enemy enemy)
        {
            this.enemy = enemy;
        }
    }
}
