using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpLox
{
    internal class ClockNativeFunction : ILoxCallable
    {
        public int Arity() => 0;

        public object Call(Interpreter interpreter, List<object> arguments) => (double)System.Environment.TickCount / 1000;

        public override string ToString() => "<clock: native fn>";

    }
}
