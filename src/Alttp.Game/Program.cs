using System;
using Alttp.Console;
using Alttp.Core.Input;
using Microsoft.Xna.Framework;
using Ninject;
using Nuclex.Game.States;
using Nuclex.Ninject.Xna;
using Nuclex.UserInterface;

namespace Alttp
{
    internal static class Program
    {
#if WINDOWS
        [STAThread]
#endif
        private static void Main(string[] args)
        {
            using (IKernel kernel = new StandardKernel())
            {
                // Initialize service bindings
                SetupBindings(kernel);

                // Configure the game's startup process
                kernel.Get<IGameInitializer>().Initializing += (s, e) => { Initialize(kernel); };

                // Run game loop
                using (var game = kernel.GetService<IGame>())
                {
                    game.Run();
                }
            }
        }

        /// <summary>
        /// Binds all dependencies to their implementations
        /// </summary>
        /// <param name="kernel">Ninject kernel to which the bindings will be added</param>
        private static void SetupBindings(IKernel kernel)
        {
            kernel.Bind<IGame>().To<AlttpGame>().InSingletonScope();
            kernel.Load<XnaModule>();
            kernel.Load<AlttpModule>();
        }

        /// <summary>
        /// Configures what happens when the game starts up
        /// </summary>
        /// <param name="kernel">Kernel that manages the game</param>
        private static void Initialize(IKernel kernel)
        {
            var gameComponents = kernel.Get<GameComponentCollection>();

            // Nuclex framework components
            gameComponents.Add(kernel.Get<InputManager>());
            gameComponents.Add(kernel.Get<GuiManager>());
            gameComponents.Add(kernel.Get<GameStateManager>());

            // Start the game
            new GameController(kernel);
        }
    }
}