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

        public IExpression Visit(Product expression)
        {
            // Special Case for polynomials ... so it will simplify nicely
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

        public IExpression Visit(Sum expression)
        {
            expression.Left = expression.Left.Accept(this);
            expression.Right = expression.Right.Accept(this);

            return _simplifier.Visit(expression);
        }

        public IExpression Visit(Variable expression)
        {
            return expression;
        }

        public IExpression Visit(Exp expression)
        {
            // Special Case for polynomials ... so it will simplify nicely
            var polynomialDerivative = DifferentiateExpWithPolynomialArg(expression);
            if (polynomialDerivative is not null)
            {
                var casted = polynomialDerivative as Polynomial;
                if (casted is not null)
                {
                    return _simplifier.Visit(casted);
                }
                return polynomialDerivative.Accept(_simplifier);
            }

            var dArg = expression.Argument.Accept(this);

            var derivative = new Product(dArg, expression);
            _simplifier.Visit(derivative); // Simplifies arg

            return derivative;
        }

        public IExpression Visit(Log expression)
        {
            // Special Case for polynomials ... so it will simplify nicely
            IExpression polynomialDerivative = Differentiate_ln_WithPolynomialArg(expression);

            if (polynomialDerivative is not null)
            {
                return (polynomialDerivative);
            }

            // Take the derivatives for any IExpression in the arument;
            var dArg = expression.Argument.Accept(this);

            var derivative = new Quotient(dArg.Accept(this), new Polynomial(new Double[] { 0, 1 }, expression.Argument));
            _simplifier.Visit(derivative); // Simplifies arg

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

            var dLeft = expression.Left.Accept(this);
            var dRight = expression.Right.Accept(this);

            // f(x)^(g(x) - 1)*g(x)f'(x) + [f(x)^(g(x) - 1)]f(x)log(f(x))g'(x)
            // f(x)^(g(x) - 1)*g(x)f'(x) + H*g'(x)log(f(x))

            var var = new Polynomial(new double[] {0, 1}, new Variable("x"));

            var firstSummandLeft = new Power(expression.Left, new Sum(expression.Right, new Polynomial(new double[] {-1}, new Variable("x"))));
            var firstSummandRight = new Product(expression.Right, dLeft);
            var firstSummand = new Product(firstSummandLeft, firstSummandRight);

            var secondSummandLeft = new Product(expression, dRight);
            var secondSummandRight = new Log(expression.Left);
            var secondSummand = new Product(secondSummandLeft, secondSummandRight);

            return new Sum(firstSummand, secondSummand);
        }

        public IExpression Visit(Polynomial poly)
        {
            if (poly.InnerExpression.ExpressionType == ExpressionTypeEnum.Constant)
            {
                return new Constant(0);
            }

            if (poly.Coefficients.Length == 0)
            {
                return new Polynomial(new double[] { 0 }, poly.InnerExpression);
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

            _simplifier.Visit(derivative); // Expands project
            _simplifier.Visit(derivative); // Simplifies sum

            return derivative;
        }

        public IExpression DifferentiateExpWithPolynomialArg(Exp expression)
        {
            var argPoly = expression.Argument as Polynomial;

            if (argPoly is null)
            {
                return null;
            }

            Polynomial dArg = this.Visit(argPoly) as Polynomial;

            var derivative = new Product(dArg, expression);

            _simplifier.Visit(derivative);

            return derivative;
        }

        public IExpression Differentiate_ln_WithPolynomialArg(Log expression)
        {
            var argPoly = expression.Argument as Polynomial;

            if (argPoly is null)
            {
                return null;
            }

            Polynomial dArg = this.Visit(argPoly) as Polynomial;

            if (dArg is null)
            {
                return null;
            }

            var derivative = new Quotient(dArg, new Polynomial(new Double[] { 0, 1 }, argPoly));
            _simplifier.Visit(derivative); // Simplifies arg

            return derivative;
        }
    }
    
}
