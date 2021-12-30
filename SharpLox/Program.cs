// See https://aka.ms/new-console-template for more information

using SharpLox;

namespace SharpLox
{
    class Program
    {
        private static readonly Interpreter interpreter = new Interpreter();

        static bool hasError = false;
        static bool hasRuntimeError = false;

        static void Main(String[] args)
        {
            if (args.Length > 1)
            {
                Console.WriteLine("Usage: SharpLox [lox script]");
                Environment.Exit(64);
            }
            else if (args.Length == 1)
            {
                RunFile(args[0]);
            }
            else
            {
                RunRrompt();
            }
        }

        private static void RunRrompt()
        {
            while (true)
            {
                Console.Write("> ");
                var line = Console.ReadLine();

                if (line == null) break;

                Run(line);

                hasError = false;
            }
        }

        private static void RunFile(string filename)
        {
            var bytes = System.IO.File.ReadAllBytes(filename);
            Run(System.Text.UTF8Encoding.UTF8.GetString(bytes));

            if (hasError)
                Environment.Exit(65);

            if (hasRuntimeError)
                Environment.Exit(70);
        }

        private static void Run(string source)
        {
            var scanner = new Scanner(source);
            var tokens = scanner.ScanTokens();
            var parser = new Parser(tokens);
            var statements = parser.Parse();

            if (hasError) return;

            interpreter.Interpret(statements);
        }

        public static void Error(int line, string message)
        {
            Report(line, "", message);
        }

        private static void Report(int line, string where, string message)
        {
            Console.Error.WriteLine("[line " + line + "] Error" + where + ": " + message);
            hasError = true;
        }

        internal static void Error(Token token, string message)
        {
            if (token.type == TokenType.EOF)
            {
                Report(token.line, " at end", message);
            }
            else
            {
                Report(token.line, " at '" + token.lexeme + "'", message);
            }
        }

        internal static void RuntimeError(RuntimeErrorException error)
        {
            Console.WriteLine(error.Message + "\n[line " + error.Token.line + "]");

            hasRuntimeError = true;
        }
    }
}