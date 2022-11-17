using UtilityLibraries;

namespace UtilityLibraries
{
    public class DifferentiationVisitor : IExpressionTreeVisitor<IExpression>
    {
        private readonly SimplificationVisitor _simplifier;
        public DifferentiationVisitor(SimplificationVisitor simplifier)
        {
            _simplifier = simplifier;
        }
        public DifferentiationVisitor()
        {
            // TODO: Refactor to use Constructor with Dependency Injection 
            _simplifier = new SimplificationVisitor();
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
            expression.Left = expression.Left.Accept(this);
            expression.Right = expression.Right.Accept(this);

            return _simplifier.Visit(expression);
        }

        public IExpression Visit(Product expression)
        {
            // Special Case for polynomials
            var polynomialDerivative = DifferentiatePolynomialProduct(expression);
            if (polynomialDerivative is not null)
            {
                var casted = polynomialDerivative as Polynomial;
                if (casted is not null)
                {
                    return _simplifier.Visit(casted);    
                }
                return polynomialDerivative.Accept(_simplifier);
            } 

            var dFirst = expression.Left.Accept(this);
            var dSecond = expression.Right.Accept(this);

            var derivative = new Sum(new Product(expression.Left, dSecond), new Product(expression.Right, dFirst));
            _simplifier.Visit(derivative); // Expands product
            _simplifier.Visit(derivative); // Adds polynomials

            return derivative;
        }

        public IExpression DifferentiatePolynomialProduct(Product expression)
        {
            var leftPoly = expression.Left as Polynomial;
            var rightPoly = expression.Right as Polynomial;

            if (leftPoly is null || rightPoly is null)
            {
                return null;
            }

            Polynomial dFirst = this.Visit(leftPoly) as Polynomial;
            Polynomial dSecond = this.Visit(rightPoly) as Polynomial;

            var derivative = new Sum(new Product(leftPoly, dSecond), new Product(rightPoly, dFirst));

            _simplifier.Visit(derivative);
            _simplifier.Visit(derivative);

            return derivative;
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

        public IExpression Visit(Polynomial poly)
        {
            if (poly.InnerExpression.ExpressionType == ExpressionTypeEnum.Constant)
            {
                return new Constant(0);
            }

            var newArray = new Double[poly.Coefficients.Length - 1];

            for (int i = 1; i < poly.Coefficients.Length; i++)
            {
                newArray[i - 1] += i * poly.Coefficients[i];
            }

            var newPoly = new Polynomial(newArray, poly.InnerExpression);
            if (poly.InnerExpression.ExpressionType == ExpressionTypeEnum.Variable)
            {
                // Don't need to apply chain rule, so just return
                return _simplifier.Visit(newPoly);
            }

            // Apply chain rule
            return new Product(poly.InnerExpression.Accept(this), newPoly);
        }

        public IExpression Visit(RootNode expression)
        {
            expression.InnerExpression = expression.InnerExpression.Accept(this);

            return expression;
        }
    }
}
