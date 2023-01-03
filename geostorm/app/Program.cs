using Raylib_cs;
using ImGuiNET;
using System.Numerics;

namespace geostorm.app
{
    class Program
    {
        static private void UpdateInput(ref core.GameInput inputs)
        {
            inputs.playerInputs.moveUp = Raylib.IsKeyDown(KeyboardKey.KEY_W);
            inputs.playerInputs.moveDown = Raylib.IsKeyDown(KeyboardKey.KEY_S);
            inputs.playerInputs.moveLeft = Raylib.IsKeyDown(KeyboardKey.KEY_A);
            inputs.playerInputs.moveRight = Raylib.IsKeyDown(KeyboardKey.KEY_D);

            inputs.playerInputs.setTarget(Raylib.GetMousePosition());
            inputs.playerInputs.shoot = inputs.debug ? Raylib.IsKeyDown(KeyboardKey.KEY_SPACE) : Raylib.IsKeyDown(KeyboardKey.KEY_SPACE) || Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_LEFT);

            inputs.playerInputs.useBomb = Raylib.IsKeyPressed(KeyboardKey.KEY_Q);
            inputs.playerInputs.buyWeaponUpgrade = Raylib.IsKeyPressed(KeyboardKey.KEY_E);
            inputs.playerInputs.buyLife = Raylib.IsKeyPressed(KeyboardKey.KEY_F);
            inputs.playerInputs.restart = Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE);

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_F3))
            {
                inputs.debug = !inputs.debug;
            }

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_P))
            {
                inputs.isGamePaused = !inputs.isGamePaused;
            }

            if (Raylib.IsGamepadAvailable(0))
            {

                inputs.playerInputs.setShootAxis(new Vector2(Raylib.GetGamepadAxisMovement(0, GamepadAxis.GAMEPAD_AXIS_RIGHT_X), Raylib.GetGamepadAxisMovement(0, GamepadAxis.GAMEPAD_AXIS_RIGHT_Y)));
                inputs.playerInputs.setMoveAxis(new Vector2(Raylib.GetGamepadAxisMovement(0, GamepadAxis.GAMEPAD_AXIS_LEFT_X), Raylib.GetGamepadAxisMovement(0, GamepadAxis.GAMEPAD_AXIS_LEFT_Y)));
                inputs.playerInputs.shoot = inputs.playerInputs.getShootAxis().X != 0 || inputs.playerInputs.getShootAxis().Y != 0;
                inputs.playerInputs.isGamepadAvailable = true;
                inputs.playerInputs.useBomb = Raylib.IsGamepadButtonPressed(0, GamepadButton.GAMEPAD_BUTTON_LEFT_TRIGGER_1);
                inputs.playerInputs.buyWeaponUpgrade = Raylib.IsGamepadButtonPressed(0, GamepadButton.GAMEPAD_BUTTON_RIGHT_TRIGGER_1);
                inputs.playerInputs.buyLife = Raylib.IsGamepadButtonPressed(0, GamepadButton.GAMEPAD_BUTTON_RIGHT_FACE_LEFT);

                inputs.playerInputs.restart = Raylib.IsGamepadButtonPressed(0, GamepadButton.GAMEPAD_BUTTON_RIGHT_FACE_DOWN);


                if (Raylib.IsGamepadButtonPressed(0, GamepadButton.GAMEPAD_BUTTON_MIDDLE_RIGHT))
                {
                    inputs.isGamePaused = !inputs.isGamePaused;
                }

                inputs.playerInputs.isGamepadAvailable = true;
            }
            else
            {
                inputs.playerInputs.isGamepadAvailable = false;
            }

            inputs.deltaTime = Raylib.GetFrameTime();
        }

        static unsafe void Main(string[] args)
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            Raylib.SetTraceLogCallback(&Logging.LogConsole);
            Raylib.SetConfigFlags(ConfigFlags.FLAG_VSYNC_HINT | ConfigFlags.FLAG_WINDOW_RESIZABLE);
            int screenWidth = 1920;
            int screenHeight = 1080;
            Raylib.InitWindow(screenWidth, screenHeight, "Geostorm");
            Vector2 screenSize = new Vector2(screenWidth, screenHeight);

            Raylib.ToggleFullscreen();
            Raylib.SetTargetFPS(60);

            Raylib.InitAudioDevice();

            ImguiController controller = new ImguiController();

            controller.Load(screenWidth, screenHeight);

            core.Game game = new core.Game(screenSize);  
            core.GameInput inputs = new core.GameInput(screenSize);

            Graphics graphics = new Graphics();

            game.AddEventListener(graphics);

            Music music;
            music = Raylib.LoadMusicStream("sound/music.ogg");
            Raylib.SetMusicVolume(music, 0.05f);
            Raylib.PlayMusicStream(music);

            Shader shader = Raylib.LoadShader(null, "shader.fs");

            RenderTexture2D target = Raylib.LoadRenderTexture(screenWidth, screenHeight);
            Raylib.SetMouseCursor(MouseCursor.MOUSE_CURSOR_CROSSHAIR);

            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!Raylib.WindowShouldClose())
            {
                Raylib.UpdateMusicStream(music);
                // Update
                //----------------------------------------------------------------------------------
                float dt = Raylib.GetFrameTime();
                controller.Update(dt);
                // Feed the input events to our ImGui controller, which passes them through to ImGui.
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                Raylib.BeginTextureMode(target);
                Raylib.ClearBackground(Color.BLACK);

                UpdateInput(ref inputs);

                if (!inputs.isGamePaused)
                {
                    game.Update(inputs);
                    game.render(graphics);
                }
                else
                {
                    game.render(graphics);
                    Raylib.DrawText("Game paused", (screenWidth / 2) - 300, (screenHeight / 2) - 25, 100, Color.WHITE);
                }

                if (Raylib.IsKeyPressed(KeyboardKey.KEY_F1))
                {
                    Raylib.ToggleFullscreen();
                }
                if (inputs.debug)
                {
                    game.debugMenu(inputs);
                }

                Raylib.EndTextureMode();

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.BLACK);
                Raylib.BeginShaderMode(shader);
                Raylib.DrawTextureRec(target.texture, new Rectangle( 0, 0, (float)target.texture.width, (float)-target.texture.height ), new Vector2( 0, 0 ), Color.WHITE);
                Raylib.EndShaderMode();
                controller.Draw();
                Raylib.EndDrawing();

                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            Raylib.UnloadMusicStream(music);
            Raylib.UnloadShader(shader);
            controller.Dispose();
            Raylib.CloseAudioDevice();
            Raylib.CloseWindow();
            //--------------------------------------------------------------------------------------
        }
    }
}