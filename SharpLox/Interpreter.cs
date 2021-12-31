using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpLox.Exprs;
using SharpLox.Stmts;

namespace SharpLox
{
    internal class Interpreter : SharpLox.Exprs.IVisitor<object>, SharpLox.Stmts.IVisitor<object>
    {
        private readonly LoxEnvironment globals = new LoxEnvironment();

        private readonly Dictionary<Expr, int> locals = new Dictionary<Expr, int>();

        private LoxEnvironment environment;

        internal LoxEnvironment Globals { get => globals; }


        internal Interpreter()
        {
            environment = globals;

            globals.Define("clock", new ClockNativeFunction());
        }

        internal void Interpret(List<Stmt> statements)
        {
            try
            {
                foreach (var statement in statements)
                {
                    Execute(statement);
                }
            }
            catch (RuntimeErrorException exp)
            {
                Program.RuntimeError(exp);
            }
        }


        public object VisitBinaryExpr(Binary expr)
        {
            object left = Evaluate(expr.left);
            object right = Evaluate(expr.right);

            switch (expr.op.type)
            {
                case TokenType.GREATER:
                    CheckNumberOperands(expr.op, left, right);
                    return (double)left > (double)right;
                case TokenType.GREATER_EQUAL:
                    CheckNumberOperands(expr.op, left, right);
                    return (double)left >= (double)right;
                case TokenType.LESS:
                    CheckNumberOperands(expr.op, left, right);
                    return (double)left < (double)right;
                case TokenType.LESS_EQUAL:
                    CheckNumberOperands(expr.op, left, right);
                    return (double)left <= (double)right;
                case TokenType.BANG_EQUAL:
                    return !IsEqual(left, right);
                case TokenType.EQUAL_EQUAL:
                    return !IsEqual(left, right);
                case TokenType.MINUS:
                    CheckNumberOperands(expr.op, left, right);
                    return (double)left - (double)right;
                case TokenType.PLUS:
                    if (left is double && right is double)
                    {
                        return (double)left + (double)right;
                    }
                    if (left is string && right is string)
                    {
                        return (string)left + (string)right;
                    }
                    throw new RuntimeErrorException(expr.op, "Operands must be two numbers or two strings.");
                case TokenType.SLASH:
                    CheckNumberOperands(expr.op, left, right);
                    return (double)left / (double)right;
                case TokenType.STAR:
                    CheckNumberOperands(expr.op, left, right);
                    return (double)left * (double)right;
            }

            return null;
        }

        public object VisitGroupingExpr(Grouping expr)
        {
            return Evaluate(expr.expression);
        }

        public object VisitLiteralExpr(Literal expr)
        {
            return expr.value;
        }

        public object VisitUnaryExpr(Unary expr)
        {
            object right = Evaluate(expr.right);

            switch (expr.op.type)
            {
                case TokenType.BANG:
                    return !IsTruthy(right);
                case TokenType.MINUS:
                    return -(double)right;
            }

            return null;
        }

        private bool IsTruthy(object obj)
        {
            if (obj == null) return false;
            if (obj is bool) return (bool)obj;

            return true;
        }

        private object Evaluate(Expr expr)
        {
            return expr.Accept(this);
        }

        private void Execute(Stmt stmt)
        {
            stmt.Accept(this);
        }

        private bool IsEqual(object a, object b)
        {
            if (a == null && b == null) return true;
            if (a == null) return false;

            return a.Equals(b);
        }

        private void CheckNumberOperands(Token op, object left, object right)
        {
            if (left is double && right is double) return;
            throw new RuntimeErrorException(op, "Operands must be numbers.");
        }
        private void CheckNumberOperand(Token op, object operand)
        {
            if (operand is double) return;
            throw new RuntimeErrorException(op, "Operand must be a number");
        }

        private String Stringify(object obj)
        {
            if (obj == null) return "nil";

            if (obj is double)
            {
                var text = obj.ToString();
                if (text.EndsWith(".0"))
                {
                    text = text.Substring(0, text.Length - 2);
                }
                return text;
            }

            return obj.ToString();
        }

        internal void Resolve(Expr expr, int depth)
        {
            locals[expr] = depth;
        }

        public object VisitExpressionStmt(Expression stmt)
        {
            Evaluate(stmt.expression);
            return null;
        }

        public object VisitPrintStmt(Print stmt)
        {
            var value = Evaluate(stmt.expression);
            Console.WriteLine(Stringify(value));
            return null;
        }

        public object VisitVariableExpr(Variable expr)
        {
            return LookupVariable(expr.name, expr);
        }

        public object VisitVarStmt(Var stmt)
        {
            object value = null;
            if (stmt.initializer != null)
            {
                value = Evaluate(stmt.initializer);
            }

            environment.Define(stmt.name.lexeme, value);

            return null;
        }

        public object VisitAssignExpr(Assign expr)
        {
            object value = Evaluate(expr.value);
            int distance = locals[expr];
            environment.AssignAt(distance, expr.name, value);
            // if distance is object and null
            // globals.Assign(expr.name, value);
            return value;
        }

        public object VisitBlockStmt(Block stmt)
        {
            ExecuteBlock(stmt.statements, new LoxEnvironment(environment));

            return null;
        }

        internal void ExecuteBlock(List<Stmt> statements, LoxEnvironment environment)
        {
            LoxEnvironment previous = this.environment;

            try
            {
                this.environment = environment;

                foreach (var statement in statements)
                {
                    Execute(statement);
                }
            }
            finally
            {
                this.environment = previous;
            }
        }

        public object VisitIfStmt(If stmt)
        {
            if (IsTruthy(Evaluate(stmt.condition)))
            {
                Execute(stmt.thenBranch);
            }
            else if (stmt.elseBranch != null)
            {
                Execute(stmt.elseBranch);
            }

            return null;
        }

        public object VisitLogicalExpr(Logical expr)
        {
            var left = Evaluate(expr.left);

            if (expr.op.type == TokenType.OR)
            {
                if (IsTruthy(left)) return left;
            }
            else
            {
                if (!IsTruthy(left)) return left;
            }

            return Evaluate(expr.right);
        }

        public object VisitWhileStmt(While stmt)
        {
            while (IsTruthy(Evaluate(stmt.condition)))
            {
                Execute(stmt.body);
            }

            return null;
        }

        public object VisitCallExpr(Call expr)
        {
            object callee = Evaluate(expr.callee);

            var arguments = new List<object>();

            foreach (var argument in expr.arguments)
            {
                arguments.Add(Evaluate(argument));
            }

            if (!(callee is ILoxCallable))
            {
                throw new RuntimeErrorException(expr.paren, "Can only call functions and classes.");
            }

            ILoxCallable function = (ILoxCallable)callee;

            if (arguments.Count != function.Arity())
            {
                throw new RuntimeErrorException(expr.paren, "Expected " + function.Arity() + " arguments but got " + arguments.Count + ".");
            }

            return function.Call(this, arguments);
        }

        public object VisitFunctionStmt(Function stmt)
        {
            var function = new LoxFunction(stmt, environment);

            environment.Define(stmt.name.lexeme, function);

            return null;
        }

        public object VisitReturnStmt(Return stmt)
        {
            object value = null;
            if (stmt.value != null) value = Evaluate(stmt.value);

            throw new ReturnException(value);
        }

        private object LookupVariable(Token name, Expr expr)
        {
            int distance = locals[expr];
            return environment.GetAt(distance, name.lexeme);

            // if distance is object and it's null
            // return globals[name]
        }
    }
}
