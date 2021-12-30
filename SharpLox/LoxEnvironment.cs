using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpLox
{
    internal class LoxEnvironment
    {
        private readonly Dictionary<string, object> values = new Dictionary<string, object>();

        internal void Define(string name, object value)
        {
            values.Add(name, value);
        }

        internal object Get(Token name)
        {
            if (values.ContainsKey(name.lexeme))
            {
                return values[name.lexeme];
            }

            throw new RuntimeErrorException(name, "Undefined variable '" + name.lexeme + "'.");
        }
    }
}
