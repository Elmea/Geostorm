using System.Numerics;

namespace geostorm.core.Events
{
    class PlayerDie : Event
    {
        public Vector2 position;
        public Vector4 color;

        public PlayerDie(Vector2 position, Vector4 color)
        {
            this.position = position;
            this.color = color;
        }
    }
}
