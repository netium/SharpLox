using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpLox
{
    internal class LoxInstance
    {
        private LoxClass klass;

        private readonly Dictionary<string, object> fields = new Dictionary<string, object>();

        internal LoxInstance(LoxClass klass)
        {
            this.klass = klass;
        }

        public override string ToString()
        {
            return klass.Name + " instance";
        }

        internal object Get(Token name)
        {
            if (fields.ContainsKey(name.lexeme))
            {
                return fields[name.lexeme];
            }

            var method = klass.FindMethod(name.lexeme);
            if (method != null) return method.Bind(this);

            throw new RuntimeErrorException(name, "Undefined property '" + name.lexeme + "'.");
        }

        internal void Set(Token name, object value)
        {
            fields.Add(name.lexeme, value);
        }
    }
}
