using System;

namespace geostorm.core.Events
{
    class Shoot : Event
    {
        public Bullet bullet;

        public Shoot(Bullet bullet)
        {
            this.bullet = bullet;
            canBeDeleted = true;
        }
    }
}
