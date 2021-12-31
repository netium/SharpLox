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
R VisitAssignExpr (Assign expr);
R VisitBinaryExpr (Binary expr);
R VisitCallExpr (Call expr);
R VisitGetExpr (Get expr);
R VisitGroupingExpr (Grouping expr);
R VisitLiteralExpr (Literal expr);
R VisitLogicalExpr (Logical expr);
R VisitSetExpr (Set expr);
R VisitThisExpr (This expr);
R VisitUnaryExpr (Unary expr);
R VisitVariableExpr (Variable expr);
}
public abstract class Expr
{
public abstract R Accept<R>(IVisitor<R> visitor);
}
public class Assign : Expr
{
public Assign(Token name, Expr value)
{
this.name = name;
this.value = value;
}

internal readonly Token name;
internal readonly Expr value;

public override R Accept<R>(IVisitor<R> visitor)
{
 return visitor.VisitAssignExpr(this);
}
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
public class Call : Expr
{
public Call(Expr callee, Token paren, List<Expr> arguments)
{
this.callee = callee;
this.paren = paren;
this.arguments = arguments;
}

internal readonly Expr callee;
internal readonly Token paren;
internal readonly List<Expr> arguments;

public override R Accept<R>(IVisitor<R> visitor)
{
 return visitor.VisitCallExpr(this);
}
}
public class Get : Expr
{
public Get(Expr obj, Token name)
{
this.obj = obj;
this.name = name;
}

internal readonly Expr obj;
internal readonly Token name;

public override R Accept<R>(IVisitor<R> visitor)
{
 return visitor.VisitGetExpr(this);
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
public class Logical : Expr
{
public Logical(Expr left, Token op, Expr right)
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
 return visitor.VisitLogicalExpr(this);
}
}
public class Set : Expr
{
public Set(Expr obj, Token name, Expr value)
{
this.obj = obj;
this.name = name;
this.value = value;
}

internal readonly Expr obj;
internal readonly Token name;
internal readonly Expr value;

public override R Accept<R>(IVisitor<R> visitor)
{
 return visitor.VisitSetExpr(this);
}
}
public class This : Expr
{
public This(Token keyword)
{
this.keyword = keyword;
}

internal readonly Token keyword;

public override R Accept<R>(IVisitor<R> visitor)
{
 return visitor.VisitThisExpr(this);
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
