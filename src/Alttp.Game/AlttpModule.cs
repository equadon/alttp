﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alttp.Console;
using Alttp.Core;
using Alttp.Core.Input;
using Alttp.Core.World;
using Alttp.Debugging;
using Alttp.GameStates;
using Alttp.Worlds;
using Ninject;
using Ninject.Activation;
using Ninject.Extensions.NamedScope;
using Ninject.Modules;
using Nuclex.Game.States;
using Nuclex.UserInterface;

namespace Alttp
{
    public class AlttpModule : NinjectModule
    {
        public const string GameStateScope = "GameState";

        public override void Load()
        {
            Kernel.Bind<InputManager>().ToSelf().InSingletonScope();
            Kernel.Bind<Nuclex.Input.IInputService>().ToMethod(GetInputManager).InSingletonScope();

            Kernel.Bind<GuiManager>().ToSelf().InSingletonScope();
            Kernel.Bind<IGuiService>().ToMethod(GetGuiManager).InSingletonScope();

            Kernel.Bind<GameStateManager>().ToSelf().InSingletonScope();

            Kernel.Bind<GameController>().ToSelf().InSingletonScope();

            // Game states
            Kernel.Bind<GameStateWorld>().ToSelf().DefinesNamedScope(GameStateScope);

            Kernel.Bind<AlttpConsole>().ToSelf().InNamedScope(GameStateScope);

            Kernel.Bind<PythonInterpreter>().ToSelf().InNamedScope(GameStateScope);

            // Game components
            Kernel.Bind<WorldManager>().ToSelf().InNamedScope(GameStateScope);
            Kernel.Bind<DebugManager>().ToSelf().InNamedScope(GameStateScope);

            // Worlds
            Kernel.Bind<IWorld>().To<LightWorld>().InNamedScope(GameStateScope)
                .WithConstructorArgument("mapResource", "Maps/LightWorld")
                .WithConstructorArgument("worldObjectsResource", "GameObjects/WorldObjects/LightWorldObjectAnimations");

            // Player
            Kernel.Bind<Player>().ToSelf().InNamedScope(GameStateScope);
        }

        private static InputManager GetInputManager(IContext context)
        {
            return context.Kernel.Get<InputManager>();
        }

        private static GuiManager GetGuiManager(IContext context)
        {
            return context.Kernel.Get<GuiManager>();
        }
    }
}
