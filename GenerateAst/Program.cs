// See https://aka.ms/new-console-template for more information

if (args.Length != 1)
{
    Console.WriteLine("Usage: GenerateAst <output directory>");
    Environment.Exit(64);
}

var outputDir = args[0];

DefineAst(outputDir, "Expr", new List<string>()
{
    "Assign : Token name, Expr value",
    "Binary : Expr left, Token op, Expr right",
    "Call : Expr callee, Token paren, List<Expr> arguments",
    "Get : Expr obj, Token name",
    "Grouping : Expr expression",
    "Literal : object value",
    "Logical : Expr left, Token op, Expr right",
    "Set : Expr obj, Token name, Expr value",
    "This : Token keyword",
    "Unary : Token op, Expr right",
    "Variable : Token name"
});
DefineAst(outputDir, "Stmt", new List<string>()
{
    "Block : List<Stmt> statements",
    "Class : Token name, List<Function> methods",
    "Expression : Expr expression",
    "Function : Token name, List<Token> paramList, List<Stmt> body",
    "If : Expr condition, Stmt thenBranch, Stmt elseBranch",
    "Print : Expr expression",
    "Return : Token keyword, Expr value",
    "Var : Token name, Expr initializer",
    "While : Expr condition, Stmt body"
});

void DefineAst(string outputDir, string baseName, List<string> types)
{
    var path = System.IO.Path.Combine(outputDir, baseName + ".cs");

    using (var sw = new StreamWriter(path))
    {
        // using statement
        sw.WriteLine("using System;");
        sw.WriteLine("using System.Collections.Generic;");
        sw.WriteLine("using System.Linq;");
        sw.WriteLine("using System.Text;");
        sw.WriteLine("using System.Threading.Tasks;");
        sw.WriteLine("using SharpLox.Exprs;");
        sw.WriteLine("using SharpLox.Stmts;");
        
        sw.WriteLine("");

        sw.WriteLine("namespace SharpLox." + baseName + "s");
        sw.WriteLine("{");

        // Visitor interface

        DefineVisitor(sw, baseName, types);

        // Base class
        sw.WriteLine("public abstract class " + baseName);

        sw.WriteLine("{");

        sw.WriteLine("public abstract R Accept<R>(IVisitor<R> visitor);");

        sw.WriteLine("}");

        // The AST classes
        foreach (var type in types)
        {
            var className = type.Split(':')[0].Trim();
            var fields = type.Split(':')[1].Trim();
            DefineType(sw, baseName, className, fields);
        }

        sw.WriteLine("}");
    }
}

void DefineVisitor(StreamWriter sw, string baseName, List<string> types)
{
    sw.WriteLine("public interface IVisitor<R>");
    sw.WriteLine("{");

    foreach (var type in types)
    {
        var typeName = type.Split(':')[0].Trim();
        sw.WriteLine("R Visit" + typeName + baseName + " (" + typeName + " " + baseName.ToLower() + ");");
    }

    sw.WriteLine("}");
}

void DefineType(StreamWriter sw, string baseName, string className, string fieldList)
{
    // Class definition
    sw.WriteLine("public class " + className + " : " + baseName);
    sw.WriteLine("{");
    // Constructor
    sw.WriteLine("public " + className + "(" + fieldList + ")");
    sw.WriteLine("{");

    var fields = fieldList.Split(',').Select(p=>p.Trim());

    foreach (var field in fields) 
    {
        var name = field.Split(' ')[1];
        sw.WriteLine("this." + name + " = " + name + ";");
    }
    sw.WriteLine("}");

    // Fields
    sw.WriteLine("");
    foreach (var field in fields)
    {
        sw.WriteLine("internal readonly " + field + ";");
    }

    // Visitor pattern
    sw.WriteLine();
    sw.WriteLine("public override R Accept<R>(IVisitor<R> visitor)");
    sw.WriteLine("{");
    sw.WriteLine(" return visitor.Visit" + className + baseName + "(this);");
    sw.WriteLine("}");
    sw.WriteLine("}");
}