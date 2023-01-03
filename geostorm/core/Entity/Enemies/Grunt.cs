using System;
using System.Numerics;
using System.Collections.Generic;

namespace geostorm.core
{
    class Grunt : Enemy
    {
        private float selfRotation;

        public Grunt(Vector2 position, float rotation, float spawnTimer, int life = 1)
    : base(position, rotation, spawnTimer, 50, 2, new Vector4(1, 0, 0, 1), life)
        {
            colisionRadius = 20;
        }

        public float getSelfRotation() { return selfRotation; }

        public override void Draw(Graphics graphics)
        {
                graphics.DrawGrunt(this);
        }

        protected override void DoUpdate(in GameInput input, GameData data, List<Event> events)
        {
            float newRotation = MathF.Atan2(data.player.position.Y - this.position.Y, data.player.position.X - this.position.X);

            position.X += (float)Math.Cos(rotation) * 5;
            position.Y += (float)Math.Sin(rotation) * 5;

            if (newRotation - rotation > Math.PI)
                rotation += (float)Math.PI * 2;
            if (newRotation - rotation < -Math.PI)
                rotation -= (float)Math.PI * 2;

            rotation = Calc.Lerp(0.5f, newRotation, rotation);

            selfRotation += input.deltaTime*2;
            if (selfRotation > 360)
                selfRotation = 0;

           
        }
    }
}
