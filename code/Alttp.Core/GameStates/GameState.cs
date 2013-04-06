using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Alttp.Engine
{
    /// <summary>
    /// Inherits from the Nuclex framework GameState class.
    /// An infinite number of GameComponents can be passed into the class.
    /// These GameComponents are then added to the main game ComponentCollection on GameState creation,
    /// and removed in OnLeaving.
    /// </summary>
    public class GameState : Nuclex.Game.States.GameState
    {
        private readonly GameComponentCollection _mainComponents;
        private readonly GameComponentCollection _subComponents = new GameComponentCollection();

        /// <summary>
        /// Will add components to the main GameComponentCollection and the game state GameComponentCollection.
        /// </summary>
        /// <param name="mainComponents">The main GameComponentCollection.</param>
        /// <param name="subComponents">A list of the GameComponents which you want to be added and removed with the CustomGameState.</param>
        protected GameState(GameComponentCollection mainComponents, params IGameComponent[] subComponents)
        {
            _mainComponents = mainComponents;

            foreach (IGameComponent subComponent in subComponents)
            {
                _subComponents.Add(subComponent);
            }
        }

        /// <summary>
        /// Add the components to the main collection here, 
        /// so that code in the GameState constructor can run.
        /// </summary>
        protected void RegisterComponents()
        {
            foreach (IGameComponent subComponent in _subComponents)
            {
                _mainComponents.Add(subComponent);
            }
        }

        public override void Update(GameTime gameTime)
        {
        }

        protected override void OnLeaving()
        {
            var tempComponents = new IGameComponent[_subComponents.Count];
            _subComponents.CopyTo(tempComponents, 0);

            foreach (IGameComponent tempComponent in tempComponents)
            {
                _subComponents.Remove(tempComponent);
                _mainComponents.Remove(tempComponent);
            }
        }
    }
}
