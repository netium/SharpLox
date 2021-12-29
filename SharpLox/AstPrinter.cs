using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpLox
{
    internal class AstPrinter : IVisitor<string>
    {
        public string visitBinaryExpr(Binary expr)
        {
            return ""; // return Parenthesize(expr.op.lexeme, expr.left, expr.right);
        }

        public string visitGroupingExpr(Grouping expr)
        {
            return ""; // return Parenthesize("group", expr.expression);
        }

        public string visitLiteralExpr(Literal expr)
        {
            throw new NotImplementedException();
        }

        public string visitUnaryExpr(Unary expr)
        {
            throw new NotImplementedException();
        }

        string Print(Expr expr)
        {
            return expr.accept(this);
        }
    }
}
