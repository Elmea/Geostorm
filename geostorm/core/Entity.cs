using System;
using System.Numerics;
using System.Collections.Generic;

namespace geostorm.core
{
    class Entity
    {
        public Vector2 position;
        public float rotation; //Rotation in RAD
        public float colisionRadius;
        public bool isDead;

        public Entity(Vector2 position, float rotation)
        {
            this.position = position;
            this.rotation = rotation;
            this.isDead = false;
        }

        static public bool CheckColision(Entity entity1, Entity entity2)
        {
            return Vector2.Distance(entity1.position, entity2.position) < entity1.colisionRadius + entity2.colisionRadius;  
        }
        public virtual void Update(in GameInput input, GameData data, List<Event> events) { }
        public virtual void Draw(Graphics graphics) { }
    }
}
