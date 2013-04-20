using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Alttp.Console.Events;

namespace Alttp.Console
{
    public class EventRaisingStreamWriter : StreamWriter
    {
        #region Event
        public event EventHandler<GenericEventArgs<string>> WroteString;
        #endregion

        public EventRaisingStreamWriter(Stream s)
            : base(s)
        { }

        private void OnWroteString(string text)
        {
            if (WroteString != null)
            {
                WroteString(this, new GenericEventArgs<string>(text));
            }
        }


        #region Overrides

        public override void Write(string value)
        {
            base.Write(value);
            OnWroteString(value);
        }

        public override void Write(bool value)
        {
            base.Write(value);
            OnWroteString(value.ToString());
        }

        #endregion
    }
}
