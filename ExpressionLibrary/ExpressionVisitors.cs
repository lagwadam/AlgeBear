using UtilityLibraries;

namespace UtilityLibraries
{
    public interface IExpressionTreeVisitor<TReturn>
    {
        TReturn Visit(Constant expression, IExpression source);
        TReturn Visit(Variable expression, IExpression source);
        TReturn Visit(BinaryOperation expression, IExpression source);
        TReturn Visit(Container expression, IExpression source);
    }

    public class EquivalencyVisitor: IExpressionTreeVisitor<Boolean>
    {
        public IList<string> Transformations { get; private set; }
        public EquivalencyVisitor()
        {
            Transformations = new List<string>();
        }

        public bool Visit(Constant target, IExpression source)
        {
            var constant = source as Constant;
            if (constant is not null)
            {
                return (target.Value == constant.Value);
            }
            
            return false; 
        }

        public bool Visit(Variable target, IExpression source)
        {
            var variable = source as Variable;
            
            var constant = source as Constant;
            if (constant is not null)
            {
                Transformations.Add($"{target.Symbol} ↦ {constant.Value}");
                return true;
            }

            if (variable is not null)
            {
                Transformations.Add($"{target.Symbol} ↦ {variable.Symbol}");
                return true;
            }
            
            return false;
        }

        public bool Visit(BinaryOperation target, IExpression source)
        {
            if (target.ExpressionType != source.ExpressionType)
            {
                return false;
            }

            BinaryOperation? casted = source as BinaryOperation;
            if (casted is null)
            {
                return false;
            }
            else
            {
                // Check both sides of operation
                bool leftValid = target.Left.Accept(this, casted.Left);

                if(!leftValid)
                {
                    return false;
                } 

                return target.Right.Accept(this, casted.Right);
            }
        }

        public bool Visit(Container target, IExpression source)
        {
            if (target.ExpressionType != source.ExpressionType)
            {
                return false;
            }

            Container? casted = source as Container;
            if (casted is null)
            {
                return false;
            }
            else
            {
                return target.Expression.Accept(this, target);
            }
        }
    }

    public class Transformation
    {
        public string Symbol { get; set; }
        public double Value { get; set; }

        public Transformation(string symbol, double value)
        {
            Symbol = symbol;
            Value = value;
        }
    }

    public class EvaluationVisitor: IExpressionTreeVisitor<Double>
    {
        public IDictionary<string, double> TransformationMap { get; private set; } 

        public IList<string> Transformations { get; private set; }
        public EvaluationVisitor()
        {
            Transformations = new List<string>();
            TransformationMap =  new Dictionary<string, double>();
        }
        
        public double Visit(Constant target, IExpression source)
        {
            return target.Value; 
        }

        public double Visit(Variable target, IExpression source)
        {
            if(TransformationMap.ContainsKey(target.Symbol))
            {
                throw new InvalidDataException("Cannot evalute {target.Symbol}. Value not specified in TransformationMap");
            }

            return TransformationMap[target.Symbol];
        }

        public double Visit(BinaryOperation target, IExpression source)
        {
            return 0;
        }

        public double Visit(Container container, IExpression source)
        {
            return container.Accept(this, source);
        }
    }
}
