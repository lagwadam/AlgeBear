using UtilityLibraries;

namespace UtilityLibraries
{
    public class EvaluationVisitor : IExpressionTreeVisitor<Double>
    {
        public IDictionary<string, double> TransformationMap { get; private set; }
        public EvaluationVisitor(IDictionary<string, double> transformationMap)
        {
            TransformationMap = transformationMap;
        }
        public double Visit(Constant expression)
        {
            return expression.Value;
        }

        public double Visit(Exp expression)
        {
            return Math.Exp(expression.Argument.Accept(this));
        }

        public double Visit(ln expression)
        {
            return Math.Log(expression.Argument.Accept(this));
        }

        public double Visit(Polynomial expression)
        {
            double result = 0;
            double innerValue = expression.InnerExpression.Accept(this);
            for (int i = 0; i < expression.Coefficients.Length; i++)
            {
                result += expression.Coefficients[i] * Math.Pow(innerValue, i);
            }
            return result;
        }

        public double Visit(Power power)
        {
            var radix = power.Left.Accept(this);
            var exponent = power.Right.Accept(this);

            if (exponent == 0)
            {
                return 1;
            }

            return Math.Pow(radix, exponent);
        }

        public double Visit(Product expression)
        {
            return expression.Left.Accept(this) * expression.Right.Accept(this);
        }

        public double Visit(Quotient expression)
        {
            var denominator = expression.Right.Accept(this);
            if (denominator == 0)
            {
                throw new DivideByZeroException($"Cannot Divide by zero! Expression: {expression.Right.ToString()}");
            }
            return expression.Left.Accept(this) / expression.Right.Accept(this);
        }

        public double Visit(RootNode expression)
        {
            return expression.Accept(this);
        }

        public double Visit(Variable expression)
        {
            if (!TransformationMap.ContainsKey(expression.Symbol))
            {
                throw new InvalidDataException("Cannot evalute {expression.Symbol}. Value not specified in TransformationMap");
            }

            return TransformationMap[expression.Symbol];
        }
        public double Visit(Sum expression)
        {
            return expression.Left.Accept(this) + expression.Right.Accept(this);
        }
    }
}
