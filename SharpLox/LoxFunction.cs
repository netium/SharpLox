using SharpLox.Stmts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpLox
{
    internal class LoxFunction : ILoxCallable
    {
        private readonly Function declaration;

        internal LoxFunction(Function declaration)
        {
            this.declaration = declaration;
        }

        public int Arity() => declaration.paramList.Count;

        public object Call(Interpreter interpreter, List<object> arguments)
        {
            LoxEnvironment environment = new LoxEnvironment(interpreter.Globals);

            for (int i = 0; i < declaration.paramList.Count; i++)
            {
                environment.Define(declaration.paramList[i].lexeme, arguments[i]);
            }

            try
            {
                interpreter.ExecuteBlock(declaration.body, environment);
            }
            catch (ReturnException exp)
            {
                return exp.Value;
            }

            return null;
        }

        public override string ToString() => "<fn " + declaration.name.lexeme + ">";
    }
}
