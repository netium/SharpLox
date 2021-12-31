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
R VisitClassStmt (Class stmt);
R VisitExpressionStmt (Expression stmt);
R VisitFunctionStmt (Function stmt);
R VisitIfStmt (If stmt);
R VisitPrintStmt (Print stmt);
R VisitReturnStmt (Return stmt);
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
public class Class : Stmt
{
public Class(Token name, Variable superclass, List<Function> methods)
{
this.name = name;
this.superclass = superclass;
this.methods = methods;
}

internal readonly Token name;
internal readonly Variable superclass;
internal readonly List<Function> methods;

public override R Accept<R>(IVisitor<R> visitor)
{
 return visitor.VisitClassStmt(this);
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
public class Function : Stmt
{
public Function(Token name, List<Token> paramList, List<Stmt> body)
{
this.name = name;
this.paramList = paramList;
this.body = body;
}

internal readonly Token name;
internal readonly List<Token> paramList;
internal readonly List<Stmt> body;

public override R Accept<R>(IVisitor<R> visitor)
{
 return visitor.VisitFunctionStmt(this);
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
public class Return : Stmt
{
public Return(Token keyword, Expr value)
{
this.keyword = keyword;
this.value = value;
}

internal readonly Token keyword;
internal readonly Expr value;

public override R Accept<R>(IVisitor<R> visitor)
{
 return visitor.VisitReturnStmt(this);
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
