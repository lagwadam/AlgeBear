using UtilityLibraries;

namespace UtilityLibraries
{
    public class SimplificationVisitor : IExpressionTreeVisitor<IExpression>
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
            var simplified = SimplifySumConstants(expression);
            if (simplified is not null)
            {
                return simplified;
            }

            IExpression simplifiedSum = AddPolynomials(expression);
            if (simplifiedSum is not null)
            {
                return simplifiedSum;
            }

            expression.Left = expression.Left.Accept(this);
            expression.Right = expression.Right.Accept(this);

            return expression;
        }

        public IExpression Visit(Product expression)
        {
            IExpression simplified = SimplifyProductConstants(expression);
            if (simplified is not null)
            {
                return simplified;
            }

            IExpression expanded = MultiplyPolynomials(expression);
            if (expanded is not null)
            {
                return expanded;
            }
            
            expression.Left = expression.Left.Accept(this);
            expression.Right = expression.Right.Accept(this);

            return expression;
        }

        public IExpression Visit(Difference expression)
        {
            var leftConstant = expression.Left as Constant;
            var rightConstant = expression.Right as Constant;

            if (leftConstant is not null && rightConstant is not null)
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

            if (leftConstant is not null && leftConstant.Value == 0 && rightConstant is null)
            {
                return new Constant(0);
            }

            if (leftConstant is not null && rightConstant is not null && rightConstant.Value != 0)
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

            if (leftConstant is not null && rightConstant is not null)
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

            return expression;
        }

        public IExpression Visit(RootNode expression)
        {
            expression.InnerExpression = expression.InnerExpression.Accept(this);

            return expression;
        }

        public Polynomial AddPolynomials(Sum expression)
        {
            var leftPoly = expression.Left as Polynomial;
            var rightPoly = expression.Right as Polynomial;
            
            if (leftPoly is null || rightPoly is null)
            {
                return null;
            }

            // Make sure the polynomials have the same inner expressions
            if (leftPoly.InnerExpression.ToString() != rightPoly.InnerExpression.ToString())
            {
                return null;
            }

            var leftArray = leftPoly.Coefficients;
            var rightArray = rightPoly.Coefficients;
            
            var newArray = new Double[Math.Max(leftArray.Length, rightArray.Length)];

            for (int i=0; i<leftArray.Length; i++)
            {
                newArray[i] += leftArray[i];
            }

            for (int i=0; i<rightArray.Length; i++)
            {
                newArray[i] += rightArray[i];
            }

            var newPoly = new Polynomial(newArray, leftPoly.InnerExpression);
             
            return newPoly;
        }

        public Polynomial MultiplyPolynomials(Product expression)
        {
            var leftPoly = expression.Left as Polynomial;
            var rightPoly = expression.Right as Polynomial;
            
            if (leftPoly is null || rightPoly is null)
            {
                return null;
            }

            // Make sure the polynomials have the same inner expressions
            if (leftPoly.InnerExpression.ToString() != rightPoly.InnerExpression.ToString())
            {
                return null;
            }

            var leftArray = leftPoly.Coefficients;
            var rightArray = rightPoly.Coefficients;
            
            var newArray = new Double[leftArray.Length + rightArray.Length];

            for (int i=0; i<leftArray.Length; i++)
            {
                for (int j=0; j<rightArray.Length; j++)
                {
                    newArray[i+j] += leftArray[i]*rightArray[j];
                }
            }

            var newPoly = new Polynomial(newArray, leftPoly.InnerExpression);
             
            return newPoly;
        }

        public IExpression SimplifySumConstants(Sum expression)
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

            if (leftConstant is not null && rightConstant is not null)
            {
                return new Constant(leftConstant.Value + rightConstant.Value);
            }

            return null;
        }
        public IExpression SimplifyProductConstants(Product expression)
        {
            var leftConstant = expression.Left as Constant;
            var rightConstant = expression.Right as Constant;

            if (leftConstant is not null)
            {
                if (leftConstant.Value == 0)
                {
                    return new Constant(0);
                }

                if (leftConstant.Value == 1)
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

                if (rightConstant.Value == 1)
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

            if (leftConstant is not null && rightConstant is not null)
            {
                return new Constant(leftConstant.Value * rightConstant.Value);
            }

            return null;
        }
    }
}
