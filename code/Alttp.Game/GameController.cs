using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alttp.GameStates;
using Ninject;
using Nuclex.Game.States;

namespace Alttp
{
    /// <summary>
    /// High-level class to control states.
    /// Contains delegates which will be attached to game state events.
    /// </summary>
    internal class GameController
    {
        protected readonly GameStateManager GameStateManager;
        protected readonly IKernel Kernel;

        public GameController(IKernel kernel)
        {
            Kernel = kernel;

            GameStateManager = Kernel.Get<GameStateManager>();

            GameStart();
        }

        private void GameStart()
        {
            var gameState = Kernel.Get<GameStateWorld>();

            GameStateManager.Push(gameState);
        }
    }
}
