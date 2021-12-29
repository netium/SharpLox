using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpLox
{
public interface IVisitor<R>
{
R visitBinaryExpr (Binary expr);
R visitGroupingExpr (Grouping expr);
R visitLiteralExpr (Literal expr);
R visitUnaryExpr (Unary expr);
}
public abstract class Expr
{
public abstract R accept<R>(IVisitor<R> visitor);
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

public override R accept<R>(IVisitor<R> visitor)
{
 return visitor.visitBinaryExpr(this);
}
}
public class Grouping : Expr
{
public Grouping(Expr expression)
{
this.expression = expression;
}

internal readonly Expr expression;

public override R accept<R>(IVisitor<R> visitor)
{
 return visitor.visitGroupingExpr(this);
}
}
public class Literal : Expr
{
public Literal(object value)
{
this.value = value;
}

internal readonly object value;

public override R accept<R>(IVisitor<R> visitor)
{
 return visitor.visitLiteralExpr(this);
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

public override R accept<R>(IVisitor<R> visitor)
{
 return visitor.visitUnaryExpr(this);
}
}
}
