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
            var leftConstant = expression.Left as Constant;
            var rightConstant = expression.Right as Constant;
            
            if (leftConstant is not null && leftConstant.Value == 0)
            {
                return expression.Right.Accept(this);
            }

            if (rightConstant is not null && rightConstant.Value == 0)
            {
                return expression.Left.Accept(this);
            }

            if(leftConstant is not null && rightConstant is not null)
            {
                return new Constant(leftConstant.Value + rightConstant.Value);
            }   

            expression.Left = expression.Left.Accept(this);
            expression.Right = expression.Right.Accept(this);

            return expression;
        }

        public IExpression Visit(Product expression)
        {
            var leftConstant = expression.Left as Constant;
            var rightConstant = expression.Right as Constant;
            
            if (leftConstant is not null)
            {
                if (leftConstant.Value == 0)
                {
                    return new Constant(0);
                }
                
                if(leftConstant.Value == 1)
                {
                    return expression.Right.Accept(this);
                }
            }

            if (rightConstant is not null)
            {
                if (rightConstant.Value == 0)
                {
                    return new Constant(0);
                }
                
                if(rightConstant.Value == 1)
                {
                    return expression.Left.Accept(this);
                }
            }

            if (rightConstant is not null && rightConstant.Value == 0)
            {
                return new Constant(0);
            }

            if (leftConstant is not null && leftConstant.Value == 0)
            {
                return new Constant(0);
            }

            if (rightConstant is not null && rightConstant.Value == 0)
            {
                return new Constant(0);
            }

            if(leftConstant is not null && rightConstant is not null)
            {
                return new Constant(leftConstant.Value * rightConstant.Value);
            }   
            
            expression.Left = expression.Left.Accept(this);
            expression.Right = expression.Right.Accept(this);

            return expression;
        }

        public IExpression Visit(Difference expression)
        {
            var leftConstant = expression.Left as Constant;
            var rightConstant = expression.Right as Constant;
            
            if(leftConstant is not null && rightConstant is not null)
            {
                return new Constant(leftConstant.Value - rightConstant.Value);
            }   

            expression.Left = expression.Left.Accept(this);
            expression.Right = expression.Right.Accept(this);

            return expression;
        }
        
        public IExpression Visit(Quotient expression)
        {
            IExpression simplified = expression;
            var leftConstant = expression.Left as Constant;
            var rightConstant = expression.Right as Constant;
            
            if(leftConstant is not null && leftConstant.Value == 0 && rightConstant is null)
            {
                return new Constant(0);
            }

            if(leftConstant is not null && rightConstant is not null && rightConstant.Value !=0)
            {
                simplified = new Constant(leftConstant.Value / rightConstant.Value);
            }   
    
            expression.Left = expression.Left.Accept(this);
            expression.Right = expression.Right.Accept(this);

            return expression;
        }

        public IExpression Visit(Power expression)
        {
            IExpression simplified = expression;
            var leftConstant = expression.Left as Constant;
            var rightConstant = expression.Right as Constant;
            
            if(leftConstant is not null && rightConstant is not null)
            {
                return new Constant(Math.Pow(leftConstant.Value, rightConstant.Value));
            }   

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

        public IExpression Visit(RootNode expression)
        {
            expression.InnerExpression = expression.InnerExpression.Accept(this);

            return expression;
        }
    }
}
