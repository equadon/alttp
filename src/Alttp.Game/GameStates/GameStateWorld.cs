using Alttp.Console;
using Alttp.Core;
using Alttp.Core.GameStates;
using Alttp.Core.Input;
using Alttp.Core.UI.Controls;
using Alttp.Debugging;
using Alttp.Worlds;
using Microsoft.Xna.Framework;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Visuals.Flat;

namespace Alttp.GameStates
{
    public class GameStateWorld : GameState
    {
        private readonly InputManager _input;
        private WorldManager _world;
        private DebugManager _debug;
        private Player _player;
        private readonly GuiManager _gui;
        private readonly Game _game;
        private readonly AlttpConsole _console;

        public GameStateWorld(GameComponentCollection mainCollection, InputManager input, WorldManager world, DebugManager debug, Player player, GuiManager gui, Game game, AlttpConsole console)
            : base(mainCollection, world, debug, console)
        {
            _input = input;
            _world = world;
            _debug = debug;
            _player = player;
            _gui = gui;
            _game = game;
            _console = console;

            RegisterComponents();

            Initialize();
        }

        private void Initialize()
        {
            _gui.Visualizer = FlatGuiVisualizer.FromFile(_game.Services, "Content/Skins/Alttp/Alttp.skin.xml");

            // Add image control renderer to renderer repository
            var flatGuiVisualizer = _gui.Visualizer as FlatGuiVisualizer;

            flatGuiVisualizer.RendererRepository.AddAssembly(typeof(FlatImageControlRenderer).Assembly);

            _gui.Screen = new Screen(_game.GraphicsDevice.Viewport.Width, _game.GraphicsDevice.Viewport.Height);
            _gui.DrawOrder = 1100;
        }

        public override void Update(GameTime gameTime)
        {
            _input.UpdateStates();

            _gui.Update(gameTime);
        }
    }
}
