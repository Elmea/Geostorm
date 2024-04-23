using System;

namespace geostorm.core.Events
{
    class LevelUp : Event
    {
        public int level;
        public float timer;
        public LevelUp(int level)
        {
            this.level = level;
            timer = 2;
        }
    }
}
