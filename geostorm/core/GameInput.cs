using System;
using System.Numerics;

namespace geostorm.core
{
    struct PlayerInput
    {
        public bool shoot;
        private Vector2 shootTarget;
        public void setTarget(Vector2 target) { shootTarget = target; }
        public Vector2 getTarget() { return shootTarget; }

        private Vector2 shootAxis;
        public void setShootAxis(Vector2 Axis) { shootAxis = Axis; }
        public Vector2 getShootAxis() { return shootAxis; }

        public bool moveUp;
        public bool moveDown;
        public bool moveLeft;
        public bool moveRight;

        private Vector2 moveAxis;
        public void setMoveAxis(Vector2 Axis) { moveAxis = Axis; }
        public Vector2 getMoveAxis() { return moveAxis; }

        public bool isGamepadAvailable;

        public bool buyWeaponUpgrade;
        public bool useBomb;
        public bool buyLife;

        public bool restart;
    }

    class GameInput
    {
        public Vector2 screenSize;
        public float deltaTime;
        public bool debug;
        public bool isGamePaused;

        public PlayerInput playerInputs;

        public GameInput(Vector2 screenSize)
        {
            this.screenSize = screenSize;
            playerInputs = new PlayerInput();
        }
    }
}

