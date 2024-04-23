using System;
using System.Numerics;
using System.Collections.Generic;

namespace geostorm.core
{
    class GameData : IGameEventListener
    {
        // ------------------------------------------ [List] -------------------------------------------
        private List<Entity> entities;
        public Player player;
        private List<Enemy> enemies;
        private List<Bullet> bullets;
        private List<BlackHole> blackHoles;

        public IEnumerable<Entity> Entities { get { return entities; } }
        public IEnumerable<Enemy> Enemies { get { return enemies; } }
        public IEnumerable<Bullet> Bullets { get { return bullets; }  }
        public IEnumerable<BlackHole> BlackHoles { get { return blackHoles; } }

        // ---------------------------------------- [Temp list] ----------------------------------------
        private List<Enemy> EnemyAdded;
        private List<Bullet> BulletAdded;
        private List<BlackHole> BlackHoleAdded;

        public void AddEnemyDelayed(Enemy enemy)
        {
            EnemyAdded.Add(enemy);
        }

        public void AddBulletDelayed(Bullet bullet)
        {
            BulletAdded.Add(bullet);
        }

        public void AddBlackHoleDelayed(BlackHole blackHole)
        {
            BlackHoleAdded.Add(blackHole);
        }

        public void addPlayer(Player player)
        {
            entities.Add(player);
            this.player = player;
        }

        public void update()
        {
            foreach (Enemy enemy in EnemyAdded)
            {
                enemies.Add(enemy);
                entities.Add(enemy);
            }
            EnemyAdded.Clear();

            foreach (Bullet bullet in BulletAdded)
            {
                bullets.Add(bullet);
                entities.Add(bullet);
            }
            BulletAdded.Clear();

            foreach (BlackHole blackHole in BlackHoleAdded)
            {
                blackHoles.Add(blackHole);
                entities.Add(blackHole);
            }
            BlackHoleAdded.Clear();

            // Delete dead entity
            entities.RemoveAll(entity => entity.isDead);
            enemies.RemoveAll(entity => entity.isDead);
            bullets.RemoveAll(entity => entity.isDead);
            blackHoles.RemoveAll(entity => entity.isDead);
        }

        public GameData()
        {
            entities = new List<Entity>();
            enemies = new List<Enemy>();
            bullets = new List<Bullet>();
            blackHoles = new List<BlackHole>();

            EnemyAdded = new List<Enemy>();
            BulletAdded = new List<Bullet>();
            BlackHoleAdded = new List<BlackHole>();
        }

        public void HandleEvent(GameData data, in GameInput input, List<Event> events)
        {
            foreach (Event curEvent in events)
            {
                switch (curEvent)
                {
                    case Events.EnemyKilled:
                        Events.EnemyKilled enemyKilled = curEvent as Events.EnemyKilled;
                        if (enemyKilled.frameCount < 1)
                        {
                            player.score += enemyKilled.enemy.pointYeld;
                            player.money += enemyKilled.enemy.moneyYeld;
                        }
                        break;

                    case core.Events.GameOver:
                        data.player.money = 0;
                        if (input.playerInputs.restart)
                        {
                            core.Events.GameOver gameOver = curEvent as core.Events.GameOver;
                            entities.Clear();
                            enemies.Clear();
                            bullets.Clear();
                            blackHoles.Clear();
                            Player newPlayer = new Player(new Vector2(input.screenSize.X / 2, input.screenSize.Y / 2), 5);
                            data.addPlayer(newPlayer);
                            gameOver.canBeDeleted = true;
                        }
                        break;
                }
            }
        }
    }
}
