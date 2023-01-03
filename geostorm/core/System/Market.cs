using System;
using System.Collections.Generic;
using System.Numerics;

namespace geostorm.core
{
    class Market : Isystem, IGameEventListener
    {
        public bool upgradeBuyable;
        public bool bombBuyable;
        public bool lifeBuyable;

        public bool gamepadAvailable;
        public bool isLevelMax;

        private int bombBought = 1;
        private int upgradePrice;
        private int lifeBought = 0;
        public int getBombPrice() { return 25 * bombBought; }

        public int getLifePrice() { return 200 + 50 * lifeBought; }

        private void SetUpgradePrice(GameData data) { upgradePrice = 100 + 50 * (data.player.weapon.getLevel() - 1); }
        public int GetUpgradePrice() { return upgradePrice; }


        public void DrawMarket(Graphics graphics)
        {
            graphics.DrawMarket(this);
        }

        public void Update(in GameInput input, GameData data, List<Event> events)
        {
            gamepadAvailable = input.playerInputs.isGamepadAvailable;

            if (data.player.weapon.getLevel() == Weapon.levelMax)
            {
                isLevelMax = true;
            }

            SetUpgradePrice(data);

            if (data.player.money >= GetUpgradePrice())
                upgradeBuyable = true;
            else
                upgradeBuyable = false;

            if (upgradeBuyable)
            {
                if (input.playerInputs.buyWeaponUpgrade && !isLevelMax)
                {
                    data.player.weapon.levelUp();
                    data.player.money -= GetUpgradePrice();
                }
            }

            if (data.player.money >= getBombPrice())
                bombBuyable = true;
            else
                bombBuyable = false;

            if (bombBuyable)
            {
                if (input.playerInputs.useBomb)
                {
                    data.player.money -= getBombPrice();
                    bombBought++;
                    events.Add(new Events.UseBomb(data.player.position));
                    foreach (Enemy enemy in data.Enemies)
                    {
                        if (Vector2.Distance(data.player.position, enemy.position) <= 750 && enemy.IsSpawned())
                        {
                            enemy.isDead = true;
                            events.Add(new Events.EnemyKilled(enemy, new Bullet(new Vector2(0, 0), 0)));
                        }
                    }
                }
            }

            if (data.player.money >= getLifePrice())
                lifeBuyable = true;
            else
                lifeBuyable = false;

            if (lifeBuyable)
            {
                if (input.playerInputs.buyLife)
                {
                    data.player.money -= getLifePrice();
                    data.player.life++;
                    data.player.getShielded();
                    lifeBought++;
                    events.Add(new Events.LifeBought());
                }
            }
        }

        public void HandleEvent(GameData data, in GameInput input, List<Event> events)
        {
            foreach (Event curEvent in events)
            {
                switch (curEvent)
                {
                    case Events.GameOver:
                        bombBought = 1;
                        lifeBought = 1;
                        isLevelMax = false;
                        break;
                }
            }
        }
    }
}
