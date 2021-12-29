using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpLox
{
    public class RuntimeErrorException : Exception
    {
        private readonly Token token;

        public Token Token { get { return token; } }

        public RuntimeErrorException(Token token, string message) : base(message)
        {
            this.token = token;
        }
    }
}
