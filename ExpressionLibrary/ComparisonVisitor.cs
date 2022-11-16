using UtilityLibraries;

namespace UtilityLibraries
{
    public interface ITreeComparisonVisitor<TReturn>
    {
        TReturn Visit(Constant expression, IExpression source);
        TReturn Visit(Container expression, IExpression source);
        TReturn Visit(BinaryOperation expression, IExpression source);
        TReturn Visit(Polynomial expression, IExpression source);
        TReturn Visit(Root expression, IExpression source);
        TReturn Visit(Variable expression, IExpression source);
    }

    public class EquivalencyVisitor: ITreeComparisonVisitor<Boolean>
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

        public bool Visit(Container target, IExpression source)
        {
            if (target.ExpressionType != source.ExpressionType)
            {
                return false;
            }

            Container casted = source as Container;
            if (casted is null)
            {
                return false;
            }
            else
            {
                return target.InnerExpression.Accept(this, target);
            }
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

            BinaryOperation casted = source as BinaryOperation;
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
       
        public bool Visit(Polynomial target, IExpression source)
        {
            if (target.ExpressionType != source.ExpressionType)
            {
                return false;
            }

            Polynomial casted = source as Polynomial;
            if (casted is null)
            {
                return false;
            }
            else
            {
                return target.InnerExpression.Accept(this, casted.InnerExpression);
            }
        }

        public bool Visit(Root target, IExpression source)
        {
            if (target.ExpressionType != source.ExpressionType)
            {
                return false;
            }

            Root casted = source as Root;
            if (casted is null)
            {
                return false;
            }
            else
            {
                return target.InnerExpression.Accept(this, target);
            }
        }
    }
}
