using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpLox
{
    internal class ReturnException : Exception
    {
        private readonly object value;

        internal object Value { get => value; }

        internal ReturnException(object value)
        {
            this.value = value;
        }
    }
}
