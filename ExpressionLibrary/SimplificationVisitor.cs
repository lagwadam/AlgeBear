using UtilityLibraries;

namespace UtilityLibraries
{
    public class SimplificationVisitor: IExpressionTreeVisitor<IExpression>
    {
        public SimplificationVisitor()
        {
        }

        public IExpression Visit(Constant expression)
        {
            return expression; 
        }

        public IExpression Visit(Variable expression)
        {
            return expression;
        }

        public IExpression Visit(Sum expression)
        {
            IExpression simplified = expression;
            var leftConstant = expression.Left as Constant;
            var rightConstant = expression.Right as Constant;
            
            if(leftConstant is not null && rightConstant is not null)
            {
                simplified = new Constant(leftConstant.Value + rightConstant.Value);
            }   

            return simplified;
        }

        public IExpression Visit(Product expression)
        {
            expression.Left = expression.Left.Accept(this);
            expression.Right = expression.Right.Accept(this);

            return expression;
        }

        public IExpression Visit(Difference expression)
        {
            expression.Left = expression.Left.Accept(this);
            expression.Right = expression.Right.Accept(this);

            return expression;
        }
        
        public IExpression Visit(Quotient expression)
        {
            expression.Left = expression.Left.Accept(this);
            expression.Right = expression.Right.Accept(this);

            return expression;
        }

        public IExpression Visit(Power expression)
        {
            expression.Left = expression.Left.Accept(this);
            expression.Right = expression.Right.Accept(this);

            return expression;
        }

        public IExpression Visit(Container expression)
        {
            expression.InnerExpression = expression.InnerExpression.Accept(this);

            return expression;
        }

        public IExpression Visit(Polynomial expression)
        {
            expression.InnerExpression = expression.InnerExpression.Accept(this);
            
            return expression.Accept(this);
        }

        public IExpression Visit(Root expression)
        {
            expression.InnerExpression = expression.InnerExpression.Accept(this);

            return expression;
        }
    }
}
