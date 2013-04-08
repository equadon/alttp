using Alttp.Core.GameStates;
using Alttp.Core.Input;
using Alttp.Debugging;
using Alttp.Worlds;
using Microsoft.Xna.Framework;

namespace Alttp.GameStates
{
    public class GameStateWorld : GameState
    {
        private readonly InputManager _input;
        private WorldManager _world;
        private DebugManager _debug;
        private Player _player;

        public GameStateWorld(GameComponentCollection mainCollection, InputManager input, WorldManager world, DebugManager debug, Player player)
            : base(mainCollection, world, debug)
        {
            _input = input;
            _world = world;
            _debug = debug;
            _player = player;

            RegisterComponents();
        }

        public override void Update(GameTime gameTime)
        {
            _input.UpdateStates();
        }
    }
}
