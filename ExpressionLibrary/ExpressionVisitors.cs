using UtilityLibraries;

namespace UtilityLibraries
{
    public interface IExpressionTreeVisitor<TReturn>
    {
        TReturn Visit(Constant expression);
        TReturn Visit(Variable expression);
        TReturn Visit(Sum expression);
        TReturn Visit(Product expression);
        TReturn Visit(Difference expression);
        TReturn Visit(Quotient expression);
        TReturn Visit(Power expression);
        TReturn Visit(Container expression);
    }

    public class EvaluationVisitor: IExpressionTreeVisitor<Double>
    {
        public IDictionary<string, double> TransformationMap { get; private set; } 
        public EvaluationVisitor(IDictionary<string, double> transformationMap)
        {
            TransformationMap =  transformationMap;
        }
        
        public double Visit(Constant target)
        {
            return target.Value; 
        }

        public double Visit(Variable target)
        {
            if(!TransformationMap.ContainsKey(target.Symbol))
            {
                throw new InvalidDataException("Cannot evalute {target.Symbol}. Value not specified in TransformationMap");
            }

            return TransformationMap[target.Symbol];
        }

        public double Visit(Sum target)
        {
            return target.Left.Accept(this) + target.Right.Accept(this);
        }

        public double Visit(Product target)
        {
            return target.Left.Accept(this) * target.Right.Accept(this);
        }

        public double Visit(Difference target)
        {
            return target.Left.Accept(this) - target.Right.Accept(this);
        }
        
        public double Visit(Quotient target)
        {
            var denominator = target.Right.Accept(this);
            if (denominator == 0)
            {
                throw new DivideByZeroException($"Cannot Divide by zero! Expression: {target.Right.ToString()}");
            }
            return target.Left.Accept(this) / target.Right.Accept(this);
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

        // public double Visit(BinaryOperation target)
        // {
        //     throw new NotImplementedException("Binary Operation Accept is abstract, so it should be handled in the subclasses.");
        // }

        public double Visit(Container container)
        {
            return container.Accept(this);
        }
    }
}
