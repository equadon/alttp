using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alttp.Core.Events;
using Alttp.Core.UI.Controls;
using Microsoft.Xna.Framework;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls.Desktop;

namespace Alttp.Core.UI.ContextMenus
{
    public class ContextMenu : ContextMenuControl
    {
        private readonly Dictionary<string, Action> _commands;
        public readonly Action Hide;

        public Vector2 Position
        {
            get { return new Vector2(Bounds.Top.Offset, Bounds.Left.Offset); }
            set { Bounds.Location = new UniVector(value.X, value.Y); }
        }

        public ContextMenu(string name, Action hide)
        {
            Name = name;
            SelectionMode = ListSelectionMode.Single;
            _commands = new Dictionary<string, Action>();
            Hide = hide;

            Bounds.Size.X = 200;
            UpdateHeight();
        }

        protected void AddCommand(string name, Action action)
        {
            if (!_commands.ContainsKey(name))
            {
                _commands.Add(name, action);
                Items.Add(name);
            }
        }

        /// <summary>
        /// Execute command for item on specified row.
        /// </summary>
        /// <param name="row">Row of the clicked item</param>
        public virtual void Execute(int row)
        {
            _commands[Items[row]]();
            Hide();
        }

        protected void UpdateHeight()
        {
            Bounds.Size.Y = 39 + Items.Count * ItemHeight;
        }

        protected override void OnRowClicked(int row)
        {
            Execute(row);
        }
    }
}
