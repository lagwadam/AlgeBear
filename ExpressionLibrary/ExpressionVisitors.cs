using UtilityLibraries;

namespace UtilityLibraries
{
    public interface IExpressionTreeVisitor<TReturn>
    {
        TReturn Visit(Constant expression);
        TReturn Visit(Container expression);
        TReturn Visit(Difference expression);
        TReturn Visit(Quotient expression);
        TReturn Visit(Polynomial expression);
        TReturn Visit(Power expression);
        TReturn Visit(Product expression);
        TReturn Visit(Root expression);
        TReturn Visit(Sum expression);
        TReturn Visit(Variable expression);
    }

    public class EvaluationVisitor: IExpressionTreeVisitor<Double>
    {
        public IDictionary<string, double> TransformationMap { get; private set; } 
        public EvaluationVisitor(IDictionary<string, double> transformationMap)
        {
            TransformationMap =  transformationMap;
        }
        public double Visit(Constant expression)
        {
            return expression.Value; 
        }

        public double Visit(Variable expression)
        {
            if(!TransformationMap.ContainsKey(expression.Symbol))
            {
                throw new InvalidDataException("Cannot evalute {expression.Symbol}. Value not specified in TransformationMap");
            }

            return TransformationMap[expression.Symbol];
        }

        public double Visit(Sum expression)
        {
            return expression.Left.Accept(this) + expression.Right.Accept(this);
        }

        public double Visit(Product expression)
        {
            return expression.Left.Accept(this) * expression.Right.Accept(this);
        }

        public double Visit(Difference expression)
        {
            return expression.Left.Accept(this) - expression.Right.Accept(this);
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

        public double Visit(Container container)
        {
            return container.Accept(this);
        }

        public double Visit(Polynomial expression)
        {
            return expression.Accept(this);
        }

        public double Visit(Root expression)
        {
            return expression.Accept(this);
        }
    }
}
