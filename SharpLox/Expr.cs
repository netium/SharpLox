using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpLox.Exprs;
using SharpLox.Stmts;

namespace SharpLox.Exprs
{
public interface IVisitor<R>
{
R VisitBinaryExpr (Binary expr);
R VisitGroupingExpr (Grouping expr);
R VisitLiteralExpr (Literal expr);
R VisitUnaryExpr (Unary expr);
R VisitVariableExpr (Variable expr);
}
public abstract class Expr
{
public abstract R Accept<R>(IVisitor<R> visitor);
}
public class Binary : Expr
{
public Binary(Expr left, Token op, Expr right)
{
this.left = left;
this.op = op;
this.right = right;
}

internal readonly Expr left;
internal readonly Token op;
internal readonly Expr right;

public override R Accept<R>(IVisitor<R> visitor)
{
 return visitor.VisitBinaryExpr(this);
}
}
public class Grouping : Expr
{
public Grouping(Expr expression)
{
this.expression = expression;
}

internal readonly Expr expression;

public override R Accept<R>(IVisitor<R> visitor)
{
 return visitor.VisitGroupingExpr(this);
}
}
public class Literal : Expr
{
public Literal(object value)
{
this.value = value;
}

internal readonly object value;

public override R Accept<R>(IVisitor<R> visitor)
{
 return visitor.VisitLiteralExpr(this);
}
}
public class Unary : Expr
{
public Unary(Token op, Expr right)
{
this.op = op;
this.right = right;
}

internal readonly Token op;
internal readonly Expr right;

public override R Accept<R>(IVisitor<R> visitor)
{
 return visitor.VisitUnaryExpr(this);
}
}
public class Variable : Expr
{
public Variable(Token name)
{
this.name = name;
}

internal readonly Token name;

public override R Accept<R>(IVisitor<R> visitor)
{
 return visitor.VisitVariableExpr(this);
}
}
}
