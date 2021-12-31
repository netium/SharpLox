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
        private readonly LoxEnvironment closure;

        private readonly bool isInitializer;

        internal LoxFunction(Function declaration, LoxEnvironment closure, bool isInitializer)
        {
            this.closure = closure;
            this.declaration = declaration;
            this.isInitializer = isInitializer;
        }

        public int Arity() => declaration.paramList.Count;

        public object Call(Interpreter interpreter, List<object> arguments)
        {
            LoxEnvironment environment = new LoxEnvironment(closure);

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
                if (isInitializer) return closure.GetAt(0, "this");

                return exp.Value;
            }

            if (isInitializer) return closure.GetAt(0, "this");

            return null;
        }

        internal LoxFunction Bind(LoxInstance loxInstance)
        {
            LoxEnvironment environment = new LoxEnvironment(closure);
            environment.Define("this", loxInstance);
            return new LoxFunction(declaration, environment, isInitializer);
        }

        public override string ToString() => "<fn " + declaration.name.lexeme + ">";
    }
}
