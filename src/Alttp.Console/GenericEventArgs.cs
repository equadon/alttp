using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alttp.Console
{
    public class GenericEventArgs<T> : EventArgs
    {
        public T Value
        {
            get;
            private set;
        }

        public GenericEventArgs(T value)
        {
            Value = value;
        }
    }
}
