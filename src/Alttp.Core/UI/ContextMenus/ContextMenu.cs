using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alttp.Core.Events;
using Alttp.Core.GameObjects.Interfaces;
using Alttp.Core.UI.Controls;
using Microsoft.Xna.Framework;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls.Desktop;

namespace Alttp.Core.UI.ContextMenus
{
    public class ContextMenu : ContextMenuControl
    {
        public event EventHandler CommandExecuted;

        public static readonly int MinimumWidth = 170;

        protected Dictionary<string, Action> Commands { get; set; }

        public Vector2 Position
        {
            get { return new Vector2(Bounds.Top.Offset, Bounds.Left.Offset); }
            set { Bounds.Location = new UniVector(value.X, value.Y); }
        }

        public ContextMenu(string name)
        {
            Name = name;
            SelectionMode = ListSelectionMode.Single;
            Commands = new Dictionary<string, Action>();

            UpdateSize();
        }

        protected void AddCommand(string name, Action action)
        {
            if (!Commands.ContainsKey(name))
            {
                Commands.Add(name, action);
                Items.Add(name);
            }
        }

        protected void ChangeCommand(string oldName, string newName, Action newAction)
        {
            if (Commands.ContainsKey(oldName) && Items.Contains(oldName))
            {
                Items[Items.IndexOf(oldName)] = newName;
                Commands.Remove(oldName);
                Commands.Add(newName, newAction);
            }
        }

        protected void ToggleCommandName(string s1, string s2)
        {
            if (!(Commands.ContainsKey(s1) || Commands.ContainsKey(s2)))
                return;

            string oldName = (Commands.ContainsKey(s1)) ? s1 : s2,
                   newName = (oldName == s1) ? s2 : s1;
            ChangeCommand(oldName, newName, Commands[oldName]);
        }

        /// <summary>
        /// Execute command for item on specified row.
        /// </summary>
        /// <param name="row">Row of the clicked item</param>
        public virtual void Execute(int row)
        {
            Commands[Items[row]]();
            if (CommandExecuted != null)
                CommandExecuted(this, EventArgs.Empty);
        }

        protected virtual void Clear()
        {
            // Clear previous commands
            Items.Clear();
            Commands.Clear();
        }

        protected void UpdateSize()
        {
            Bounds.Size.X = (int) MathHelper.Clamp(Name.Length * 10.5f, MinimumWidth, Config.ScreenWidth);
            Bounds.Size.Y = 39 + Items.Count * ItemHeight;
        }

        protected override void OnRowClicked(int row)
        {
            Execute(row);
        }
    }
}
