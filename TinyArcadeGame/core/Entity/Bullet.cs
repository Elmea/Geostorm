using System;
using System.Collections.Generic;
using System.Numerics;

namespace geostorm.core
{
    class Bullet : Entity
    {
        private float speed;

        public Bullet(Vector2 _position, float _rotation)
            : base(_position, _rotation)
        {
            speed = 15;
            colisionRadius = 5;
        }

        public override void Update(in GameInput input, GameData data, List<Event> events)
        {
            position.X += (float)Math.Cos(rotation) * speed;
            position.Y += (float)Math.Sin(rotation) * speed;
        }

        public override void Draw(Graphics graphics)
        {
            graphics.DrawBullet(this);
        }
    }
}
