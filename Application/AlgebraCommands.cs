using Newtonsoft.Json;
using UtilityLibraries;

namespace Application
{
    public enum OperationEnum
    {
        Addition,
        Exp,
        Multiplication,
        Power,
        Unary
    }

    public static class Printer
    {
        public static void PrintString(string line, bool startWithBreak = false, bool endWithBreak = false)
        {
            if (startWithBreak) line = $"{Environment.NewLine}{line}";
            if (endWithBreak) line = $"{line}{Environment.NewLine}";
            Console.Write($"{line}");
        }
        public static void PrintNewLine(string line)
        {
            PrintString(line, true);
        }

        public static void PrintNewLineWithBreak(string line)
        {
            PrintString(line, true, true);
        }

        public static void PrintLineWithBreak(string line)
        {
            PrintString(line, false, true);
        }
    }

    public static class AlgebraCommands
    {
        static IList<string> SerializedCoefficients = new List<string>();
        static IList<Double[]> Coefficients = new List<Double[]>();
        static OperationEnum Operation = OperationEnum.Unary;
        public static void HandleInput(string input)
        {
            input = input.Replace(" ", "");
            SimplificationVisitor simplifier = new SimplificationVisitor();
            DifferentiationVisitor dxVisitor = new DifferentiationVisitor();
            EvaluationVisitor evaluator = new EvaluationVisitor(CreateEvaluationMap(1));

            SerializedCoefficients = new List<string>();
            Operation = OperationEnum.Addition;
            IExpression? expression = null;
            IExpression? expressionCopy = null;

            Printer.PrintNewLine("Splitting input for addition, multiplication, Exp of polynomials");

            if (input.Contains("+"))
            {
                Printer.PrintNewLine("Deserializing polynomial addition ...");
                SerializedCoefficients = input.Split("+");
                Operation = OperationEnum.Addition;
            }
            else if (input.Contains("*"))
            {
                Printer.PrintNewLine("Deserializing polynomial multiplication ...");
                SerializedCoefficients = input.Split("*");
                Operation = OperationEnum.Multiplication;
            }
            else if (input.Contains("^"))
            {
                Printer.PrintNewLine("Deserializing polynomial exponentiation ...");
                SerializedCoefficients = input.Split("^");
                Operation = OperationEnum.Power;
            }
            else if (input.Contains("exp"))
            {
                Printer.PrintNewLine("Deserializing polynomial exponentiation ...");
                var argument = input.Split("exp(").Last();
                char endParen = ')';
                if (argument is not null && argument.Last() == endParen)
                {
                    argument = argument.Substring(0, argument.Length-1);
                }
                SerializedCoefficients = new List<string> {argument!};
                Operation = OperationEnum.Exp;
            }
            else
            {
                Operation = OperationEnum.Unary;
                Printer.PrintNewLine("Deserializing polynomial ...");
                SerializedCoefficients.Add(input);
            }

            foreach (var serializedCoeffs in SerializedCoefficients)
            {
                Printer.PrintNewLine($"Deserializing coefficient: {serializedCoeffs}");
                var coeffs = JsonConvert.DeserializeObject<Double[]>(serializedCoeffs);
                Printer.PrintNewLine($"Deserialized = {coeffs}");
                if (Operation == OperationEnum.Addition)
                {
                    if (expression is null)
                    {
                        expression = new Polynomial(coeffs, new Variable("x"));
                        // Keep a copy of original Expression to show product rule
                        expressionCopy = new Polynomial(coeffs, new Variable("x"));
                    }
                    else
                    {
                        expression = new Sum(expression, new Polynomial(coeffs, new Variable("x")));
                        // Keep a copy of original Expression to show product rule
                        expressionCopy = new Sum(expressionCopy, new Polynomial(coeffs, new Variable("x")));
                    }

                }
                else if (Operation == OperationEnum.Multiplication)
                {
                    if (expression is null)
                    {
                        expression = new Polynomial(coeffs, new Variable("x"));
                        // Keep a copy of original Expression to show product rule
                        expressionCopy = new Polynomial(coeffs, new Variable("x"));
                    }
                    else
                    {
                        expression = new Product(expression, new Polynomial(coeffs, new Variable("x")));
                        // Keep a copy of original Expression to show product rule
                        expressionCopy = new Product(expressionCopy, new Polynomial(coeffs, new Variable("x")));
                    }
                }
                else if (Operation == OperationEnum.Exp)
                {
                    if (expression is null)
                    {
                        var poly = new Polynomial(coeffs,new Variable("x"));
                        var polyCopy = new Polynomial(coeffs,new Variable("x"));
                        expression = new Exp(poly);
                        // Keep a copy of original Expression to show product rule
                        expressionCopy = new Polynomial(coeffs, new Variable("x"));
                    }
                    else
                    {
                        // TODO: Add this when we support Powers in the CLI, test with Unit Test for now
                        
                        // Exp? arg = expression as Exp;
                        // if(arg is not null)
                        // {
                        //     Exp newExp = new Exp()
                        // }                        
                        // expression = new Exp(expression, new Polynomial(coeffs, new Variable("x")));
                        // // Keep a copy of original Expression to show product rule
                        // expressionCopy = new Power(expressionCopy, new Polynomial(coeffs, new Variable("x")));
                    }
                }
                else if (Operation == OperationEnum.Power)
                {
                    if (expression is null)
                    {
                        expression = new Polynomial(coeffs, new Variable("x"));
                        // Keep a copy of original Expression to show product rule
                        expressionCopy = new Polynomial(coeffs, new Variable("x"));
                    }
                    else
                    {
                        expression = new Power(expression, new Polynomial(coeffs, new Variable("x")));
                        // Keep a copy of original Expression to show product rule
                        expressionCopy = new Power(expressionCopy, new Polynomial(coeffs, new Variable("x")));
                    }
                }
                else
                {
                    expression = new Polynomial(coeffs, new Variable("x"));
                    // Keep a copy of original Expression to show product rule
                    expressionCopy = new Polynomial(coeffs, new Variable("x"));
                }
            }

            var unSimplified = expression!.ToString();
            var simplified = expression.Accept(simplifier).ToString();

            Printer.PrintNewLineWithBreak("Your expression is:");
            Printer.PrintNewLineWithBreak(expression.ToString() ?? string.Empty);

            if (unSimplified == simplified)
            {
                Printer.PrintNewLineWithBreak("Nice! Your expression is already simplified");
            }
            else
            {
                Simplify(expression);

                Printer.PrintNewLineWithBreak("Expanded and Simplied, your expression is:");
                Printer.PrintNewLineWithBreak(expression.Accept(simplifier).ToString() ?? string.Empty);
            }

            Printer.PrintLineWithBreak("The derivative of your polynomial is:");
            Printer.PrintNewLineWithBreak($"dP/dx = {expression.Accept(dxVisitor).ToString() ?? string.Empty.ToString() ?? string.Empty}");

            Printer.PrintNewLine($"We can also take the derivative of the unsimplified polynomial using the product rule and the linearity of the derivative.");
            Printer.PrintNewLineWithBreak($"Check it out! Enter any key to see it ...");
            Console.ReadLine();
            Printer.PrintNewLine($"OriginalPolynomial: {expressionCopy}");
            expressionCopy = expressionCopy!.Accept(dxVisitor);
            Printer.PrintNewLineWithBreak($"dP/dx = {expressionCopy.ToString()}");

            Printer.PrintNewLine($"This [can be simplified as well to: {Simplify(expressionCopy)}");

            Printer.PrintNewLine($"The Evaluation visitor can calculate values, enter any key ...");
            Console.ReadLine();
            // Using the EvaluationVisitor, we can evaluate this polynomial for any values. For example:
            PrintValue(evaluator, expression, 0);
            PrintValue(evaluator, expression, -1);
            PrintValue(evaluator, expression, 1.123);
            PrintValue(evaluator, expression, -3.23);
            PrintValue(evaluator, expression, 12);
        }

        public static void PrintValue(EvaluationVisitor evaluator, IExpression expression, double val)
        {
            evaluator.TransformationMap["x"] = val;
            Printer.PrintNewLine($"P({val})={expression.Accept(evaluator)}");
        }

        public static string Simplify(IExpression expression)
        {
            SimplificationVisitor simplifier = new SimplificationVisitor();
            string? unSimplified = expression.ToString();
            string? simplified = string.Empty;
            while (unSimplified != simplified)
            {
                unSimplified = simplified;
                simplified = expression.Accept(simplifier).ToString();
            }
            while (true)
                return simplified ?? string.Empty;
        }

        public static IDictionary<string, double> CreateEvaluationMap(double val, string var = "x")
        {
            return new Dictionary<string, double> { { var, val } };
        }
    }
}