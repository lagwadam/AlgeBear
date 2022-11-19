using UtilityLibraries;

namespace UtilityLibraries
{
    public class ExpressionMatchingVisitor: IExpressionMatchingVisitor<Boolean>
    {
        public IList<string> Transformations { get; private set; }
        public ExpressionMatchingVisitor()
        {
            Transformations = new List<string>();
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

        public bool Visit(Constant target, IExpression source)
        {
            var constant = source as Constant;
            if (constant is not null)
            {
                return (target.Value == constant.Value);
            }
            
            return false; 
        }

        public bool Visit(Exp target, IExpression source)
        {
            if (target.ExpressionType != source.ExpressionType)
            {
                return false;
            }

            Exp casted = source as Exp;
            if (casted is null)
            {
                return false;
            }
            else
            {
                return target.Argument.Accept(this, casted.Argument);
            }
        }

        public bool Visit(Function target, IExpression source)
        {
            Function casted = source as Function;
            if (casted is null)
            {
                return false;
            }
            else
            {
                return target.Argument.Accept(this, casted.Argument);
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

        public bool Visit(RootNode target, IExpression source)
        {
            if (target.ExpressionType != source.ExpressionType)
            {
                return false;
            }

            RootNode casted = source as RootNode;
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
    }
}
