// See https://aka.ms/new-console-template for more information

class Program {

    static bool hasError = false;

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
    }

    private static void Run(string source)
    {
        Scanner scanner = new Scanner(source);
        List<Token> tokens = scanner.ScanTokens();

        foreach (var token in tokens) {
            Console.WriteLine(token);
        }
    }

    private static Error(int line, string message)
    {
        Report(line, "", message);
    }

    private static void Report(int line, string where, string message)
    {
        Console.Error.WriteLine("[line " + line + "] Error" + where + ": " + message);
        hadError = true;
    }
}

