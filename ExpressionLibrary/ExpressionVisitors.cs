using UtilityLibraries;

namespace UtilityLibraries
{
    public interface IExpressionVisitor 
    {
        void Visit(Constant expression);
        void Visit(Variable expression);
        void Visit(BinaryOperation expression);
    }
    
    public interface ICompareVisitor<TReturn>
    {
        TReturn Visit(Constant expression, IExpression source);
        TReturn Visit(Variable expression, IExpression source);
        TReturn Visit(BinaryOperation expression, IExpression source);
    }

    public class ExpressionVariablesVisitor : IExpressionVisitor
    {
        public ISet<string> Variables { get; private set; }
        
        public ExpressionVariablesVisitor()
        {
            Variables = new HashSet<string>();
        }
        
        public void Visit(Constant expression)
        {
            // No vars to add; no expressions to drill down into
        }

        public void Visit(Variable expression)
        {
            Variables.Add(expression.Symbol);
        }

        public void Visit(BinaryOperation expression)
        {
            expression.Left.Accept(this);
            expression.Right.Accept(this);
        }
    }
    public class EquivalencyVisitor: ICompareVisitor<Boolean>
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
                return (target.Val == constant.Val);
            }
            
            return false; 
        }

        public bool Visit(Variable target, IExpression source)
        {
            var variable = source as Variable;
            
            var constant = source as Constant;
            if (constant is not null)
            {
                Transformations.Add($"{target.Symbol} ↦ {constant.Val}");
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
    }
}
