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
R VisitIfStmt (If stmt);
R VisitPrintStmt (Print stmt);
R VisitVarStmt (Var stmt);
R VisitWhileStmt (While stmt);
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
public class If : Stmt
{
public If(Expr condition, Stmt thenBranch, Stmt elseBranch)
{
this.condition = condition;
this.thenBranch = thenBranch;
this.elseBranch = elseBranch;
}

internal readonly Expr condition;
internal readonly Stmt thenBranch;
internal readonly Stmt elseBranch;

public override R Accept<R>(IVisitor<R> visitor)
{
 return visitor.VisitIfStmt(this);
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
public class While : Stmt
{
public While(Expr condition, Stmt body)
{
this.condition = condition;
this.body = body;
}

internal readonly Expr condition;
internal readonly Stmt body;

public override R Accept<R>(IVisitor<R> visitor)
{
 return visitor.VisitWhileStmt(this);
}
}
}
