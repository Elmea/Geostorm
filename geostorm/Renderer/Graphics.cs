using System;
using System.Numerics;
using Raylib_cs;
using System.Collections.Generic;

namespace geostorm
{
    struct PlayerVertice
    {
        public float preScale;
        public Vector2[] vertices;
    }

    struct WeaponVertice
    {
        public float preScale;
        public Vector2[] vertices;
    }

    struct BulletVertice
    {
        public float preScale;
        public Vector2[] vertices;
    }

    struct GruntVertice
    {
        public float preScale;
        public Vector2[] vertices;
    }
    struct LifeVertice
    {
        public float preScale;
        public Vector2[] vertices;
    }
    class Graphics : IGameEventListener
    {
        private PlayerVertice playerVertice = new PlayerVertice();
        private WeaponVertice weaponVertice = new WeaponVertice();
        private BulletVertice bulletVertice = new BulletVertice();
        private GruntVertice gruntVertice = new GruntVertice();
        private LifeVertice lifeVertice = new LifeVertice();

        private void LoadPlayer()
        {
            playerVertice.vertices = new Vector2[8];

            playerVertice.preScale = 20.0f;
            playerVertice.vertices[0] = new Vector2(-0.5f, 0.0f) * playerVertice.preScale; //IC
            playerVertice.vertices[1] = new Vector2(-0.2f, -0.55f) * playerVertice.preScale; //TIW
            playerVertice.vertices[2] = new Vector2(0.6f, -0.3f) * playerVertice.preScale; //TG
            playerVertice.vertices[3] = new Vector2(-0.4f, -0.8f) * playerVertice.preScale; //TOW
            playerVertice.vertices[4] = new Vector2(-1.0f, 0.0f) * playerVertice.preScale; //OC
            playerVertice.vertices[5] = new Vector2(playerVertice.vertices[3].X, -playerVertice.vertices[3].Y); //BOW
            playerVertice.vertices[6] = new Vector2(playerVertice.vertices[2].X, -playerVertice.vertices[2].Y); //B
            playerVertice.vertices[7] = new Vector2(playerVertice.vertices[1].X, -playerVertice.vertices[1].Y); //BIW
        }

        private void LoadWeapon()
        {
            weaponVertice.vertices = new Vector2[8];

            weaponVertice.preScale = 30.0f;
            weaponVertice.vertices[0] = Calc.PlanRotation(new Vector2(-0.25f, 0.0f) * playerVertice.preScale, Calc.DegreesToRad(90)); //IC
            weaponVertice.vertices[1] = Calc.PlanRotation(new Vector2(0.25f, 0.0f) * playerVertice.preScale, Calc.DegreesToRad(90)); //TIW
            weaponVertice.vertices[2] = Calc.PlanRotation(new Vector2(0.0f, -0.25f) * playerVertice.preScale, Calc.DegreesToRad(90)); //TG

            float weaponDistance = 20;
            weaponVertice.vertices[0].X += weaponDistance;
            weaponVertice.vertices[1].X += weaponDistance;
            weaponVertice.vertices[2].X += weaponDistance;
        }

        private void LoadBullet()
        {
            bulletVertice.vertices = new Vector2[4];

            bulletVertice.preScale = 15.0f;
            bulletVertice.vertices[0] = new Vector2(-0.3f, 0.0f) * bulletVertice.preScale; // Left
            bulletVertice.vertices[1] = new Vector2(-0.1f, 0.2f) * bulletVertice.preScale; // Top 
            bulletVertice.vertices[2] = new Vector2(0.8f, 0.0f) * bulletVertice.preScale; // Right
            bulletVertice.vertices[3] = new Vector2(bulletVertice.vertices[1].X, -bulletVertice.vertices[1].Y); // Bottom
        }

        private void LoadGrunt()
        {
            gruntVertice.vertices = new Vector2[4];

            gruntVertice.preScale = 18.0f;
            gruntVertice.vertices[0] = new Vector2(-1.0f, 0.0f) * gruntVertice.preScale; // Left
            gruntVertice.vertices[1] = new Vector2(-0.0f, -1.0f) * gruntVertice.preScale; // Top
            gruntVertice.vertices[2] = new Vector2(1.0f, 0.0f) * gruntVertice.preScale; // Right
            gruntVertice.vertices[3] = new Vector2(-0.0f, 1.0f) * gruntVertice.preScale; // Bottom
        }


        private void LoadLife()
        {
            lifeVertice.vertices = new Vector2[10];

            lifeVertice.preScale = 5.0f;
            lifeVertice.vertices[0] = new Vector2(0.0f, 3.0f) * lifeVertice.preScale; 
            lifeVertice.vertices[1] = new Vector2(-3.0f, -1.0f) * lifeVertice.preScale; 
            lifeVertice.vertices[2] = new Vector2(-3.0f, -2.0f) * lifeVertice.preScale; 
            lifeVertice.vertices[3] = new Vector2(-2.0f, -3.0f) * lifeVertice.preScale;
            lifeVertice.vertices[4] = new Vector2(-1.0f, -3.0f) * lifeVertice.preScale;
            lifeVertice.vertices[5] = new Vector2(0.0f, -2.0f) * lifeVertice.preScale;
            lifeVertice.vertices[6] = new Vector2(1.0f, -3.0f) * lifeVertice.preScale;
            lifeVertice.vertices[7] = new Vector2(2.0f, -3.0f) * lifeVertice.preScale;
            lifeVertice.vertices[8] = new Vector2(3.0f, -2.0f) * lifeVertice.preScale;
            lifeVertice.vertices[9] = new Vector2(3.0f, -1.0f) * lifeVertice.preScale;

        }


        public void drawPlayerUI (core.Player player, Vector2 position)
        {
            Vector4 color = player.GetColor();
            color = color * 255;
            Color colorToDisp = new Color((byte)color.X, (byte)color.Y, (byte)color.Z, (byte)color.W);
            colorToDisp.a = 255;

            Raylib.DrawText($"Player level {player.getLevel()}", (int)position.X, (int)position.Y, 50, colorToDisp);

            Raylib.DrawText($"weapon level {player.weapon.getLevel()}", (int)position.X, (int)position.Y + 55, 30, colorToDisp);

            Raylib.DrawText($"Score {player.score}", (int)position.X, (int)position.Y + 90, 20, colorToDisp);

            Raylib.DrawText($"$$ {player.money}", (int)position.X, (int)position.Y + 115, 20, colorToDisp);

            for (int offset = 0; offset < player.life; offset++)
            {
                Vector2 drawPosition = new Vector2((int)position.X + 400 + 50 * offset, (int)position.Y + 20);
                for (int i = 0; i < 10; i++)
                {
                    Raylib.DrawLineEx(lifeVertice.vertices[i] + drawPosition, lifeVertice.vertices[(i + 1) % 10] + drawPosition, 1, colorToDisp);
                }
            }
        }

        public void DrawPlayer(core.Player player)
        {
            Vector4 color = player.GetColor();
            color = color * 255;
            Color colorToDisp = new Color((byte)color.X, (byte)color.Y, (byte)color.Z, (byte)color.W);

            for (int i = 0; i < 8; i++)
            {
                Raylib.DrawLineEx(Calc.PlanRotation(playerVertice.vertices[i], player.rotation) + player.position, Calc.PlanRotation(playerVertice.vertices[(i + 1) % 8], player.rotation) + player.position, 1, colorToDisp);
            }

            float shootAngle = player.weapon.getShootAngle();

            for (int i = 0; i < 3; i++)
            {
                Raylib.DrawLineEx(Calc.PlanRotation(weaponVertice.vertices[i], shootAngle) + player.position, Calc.PlanRotation(weaponVertice.vertices[(i + 1) % 3], shootAngle) + player.position, 1, colorToDisp);
            }
        }

        public void DrawBullet(core.Bullet bullet)
        {
            for (int i = 0; i < 4; i++)
            {
                Raylib.DrawLineEx(Calc.PlanRotation(bulletVertice.vertices[i], bullet.rotation) + bullet.position, Calc.PlanRotation(bulletVertice.vertices[(i + 1) % 4], bullet.rotation) + bullet.position, 1, Color.YELLOW);
            }
        }

        public void DrawGrunt(core.Grunt grunt)
        {
            Vector4 color = grunt.GetColor();
            color = color * 255;
            Color colorToDisp = new Color((byte)color.X, (byte)color.Y, (byte)color.Z, (byte)color.W);

            for (int i = 0; i < 4; i++)
            {
                Raylib.DrawLineEx(Calc.PlanRotation(gruntVertice.vertices[i], grunt.getSelfRotation()) + grunt.position, Calc.PlanRotation(gruntVertice.vertices[(i + 1) % 4], grunt.getSelfRotation()) + grunt.position, 1, grunt.IsSpawned() ? colorToDisp : Raylib.ColorAlpha(colorToDisp, 0.5f));
            }
        }

        public void DrawMarket(core.Market market)
        {
            if (market.gamepadAvailable)
            {
                Raylib.DrawText($"Buy bomb (LB) : { market.getBombPrice()} $", 300, 55, 30, market.bombBuyable ? new Color(109, 222, 78, 200) : new Color(237, 38, 38, 200));

                Raylib.DrawText($"Buy life (X) : { market.getLifePrice()} $", 300, 85, 30, market.lifeBuyable ? new Color(109, 222, 78, 200) : new Color(237, 38, 38, 200));

                if (!market.isLevelMax)
                    Raylib.DrawText($"Buy upgrade (RB) : { market.GetUpgradePrice()} $", 300, 115, 30, market.upgradeBuyable ? new Color(109, 222, 78, 200) : new Color(237, 38, 38, 200));

            }
            else
            {
                Raylib.DrawText($"Buy bomb (Q) : { market.getBombPrice()} $", 300, 55, 30, market.bombBuyable ? new Color(109, 222, 78, 200) : new Color(237, 38, 38, 200));

                Raylib.DrawText($"Buy life (F) : { market.getLifePrice()} $", 300, 85, 30, market.lifeBuyable ? new Color(109, 222, 78, 200) : new Color(237, 38, 38, 200));

                if (!market.isLevelMax)
                    Raylib.DrawText($"Buy upgrade (E) : { market.GetUpgradePrice()} $", 300, 115, 30, market.upgradeBuyable ? new Color(109, 222, 78, 200) : new Color(237, 38, 38, 200));

            }
        }

        public void DrawBlackHole(core.BlackHole blackHole)
        {
            Raylib.DrawCircleLines((int)blackHole.position.X, (int)blackHole.position.Y, 20, blackHole.IsSpawned() ? new Color(131, 59, 209, 255) : new Color(131, 59, 209, 100));
            if (blackHole.IsSpawned())
                Raylib.DrawCircleLines((int)blackHole.position.X, (int)blackHole.position.Y, Calc.Lerp(blackHole.getLifeTimer()/5f, 20, 0), new Color(131, 59, 209, 255));
        }

        public void DrawParticle(core.Particle particle)
        {
            Vector2 endpoint = Calc.PlanRotation(new Vector2(particle.size * particle.speed, 0), particle.rotation) + particle.position;

            Vector4 color = particle.getColor();
            color = color * 255;
            Color colorToDisp = new Color((byte)color.X, (byte)color.Y, (byte)color.Z, (byte)color.W);

            Raylib.DrawLine((int)particle.position.X, (int)particle.position.Y, (int)endpoint.X, (int)endpoint.Y, colorToDisp);
        }

        public void HandleEvent(core.GameData data, in core.GameInput input, List<core.Event> events)
        {
            foreach (core.Event curEvent in events)
            {
                switch (curEvent)
                {
                    case core.Events.EnemyKilled:
                        core.Events.EnemyKilled enemyKilled = curEvent as core.Events.EnemyKilled;
                        Raylib.DrawText($"+ {enemyKilled.enemy.pointYeld} pts", (int)(enemyKilled.enemy.position.X), (int)(enemyKilled.enemy.position.Y), 15, Color.WHITE);
                        Raylib.DrawText($"+ {enemyKilled.enemy.moneyYeld} $", (int)(enemyKilled.enemy.position.X), (int)(enemyKilled.enemy.position.Y + 15), 15, Color.LIME);
                        enemyKilled.frameCount++;
                        if (enemyKilled.frameCount > 25)
                            enemyKilled.canBeDeleted = true;
                        break;

                    case core.Events.UseBomb:
                        core.Events.UseBomb useBomb = curEvent as core.Events.UseBomb;
                        Raylib.DrawCircleLines((int)useBomb.position.X, (int)useBomb.position.Y, Calc.Lerp( (float)useBomb.frameCount/15, 0, 750), new Color(255, 86, 48, 125));
                        useBomb.frameCount++;
                        if (useBomb.frameCount >= 25)
                            useBomb.canBeDeleted = true;
                        break;

                    case core.Events.BlackHoleExplode:
                        core.Events.BlackHoleExplode blackHoleExplode = curEvent as core.Events.BlackHoleExplode;
                        Raylib.DrawCircleLines((int)blackHoleExplode.blackHole.position.X, (int)blackHoleExplode.blackHole.position.Y, Calc.Lerp((float)blackHoleExplode.frameCount / 15, 0, blackHoleExplode.blackHole.getExplosionRange()), new Color(255, 86, 48, 125));

                        blackHoleExplode.frameCount++;
                        if (blackHoleExplode.frameCount >= 15)
                            blackHoleExplode.canBeDeleted = true;
                        break;

                    case core.Events.LevelUp:
                        core.Events.LevelUp levelUp = curEvent as core.Events.LevelUp;
                        Raylib.DrawText($"Level {levelUp.level}", (int)(input.screenSize.X/2 - 100), (int)(input.screenSize.Y/2 - 50), 50, Color.PURPLE);
                        levelUp.timer -= input.deltaTime;
                        if (levelUp.timer < 0)
                        {
                            levelUp.canBeDeleted = true;
                        }

                        break;

                    case core.Events.GameOver:
                        Raylib.DrawText("Game over", (int)(input.screenSize.X / 2 - 300), (int)(input.screenSize.Y / 2 - 100), 100, Raylib.ColorAlpha(Color.RED, 0.7f));

                        if (!input.playerInputs.isGamepadAvailable)
                            Raylib.DrawText("Press space to restart", (int)(input.screenSize.X / 2 - 200), (int)(input.screenSize.Y / 2), 50, Raylib.ColorAlpha(Color.GOLD, 0.7f));
                        else
                            Raylib.DrawText("Press A to restart", (int)(input.screenSize.X / 2 - 200), (int)(input.screenSize.Y / 2), 50, Raylib.ColorAlpha(Color.GOLD, 0.7f));
                        break;

                    case core.Events.LifeBought:
                        core.Events.LifeBought lifeBought = curEvent as core.Events.LifeBought;
                        Raylib.DrawCircleLines((int)data.player.position.X, (int)data.player.position.Y, 25, Color.SKYBLUE);
                        lifeBought.timer -= input.deltaTime;
                        if (lifeBought.timer < 0)
                            lifeBought.canBeDeleted = true;
                        break;
                }
            }
        }

        public Graphics()
        {
            LoadPlayer();
            LoadWeapon();
            LoadBullet();
            LoadGrunt();
            LoadLife();
        }
    }
}
