using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace geostorm.core
{
    class EnemySpawnSystem : Isystem
    {
        private float timer;
        private Random random;

        public EnemySpawnSystem() 
        { 
            timer = 2;
            random = new Random();
        }

        public void Update(in GameInput input , GameData data, List<Event> events)
        {
            if (!data.player.isDead)
            {
                int curLevel = data.player.getLevel();
                timer -= input.deltaTime;
                if (timer <= 0)
                {
                    if (curLevel < 10)
                    {
                        int randCount = random.Next(0, (int)((curLevel + 1) * 1.5f));

                        for (int i = 0; i < randCount; i++)
                        {
                            int randEnemy = random.Next(0);
                            Enemy enemy;
                            switch (randEnemy)
                            {
                                case 0:
                                    enemy = new Grunt(new Vector2(random.Next((int)input.screenSize.X), random.Next((int)input.screenSize.Y)), 0, random.Next(1, 5));
                                    break;

                                default:
                                    enemy = new Grunt(new Vector2(random.Next((int)input.screenSize.X), random.Next((int)input.screenSize.Y)), 0, random.Next(1, 5));
                                    break;
                            }

                            data.AddEnemyDelayed(enemy);
                        }

                        if (random.Next(15 - curLevel) == 0)
                        {
                            data.AddBlackHoleDelayed(new BlackHole(new Vector2(random.Next((int)input.screenSize.X), random.Next((int)input.screenSize.Y)), 0, random.Next(10, 15)));
                        
                        }
                    }
                    else
                    {
                        int randCount = random.Next(0, 12);

                        for (int i = 0; i < randCount; i++)
                        {
                            int randEnemy = random.Next(0);
                            Enemy enemy;
                            switch (randEnemy)
                            {
                                case 0:
                                    enemy = new Grunt(new Vector2(random.Next((int)input.screenSize.X), random.Next((int)input.screenSize.Y)), 0, random.Next(1, 5));
                                    break;

                                default:
                                    enemy = new Grunt(new Vector2(random.Next((int)input.screenSize.X), random.Next((int)input.screenSize.Y)), 0, random.Next(1, 5));
                                    break;
                            }

                            data.AddEnemyDelayed(enemy);
                        }
                        if (random.Next(3) == 0)
                        {
                            data.AddBlackHoleDelayed(new BlackHole(new Vector2(random.Next((int)input.screenSize.X), random.Next((int)input.screenSize.Y)), 0, random.Next(10, 15)));
                        }
                    }
                    timer = 2;
                }
            }
        }
    }
}
