using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpLox.Exprs;
using SharpLox.Stmts;

namespace SharpLox.Stmts
{
public interface IVisitor<R>
{
R VisitBlockStmt (Block stmt);
R VisitExpressionStmt (Expression stmt);
R VisitPrintStmt (Print stmt);
R VisitVarStmt (Var stmt);
}
public abstract class Stmt
{
public abstract R Accept<R>(IVisitor<R> visitor);
}
public class Block : Stmt
{
public Block(List<Stmt> statements)
{
this.statements = statements;
}

internal readonly List<Stmt> statements;

public override R Accept<R>(IVisitor<R> visitor)
{
 return visitor.VisitBlockStmt(this);
}
}
public class Expression : Stmt
{
public Expression(Expr expression)
{
this.expression = expression;
}

internal readonly Expr expression;

public override R Accept<R>(IVisitor<R> visitor)
{
 return visitor.VisitExpressionStmt(this);
}
}
public class Print : Stmt
{
public Print(Expr expression)
{
this.expression = expression;
}

internal readonly Expr expression;

public override R Accept<R>(IVisitor<R> visitor)
{
 return visitor.VisitPrintStmt(this);
}
}
public class Var : Stmt
{
public Var(Token name, Expr initializer)
{
this.name = name;
this.initializer = initializer;
}

internal readonly Token name;
internal readonly Expr initializer;

public override R Accept<R>(IVisitor<R> visitor)
{
 return visitor.VisitVarStmt(this);
}
}
}
