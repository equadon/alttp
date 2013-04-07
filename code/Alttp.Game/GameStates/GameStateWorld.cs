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
        private WorldComponent _world;
        private DebugComponent _debug;

        public GameStateWorld(GameComponentCollection mainCollection, InputManager input, WorldComponent world, DebugComponent debug)
            : base(mainCollection, world, debug)
        {
            _input = input;
            _world = world;
            _debug = debug;

            RegisterComponents();
        }

        public override void Update(GameTime gameTime)
        {
            _input.UpdateStates();
        }
    }
}
