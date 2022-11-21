using Application;
using UtilityLibraries;

class Program
{
    public static Dictionary<int, string> Instructions => new Dictionary<int, string>()
    {
        {1, "Enter polynomial coefficients: [1, 2, 3] => 1 + 2x + 2x^2"},
        {2, "Enter a polynomial product: [1, 2]*[-1, 2, 1] => (1 + 2x) * (1 + 2x + 2x^2) "},
        {3, "Enter a sum of polynomials: Ex, [-1, -1] + [2, 1, 1] => (-1 + -x) + (2 + x + x^2) "},
        {4, "Enter an exponential: Exp([-1, 0, 3, 0, 5]) "}

    };

    static void Main(string[] args)
    {
        Console.WriteLine("");
        PrintInstructions($"Welcome to the (mini-cas) Algebra App.");
        do
        {
            string? input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                if (AreYouSure())
                {
                    break;
                }
            }
            else if (input.ToLower() == "h")
            {
                PrintHelp();
            }
            else
            {
                try
                {
                    HandleCommand(input); 
                }
                catch (Exception ex)
                {
                    Printer.PrintNewLine("Oops!");
                    Printer.PrintNewLine(ex.Message);
                }
                
                ResetConsole();
            }
        }
        while (true);

        return;
    }

    static void HandleCommand(string command)
    {
        if (UInt32.TryParse(command, out uint parsed))
        {
            Printer.PrintString($"You entered: {parsed} - {Instructions[(int)(parsed % 6)]}");

            return;
        }

        Printer.PrintNewLine($"This is what you wrote: {command}");
        AlgebraCommands.HandleInput(command);
    }

    static bool AreYouSure()
    {
        Printer.PrintNewLine("Are you sure you wanna exit? ... <n> to abort.");
        Printer.PrintNewLineWithBreak("... yes? <enter>, <y>, or anything else to exit.");

        string? input = Console.ReadLine();
        if (!String.IsNullOrEmpty(input) && input.ToLower().StartsWith("n"))
        {
            Printer.PrintString("Exit aborted ... ");
            PrintInstructions("What next? type something or <enter> to exit.");
            return false;
        }
        Printer.PrintNewLine("Adios!");
        return true;
    }

    // Declare a ResetConsole local method
    static void ResetConsole(string prefix = "")
    {
        Printer.PrintNewLine("");
        // Printer.PrintNewLine($"{prefix}... type anything to start over: ");
        // Console.ReadLine();
        // Console.Clear();
        PrintInstructions("What's next?");
    }

    static void PrintInstructions(string prefix = "", string suffix = "")
    {
        Printer.PrintString($"Press <Enter> to exit; otherwise, type something (<H> for help) then <enter>:");
    }

    static void PrintHelp()
    {
        Printer.PrintNewLineWithBreak($"Try typing one of these keys or inputting a polynomial:");

        foreach (var item in Instructions)
        {
            Printer.PrintNewLine(item.Value);
        }

        Printer.PrintNewLine($"Try it! or ... <enter> to exit.");
    }
}