using System;
using System.Collections.Generic;
using System.Numerics;
using geostorm.app;
using ImGuiNET;

namespace geostorm.core
{
    class Player : Entity
    {
        private float speed;

        public int life;
        public int score;
        public int money;
        private int level;

        private bool godMode;
        private bool gotHited;
        private bool shrine;
        private float shriningTimer;
        private float hitTinmer;
        private bool underShield;

        private Vector4 color;

        public void getHit()
        {
            gotHited = true;
            shriningTimer = 0;
            hitTinmer = 2;
            underShield = false;
        }

        public void getShielded()
        {
            gotHited = true;
            underShield = true;
            hitTinmer = 2;
        }

        public int getLevel() { return level; }

        public bool canTakeDamage() 
        {
            if (godMode)
                return false;

            return !gotHited; 
        }

        public Weapon weapon;
        public Player(Vector2 position, int life = 1)
            : base(position, 0)
        {
            this.life = life;
            score = 0;

            color = new Vector4(1, 1, 1, 1);

            colisionRadius = 18;

            weapon = new Weapon();

            shriningTimer = 0;
            gotHited = false;
            level = 1;
        }

        public void SetColor(float r, float g, float b, float a) { color = new Vector4(r, g, b, a); }
        public Vector4 GetColor() { return color; }

        private void ChangeRandColor()
        {
            Random rand = new Random();
            float r = (float)(rand.Next() % 255) / 255 + 0.1f;
            if (r < 0.4f)
                r += 0.4f;

            float g = (float)(rand.Next() % 255) / 255 + 0.1f;
            if (g < 0.4f)
                g += 0.4f;

            float b = (float)(rand.Next() % 255) / 255 + 0.1f;
            if (b < 0.4f)
                b += 0.4f;

            SetColor(r, g, b, 1);
        }

        private void checkLevelUp(List<Event> events)
        {
            if (score > level * 1000 + 500 * (level - 1))
            {
                level++;
                events.Add(new Events.LevelUp(level));
                ChangeRandColor();
            }
        }

        private void speedSlowDown()
        {
            if (speed > 0)
            {
                speed -= 0.25f;
            }

            if (speed < 0)
            {
                speed += 0.25f;
            }

            if (speed < -10)
            {
                speed = -10;
            }

            if (speed > 10)
            {
                speed = 10;
            }
        }

        private void doWallColision(in GameInput input)
        {
            if (position.X < colisionRadius)
            {
                rotation = 0;
            }

            if (position.Y < colisionRadius)
            {
                rotation = Calc.DegreesToRad(90);
            }

            if (position.X > input.screenSize.X)
            {
                rotation = Calc.DegreesToRad(180);
            }

            if (position.Y > input.screenSize.Y)
            {
                rotation = Calc.DegreesToRad(270);
            }
        }

        private void handlePlayerInputKeyboard(in GameInput input)
        {
            float newRotation;

            if (input.playerInputs.moveRight || input.playerInputs.moveUp || input.playerInputs.moveLeft || input.playerInputs.moveDown)
            {
                speed += 1f;
            }

            if (input.playerInputs.moveRight)
            {
                newRotation = Calc.DegreesToRad(0);

                if (newRotation - rotation > Math.PI)
                    rotation += (float)Math.PI * 2;
                if (newRotation - rotation < -Math.PI)
                    rotation -= (float)Math.PI * 2;

                rotation = Calc.Lerp(0.7f, newRotation, rotation);
            }

            if (input.playerInputs.moveDown)
            {
                newRotation = Calc.DegreesToRad(90);

                if (newRotation - rotation > Math.PI)
                    rotation += (float)Math.PI * 2;
                if (newRotation - rotation < -Math.PI)
                    rotation -= (float)Math.PI * 2;

                rotation = Calc.Lerp(0.7f, newRotation, rotation);
            }

            if (input.playerInputs.moveLeft)
            {
                newRotation = Calc.DegreesToRad(180);

                if (newRotation - rotation > Math.PI)
                    rotation += (float)Math.PI * 2;
                if (newRotation - rotation < -Math.PI)
                    rotation -= (float)Math.PI * 2;

                rotation = Calc.Lerp(0.7f, newRotation, rotation);
            }

            if (input.playerInputs.moveUp)
            {
                newRotation = Calc.DegreesToRad(270);

                if (newRotation - rotation > Math.PI)
                    rotation += (float)Math.PI * 2;
                if (newRotation - rotation < -Math.PI)
                    rotation -= (float)Math.PI * 2;

                rotation = Calc.Lerp(0.7f, newRotation, rotation);
            }
        }

        private void handlePlayerInputGamepad(in GameInput input)
        {
            float newRotation;

            if (input.playerInputs.getMoveAxis().X != 0 || input.playerInputs.getMoveAxis().Y != 0)
            {
                speed += 1;

                newRotation = MathF.Atan2(input.playerInputs.getMoveAxis().Y, input.playerInputs.getMoveAxis().X);

                if (newRotation - rotation > Math.PI)
                    rotation += (float)Math.PI * 2;
                if (newRotation - rotation < -Math.PI)
                    rotation -= (float)Math.PI * 2;

                rotation = Calc.Lerp(0.7f, newRotation, rotation);
            }
        }

        public void debugMenu()
        {
            ImGui.SetNextWindowBgAlpha(0.1f);
            ImGui.Begin($"Debug Player");
            ImGui.Text($"Position {position} | Rotation {rotation} | Speed {speed}");
            ImGui.SliderFloat2("Position", ref position, 0, 1500);
            ImGui.SliderAngle("Rotation", ref rotation);
            ImGui.SliderFloat("Speed", ref speed, -10, 10);
            if (ImGui.Button("Add 1000$"))
            {
                money += 1000;
            }
            ImGui.ColorPicker4("Player color", ref color);

            ImGui.End();
        }

        public override void Update(in GameInput input, GameData data, List<Event> events)
        {
            if (Program.debugMenuOpen)
                godMode = true;
            else 
                godMode = false;

            weapon.Update(input, data, events, this);

            if (input.playerInputs.isGamepadAvailable)
                handlePlayerInputGamepad(input);
            else
                handlePlayerInputKeyboard(input);

            position.X += (float)Math.Cos(rotation) * speed;
            position.Y += (float)Math.Sin(rotation) * speed;

            speedSlowDown();

            doWallColision(input);

            if (gotHited)
            {
                if (!underShield)
                {
                    if (shrine)
                        color.W = 0;
                    else
                        color.W = 1;

                    shriningTimer += input.deltaTime;
                    if (shriningTimer > 0.1f)
                    {
                        shriningTimer = 0;
                        shrine = !shrine;
                    }
                }

                hitTinmer -= input.deltaTime;

                if (hitTinmer < 0)
                    gotHited = false;
            }
            else
            {
                color.W = 1;
            }

            checkLevelUp(events);
            if (life <= 0)
            {
                isDead = true;
                events.Add(new Events.PlayerDie(position, color));
                events.Add(new Events.GameOver());
            }
        }

        public override void Draw(Graphics graphics)
        {
            graphics.DrawPlayer(this);
        }
    }
}
