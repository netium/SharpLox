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

        private readonly LoxEnvironment enclosing;

        internal LoxEnvironment Enclosing { get => enclosing; }

        internal LoxEnvironment()
        {
            this.enclosing = null;
        }

        internal LoxEnvironment(LoxEnvironment enclosing)
        {
            this.enclosing = enclosing;
        }

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
            if (this.enclosing != null) return enclosing.Get(name);

            throw new RuntimeErrorException(name, "Undefined variable '" + name.lexeme + "'.");
        }

        internal void Assign(Token name, object value)
        {
            if (values.ContainsKey(name.lexeme))
            {
                values[name.lexeme] = value;
                return;
            }

            if (this.enclosing != null)
            {
                enclosing.Assign(name, value);
                return;
            }

            throw new RuntimeErrorException(name, "Undefined variable '" + name.lexeme + "'.");
        }

        internal object GetAt(int distance, string name)
        {
            return Ancestor(distance).values[name];
        }

        private LoxEnvironment Ancestor(int distance)
        {
            var environment = this;
            for (int i = 0; i < distance; i++)
            {
                environment = environment.enclosing;
            }

            return environment;
        }

        internal void AssignAt(int distance, Token name, object value)
        {
            Ancestor(distance).values[name.lexeme] = value;
        }
    }
}
