using System;
using System.Collections.Generic;
using System.Numerics;
using ImGuiNET;

namespace geostorm.core
{
    class Game
    {
        private GameData data;

        private List<IGameEventListener> eventListeners;
        private List<Isystem> systems;

        private List<Event> events;

        private float debugEnemyTimer;
        private float debugBHTimer = 10;

        private Market market;

        private ParticleSystem particleSystem;

        private void cleanEvent()
        {
            events.RemoveAll(aEvent => aEvent.canBeDeleted);
        }

        public void AddEventListener(IGameEventListener listener)
        {
            eventListeners.Add(listener);
        }

        public void AddSystem(Isystem system)
        {
            systems.Add(system);
        }

        public void debugMenu(GameInput inputs)
        {
            ImGui.SetNextWindowBgAlpha(0.1f);
            ImGui.Begin("Game");
            ImGui.Text($"Delta time {inputs.deltaTime}");

            ImGui.SliderFloat("SpawnTimer", ref debugEnemyTimer, 0, 5);

            if (ImGui.Button("Add Enemy"))
            {
                Random rand = new Random();
                Grunt newOne = new Grunt(new Vector2(rand.Next() % inputs.screenSize.X, rand.Next() % inputs.screenSize.Y), 0, debugEnemyTimer, 1);
                data.AddEnemyDelayed(newOne);
            }

            ImGui.SliderFloat("BH LifeTime", ref debugBHTimer, 10, 120);

            if (ImGui.Button("Add Black Hole"))
            {
                Random rand = new Random();
                data.AddBlackHoleDelayed(new BlackHole(new Vector2(rand.Next() % inputs.screenSize.X, rand.Next() % inputs.screenSize.Y), 0, debugBHTimer));
            }

            ImGui.End();

            data.player.debugMenu();
        }

        public void Update(GameInput inputs)
        {

            foreach (Entity entity in data.Entities)
            {
                entity.Update(inputs, data, events);
            }

            foreach (Isystem system in systems)
            {
                system.Update(inputs, data, events);
            }

            foreach (IGameEventListener listener in eventListeners)
            {
                listener.HandleEvent(data, inputs, events);
            }

            data.update();
            cleanEvent();
        }

        public void render(Graphics graphics)
        {
            foreach (Entity entity in data.Entities)
            {
                entity.Draw(graphics);
            }

            graphics.drawPlayerUI(data.player, new Vector2(5, 5));
            market.DrawMarket(graphics);
            particleSystem.drawParticles(graphics);
        }

        public Game(Vector2 screenSize)
        {
            data = new GameData();
            eventListeners = new List<IGameEventListener>();
            systems = new List<Isystem>();
            events = new List<Event>();
            market = new Market();
            particleSystem = new ParticleSystem();

            AddEventListener(data);
            AddEventListener(particleSystem);
            AddEventListener(market);

            Player player = new Player(new Vector2(screenSize.X / 2, screenSize.Y / 2), 5);
            data.addPlayer(player);

            AddSystem(new ColisionSystem());
            AddSystem(new EnemySpawnSystem());
            AddSystem(market);
            AddSystem(particleSystem);
        }
    }
}
