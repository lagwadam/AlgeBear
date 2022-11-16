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

        public double Visit(Polynomial expression)
        {
            return expression.Accept(this);
        }

        public double Visit(Root expression)
        {
            return expression.Accept(this);
        }
    }

    public class SimplificationVisitor: IExpressionTreeVisitor<Boolean>
    {
        public SimplificationVisitor()
        {
        }

        public bool Visit(Constant target)
        {
            return true; 
        }

        public bool Visit(Variable target)
        {
            return true;
        }

        public bool Visit(Sum target)
        {
            var leftConstant = target.Left as Constant;
            var rightConstant = target.Right as Constant;
            if(leftConstant is not null && rightConstant is not null)
            {
                var expression = new Constant(leftConstant.Value + rightConstant.Value);
            }
            return target.Left.Accept(this) && target.Right.Accept(this);
        }

        public bool Visit(Product target)
        {
            return true;
        }

        public bool Visit(Difference target)
        {
            return true;
        }
        
        public bool Visit(Quotient target)
        {
            return target.Accept(this);
        }

        public bool Visit(Power power)
        {
            return power.Accept(this);
        }

        // TODO: Remove because BinaryOperation is abstract
        // public bool Visit(BinaryOperation target)
        // {
        //     throw new NotImplementedException("Binary Operation Accept is abstract, so it should be handled in the subclasses.");
        // }

        public bool Visit(Container container)
        {
            return container.Accept(this);
        }

        public bool Visit(Polynomial expression)
        {
            // return expression.Accept(this);
            return expression.Accept(this);
        }

        public bool Visit(Root expression)
        {
            return expression.Accept(this);
        }
    }
}
