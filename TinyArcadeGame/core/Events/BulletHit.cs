using System;

namespace geostorm.core.Events
{
    class BulletHit : Event
    {
        public Bullet bullet;

        public BulletHit(Bullet bullet)
        {
            this.bullet = bullet;
        }
    }
}
