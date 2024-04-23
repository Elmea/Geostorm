using System;
using System.Numerics;

namespace geostorm.core.Events
{
    class UseBomb : Event
    {
        public Vector2 position;

        public int frameCount = 0;

        public UseBomb(Vector2 position)
        {
            this.position = position;
        }
    }
}
