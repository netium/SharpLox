using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpLox.Exprs;

namespace SharpLox
{
    internal class AstPrinter : IVisitor<string>
    {
        public string VisitAssignExpr(Assign expr)
        {
            throw new NotImplementedException();
        }

        public string VisitBinaryExpr(Binary expr)
        {
            return ""; // return Parenthesize(expr.op.lexeme, expr.left, expr.right);
        }

        public string VisitCallExpr(Call expr)
        {
            throw new NotImplementedException();
        }

        public string VisitGetExpr(Get expr)
        {
            throw new NotImplementedException();
        }

        public string VisitGroupingExpr(Grouping expr)
        {
            return ""; // return Parenthesize("group", expr.expression);
        }

        public string VisitLiteralExpr(Literal expr)
        {
            throw new NotImplementedException();
        }

        public string VisitLogicalExpr(Logical expr)
        {
            throw new NotImplementedException();
        }

        public string VisitSetExpr(Set expr)
        {
            throw new NotImplementedException();
        }

        public string VisitSuperExpr(Super expr)
        {
            throw new NotImplementedException();
        }

        public string VisitThisExpr(This expr)
        {
            throw new NotImplementedException();
        }

        public string VisitUnaryExpr(Unary expr)
        {
            throw new NotImplementedException();
        }

        public string VisitVariableExpr(Variable expr)
        {
            throw new NotImplementedException();
        }

        string Print(Expr expr)
        {
            return expr.Accept(this);
        }
    }
}
