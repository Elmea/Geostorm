using System;
using System.Collections.Generic;
using System.Numerics;

namespace geostorm.core
{
    class Particle
    {
        public Vector2 position;
        public float rotation;
        public float speed;
        public float lifeTime;
        public int size;
        private Vector4 color;

        public Vector4 getColor() { return color; }

        public Particle(Vector2 position, float rotation, float speed, float lifeTime, int size, Vector4 color)
        {
            this.position = position;
            this.rotation = rotation;
            this.speed = speed;
            this.lifeTime = lifeTime;
            this.size = size;
            this.color = color;
        }
    }


    class ParticleSystem : Isystem, IGameEventListener
    {
        private List<Particle> particles;
        private Random random;

        private float ParticleOpacity = 0.75f;

        public ParticleSystem()
        {
            particles = new List<Particle>();
            random = new Random();
        }

        public void HandleEvent(GameData data, in GameInput input, List<Event> events)
        {
            foreach (Event curEvent in events)
            {
                switch (curEvent)
                {
                    case Events.EnemyKilled:
                        Events.EnemyKilled enemyKilled = curEvent as Events.EnemyKilled;

                        Vector4 colorToDisp = enemyKilled.enemy.GetColor();
                        colorToDisp.W = ParticleOpacity;
                        if (enemyKilled.frameCount == 0)
                        {
                            for (int i = 0; i < 64; i++)
                                particles.Add(new Particle(enemyKilled.enemy.position, i * MathF.PI / 32, random.Next(1, 9) / 2.5f, random.Next(0, 3), random.Next(5, 8), colorToDisp));
                        }
                        break;

                    case Events.EnemyDie:
                        Events.EnemyDie enemyDie = curEvent as Events.EnemyDie;
                        Vector4 colorTodisp = enemyDie.enemy.GetColor();
                        colorTodisp.W = ParticleOpacity;
                        for (int i = 0; i < 64; i++)
                            particles.Add(new Particle(enemyDie.enemy.position, i * MathF.PI / 32, random.Next(1, 9) / 2.5f, random.Next(0, 3), random.Next(5, 8), colorTodisp));
                        enemyDie.canBeDeleted = true;
                        break;

                    case Events.BulletHit:
                        Events.BulletHit bulletHitWall = curEvent as Events.BulletHit;

                        for (int i = 0; i < 64; i++)
                            particles.Add(new Particle(bulletHitWall.bullet.position, i * MathF.PI / 32, random.Next(1, 3), random.Next(0, 2), random.Next(3, 8), new Vector4(1, 1, 0, ParticleOpacity)));

                        bulletHitWall.canBeDeleted = true;
                        break;

                    case Events.UseBomb:
                        Events.UseBomb useBomb = curEvent as Events.UseBomb;

                        if (useBomb.frameCount < 1) 
                            for (int i = 0; i < 256; i++)
                                particles.Add(new Particle(useBomb.position, i * MathF.PI / 64, random.Next(1, 8), random.Next(0, 2), random.Next(5, 8), new Vector4(212f/255f, 151f/255f, 19f/255f, ParticleOpacity)));
                        break;

                    case Events.PlayerDie:
                        Events.PlayerDie playerDie = curEvent as Events.PlayerDie;

                        for (int i = 0; i < 128; i++)
                            particles.Add(new Particle(playerDie.position, i * MathF.PI / 32, random.Next(1, 8), random.Next(0, 2), random.Next(5, 8), playerDie.color));
                        playerDie.canBeDeleted = true;
                        break;

                    case Events.BlackHoleExplode:
                        Events.BlackHoleExplode blackHoleExplode = curEvent as Events.BlackHoleExplode;

                        if (blackHoleExplode.frameCount == 0)
                            for (int i = 0; i < 128; i++)
                                particles.Add(new Particle(blackHoleExplode.blackHole.position, i * MathF.PI / 32, random.Next(1, 8), random.Next(0, 2), random.Next(5, 8), new Vector4(203f/255f, 33f/255f, 255f/255f, ParticleOpacity)));
                        break;
                }
            }
        }

        public void Update(in GameInput input, GameData data, List<Event> events)
        {
            foreach (Particle particle in particles)
            {
                particle.position.X += (float)Math.Cos(particle.rotation) * particle.speed;
                particle.position.Y += (float)Math.Sin(particle.rotation) * particle.speed;

                particle.lifeTime -= input.deltaTime;

                if (particle.position.X < -10 || particle.position.Y < -10 || particle.position.X > input.screenSize.X + 10 || particle.position.Y > input.screenSize.Y + 10)
                    particle.lifeTime = -1;
            }

            particles.RemoveAll(particle => particle.lifeTime < 0);
        }

        public void drawParticles(Graphics graphics)
        {
            foreach (Particle particle in particles)
            {
                graphics.DrawParticle(particle);
            }
        }
    }
}
