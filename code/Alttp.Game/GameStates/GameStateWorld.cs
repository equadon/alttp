using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alttp.Engine;
using Alttp.Engine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Ninject;
using Nuclex.UserInterface;

namespace Alttp
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
