using SharpLox.Exprs;
using SharpLox.Stmts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpLox
{
    internal class Resolver : Exprs.IVisitor<object>, Stmts.IVisitor<object>
    {
        private readonly Interpreter interpreter;

        private readonly Stack<Dictionary<string, bool>> scopes = new Stack<Dictionary<string, bool>>();

        private FunctionType currentFunction = FunctionType.NONE;

        internal Resolver(Interpreter interpreter)
        {
            this.interpreter = interpreter;    
        }

        public object VisitAssignExpr(Assign expr)
        {
            Resolve(expr.value);
            ResolveLocal(expr, expr.name);
            return null;
        }

        public object VisitBinaryExpr(Binary expr)
        {
            Resolve(expr.left);
            Resolve(expr.right);

            return null;
        }

        public object VisitBlockStmt(Block stmt)
        {
            BeginScope();
            Resolve(stmt.statements);
            EndScope();

            return null;
        }

        public object VisitCallExpr(Call expr)
        {
            Resolve(expr.callee);

            foreach (var argument in expr.arguments)
            {
                Resolve(argument);
            }

            return null;
        }

        public object VisitExpressionStmt(Expression stmt)
        {
            Resolve(stmt.expression);
            return null;
        }

        public object VisitFunctionStmt(Function stmt)
        {
            Declare(stmt.name);
            Define(stmt.name);

            ResolveFunction(stmt, FunctionType.FUNCTION);

            return null;
        }

        public object VisitGroupingExpr(Grouping expr)
        {
            Resolve(expr.expression);

            return null;
        }

        public object VisitIfStmt(If stmt)
        {
            Resolve(stmt.condition);
            Resolve(stmt.thenBranch);

            if (stmt.elseBranch != null) Resolve(stmt.elseBranch);

            return null;
        }

        public object VisitLiteralExpr(Literal expr)
        {
            return null;
        }

        public object VisitLogicalExpr(Logical expr)
        {
            Resolve(expr.left);
            Resolve(expr.right);

            return null;
        }

        public object VisitPrintStmt(Print stmt)
        {
            Resolve(stmt.expression);
            return null;
        }

        public object VisitReturnStmt(Return stmt)
        {
            if (currentFunction == FunctionType.NONE)
            {
                Program.Error(stmt.keyword, "Cannot return from top-level code.");
            }

            if (stmt.value != null)
            {
                Resolve(stmt.value);
            }

            return null;
        }

        public object VisitUnaryExpr(Unary expr)
        {
            Resolve(expr.right);

            return null;
        }

        public object VisitVariableExpr(Variable expr)
        {
            if (scopes.Count > 0 && scopes.Peek()[expr.name.lexeme] == false)
            {
                Program.Error(expr.name, "Cannot read local variable in its own initializer.");
            }

            ResolveLocal(expr, expr.name);

            return null;
        }

        public object VisitVarStmt(Var stmt)
        {
            Declare(stmt.name);

            if (stmt.initializer != null)
            {
                Resolve(stmt.initializer);
            }

            Define(stmt.name);

            return null;
        }

        public object VisitWhileStmt(While stmt)
        {
            Resolve(stmt.condition);
            Resolve(stmt.body);

            return null;
        }

        internal void Resolve(List<Stmt> statements)
        {
            foreach (var statement in statements)
            {
                Resolve(statement);
            }
        }

        private void Resolve(Stmt stmt)
        {
            stmt.Accept(this);
        }

        private void Resolve(Expr expr)
        {
            expr.Accept(this);
        }

        private void BeginScope()
        {
            scopes.Push(new Dictionary<string, bool>());
        }

        private void EndScope()
        {
            scopes.Pop();
        }

        private void Declare(Token name)
        {
            if (scopes.Count == 0) return;

            Dictionary<string, bool> scope = scopes.Peek();

            if (scope.ContainsKey(name.lexeme))
            {
                Program.Error(name, "Already a variable with this name in this scope.");
            }

            scope.Add(name.lexeme, false);
        }

        private void Define(Token name)
        {
            if (scopes.Count == 0) return;
            scopes.Peek()[name.lexeme] = true;
        }

        private void ResolveLocal(Expr expr, Token name)
        {
            for (int i = scopes.Count - 1; i >= 0; i--)
            {
                if (scopes.ToArray()[i].ContainsKey(name.lexeme))
                {
                    interpreter.Resolve(expr, scopes.Count - 1 - i);
                    return;
                }
            }
        }

        private void ResolveFunction(Function function, FunctionType type)
        {
            FunctionType enclosingFunction = currentFunction;
            currentFunction = type;

            BeginScope();

            foreach (var param in function.paramList)
            {
                Declare(param);
                Define(param);
            }

            Resolve(function.body);
            EndScope();

            currentFunction = enclosingFunction;
        }
    }
}
