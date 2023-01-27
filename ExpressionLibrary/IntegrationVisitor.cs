using UtilityLibraries;

namespace UtilityLibraries
{
    public class IntegrationVisitor : IExpressionTreeVisitor<IExpression>
    {
        private readonly SimplificationVisitor _simplifier;
        private readonly EvaluationVisitor _evaluator;
        private readonly DifferentiationVisitor _differentiator;
        public IntegrationVisitor(Polynomial seriesRepresentation, Polynomial seriesIntegralRep, double center)
        {
            this.SeriesRepresentation = seriesRepresentation;
            this.SeriesIntegralRep = seriesIntegralRep;
            this.Center = center;

        }
        public Polynomial SeriesRepresentation { get; private set; }
        public Polynomial SeriesIntegralRep { get; private set; }
        public double Center { get; set; }
        public IntegrationVisitor(DifferentiationVisitor differentiator, SimplificationVisitor simplifier, EvaluationVisitor evaluator, double center)
        {
            _simplifier = simplifier;
            _differentiator = differentiator;
            _evaluator = evaluator;
            Center = center;

        }
        public IntegrationVisitor()
        {
            _differentiator = new DifferentiationVisitor();
            _simplifier = new SimplificationVisitor();
            Center = 0;
            _evaluator = new EvaluationVisitor(new Dictionary<string, double> { { "x", Center } });
        }
        public IntegrationVisitor(double center)
        {
            _differentiator = new DifferentiationVisitor();
            _simplifier = new SimplificationVisitor();
            Center = center;
            _evaluator = new EvaluationVisitor(new Dictionary<string, double> { { "x", Center } });

        }
        public IExpression Visit(Constant expression)
        {
            return IntegrateConstantTimesScalar(expression, 1);
        }

        public IExpression IntegrateBySeries(IExpression expression)
        {
            // Don't know expression, so caculate Series exansion
            double[] coeffs = new double[11];
            IExpression currentDerivative = expression;
            double currentFactorial = 1;
            double nonZeroCount = 0; // Only count non zero terms
            for (int i = 0; i < 5; i++)
            {
                coeffs[i] = (currentDerivative.Accept(_evaluator) / currentFactorial);
                if (coeffs[i] != 0)
                {
                    nonZeroCount += 1;
                }

                // Only ned five terms
                if (nonZeroCount >= 3)
                {
                    break;
                }

                // Prepare for next iteration
                currentDerivative = currentDerivative.Accept(_differentiator);

                if (i == 0)
                {
                    currentFactorial = 1;
                }
                else
                {
                    currentFactorial = currentFactorial * (i + 1);
                }

            }

            SeriesRepresentation = new Polynomial(coeffs, new Variable("x"));
            SeriesIntegralRep = this.Visit(new Polynomial(coeffs, new Variable("x"))) as Polynomial;

            return SeriesIntegralRep;
        }

        public IExpression Visit(Product expression)
        {
            IExpression simplified = expression.Accept(_simplifier);
            if (simplified is not null)
            {
                simplified = simplified.Accept(_simplifier);
            }

            Polynomial polynomial = simplified.Accept(_simplifier) as Polynomial;

            if (polynomial is not null)
            {
                var polynomialIntegral = Visit(polynomial);
                if (polynomialIntegral is not null)
                {
                    var casted = polynomialIntegral as Polynomial;
                    if (casted is not null)
                    {
                        return _simplifier.Visit(casted);
                    }
                    return polynomialIntegral.Accept(_simplifier);
                }
            }

            var integrated = IntegrateConstantTimesExpression(expression.Left as Constant, expression.Right);
            if (integrated is not null)
            {
                return integrated.Accept(_simplifier);
            }

            integrated = IntegrateConstantTimesExpression(expression.Right as Constant, expression.Left);
            if (integrated is not null)
            {
                return integrated;
            }

            throw new ArgumentException($"Cannot integrate given product: {expression.ToString()}");
        }

        public Polynomial IntegrateConstantTimesScalar(Constant constant, double scalar)
        {
            // return IntegrateConstant(expressionConstant)
            if (constant is null)
            {
                return null;
            }

            if (constant.Value == 0 || scalar == 0)
            {
                return new Polynomial(new double[] { 1 }, new Variable("x"));
            }

            // expression is non zero constant, so return constant * x
            return new Polynomial(new double[] { 0, constant.Value * scalar }, new Variable("x"));
        }

        public IExpression IntegrateConstantTimesExpression(Constant constant, IExpression expression)
        {
            if (constant is null)
            {
                return constant;
            }

            if (constant.Value == 0)
            {
                return ReturnZeroTimesExpression(expression);
            }

            Polynomial integrated = IntegrateConstantTimesScalar(expression as Constant, constant.Value);
            if (integrated is not null)
            {
                return integrated;
            }

            // Try to compute constant*variable first, then constant*polynomail, then constant*IExpression
            // This helps to generate a simpler integral
            Variable expressionVar = expression as Variable;
            if (expressionVar is not null)
            {
                // Product is a constant times x
                return new Polynomial(new double[] { 0, 0, 0.5 * constant.Value }, expressionVar);
            }
            else
            {
                Polynomial polyVar = expression as Polynomial;
                if (polyVar is not null)
                {
                    // Product is a constant times Polynomial
                    return new Product(constant, Visit(polyVar)).Accept(_simplifier);
                }
                else
                {
                    // Product is a constant times IExpression
                    return new Product(constant, expression.Accept(this));
                }
            }
        }

        public IExpression ReturnZeroTimesExpression(IExpression expression)
        {
            IExpression innerExpression = new Variable("x");
            if (expression is Variable)
            {
                innerExpression = expression;
            }
            else if (expression is Polynomial)
            {
                innerExpression = (expression as Polynomial).InnerExpression;
            }
            else
            {
                // Expression is IExpression, but we're lacking a feature to get it's innerExpression, so leave as defaulted
            }
            return new Polynomial(new double[] { 1 }, innerExpression);
        }

        public IExpression Visit(Sum expression)
        {
            // Take derivatives of left and right separately
            expression.Left = expression.Left.Accept(this);
            expression.Right = expression.Right.Accept(this);

            return _simplifier.Visit(expression);
        }

        public IExpression Visit(Variable expression)
        {
            return new Polynomial(new double[] { 0, 0, 0.5 }, new Variable(expression.Symbol));
        }

        public IExpression Visit(Exp expression)
        {
            if (expression.Argument is Variable)
            {
                return expression;
            }
            else
            {
                return IntegrateBySeries(expression);
            }
        }

        public IExpression Visit(Log expression)
        {
            Variable arg = expression.Argument as Variable;
            if (arg is not null)
            {
                new Sum(new Product(arg, new Log(arg)), new Product(new Constant(-1), new Log(arg)));
                return expression;
            }
            else
            {
                Polynomial polyArg = expression.Argument as Polynomial;
                if (polyArg.ToString() == "x" && polyArg.InnerExpression.ExpressionType == ExpressionTypeEnum.Variable)
                {
                    var prod = new Product(new Log(polyArg), new Polynomial(new double[] { 0, 1 }, polyArg));
                    return new Sum(prod, new Polynomial(new double[] { 0, -1 }, polyArg));
                }
                return expression;
                // throw new Exception("Cannot integrate Log functions with composition");
            }
        }

        public IExpression Visit(Quotient expression)
        {
            var leftConstant = expression.Left as Constant;
            var rightConstant = expression.Right as Constant;

            // Check for zero and constants
            if (leftConstant is not null)
            {
                if (leftConstant.Value == 0 && rightConstant is null)
                {
                    return new Constant(1);
                }

                if (rightConstant is not null && rightConstant.Value != 0)
                {
                    return new Polynomial(new double[] { 0, rightConstant.Value / leftConstant.Value }, new Variable("x"));
                }
            }

            if (rightConstant is not null)
            {
                if (rightConstant.Value == 0)
                {
                    throw new DivideByZeroException("The expression has a division by zero.");
                }

                // If dividing by constant, return integral of he left side multiplied by 1 / constant
                if (rightConstant is not null)
                {
                    return new Product(expression.Left, new Constant(1 / rightConstant.Value)).Accept(this);
                }
            }

            throw new InvalidDataException("Cannot integrate given quotient");
        }

        public IExpression Visit(Power expression)
        {
            IExpression simplified = expression;
            var leftConstant = expression.Left as Constant;
            var rightConstant = expression.Right as Constant;

            if (leftConstant is not null && rightConstant is not null)
            {
                return new Polynomial(new double[] { 0, Math.Pow(leftConstant.Value, rightConstant.Value) }, new Variable("x"));
            }

            throw new InvalidDataException("Cannot integration the power expression provided");
        }

        public IExpression Visit(Polynomial poly)
        {
            if (poly.InnerExpression.ExpressionType == ExpressionTypeEnum.Constant)
            {
                return new Constant(1);
            }

            IExpression innerExpression;
            var innerPoly = poly.InnerExpression as Polynomial;
            if (innerPoly is not null)
            {
                if (innerPoly.Coefficients != new double[] { 0, 1 })
                {
                    throw new Exception("Can only integrate polynomials if the innerexpression is a Variable ");
                }
                innerExpression = innerPoly.InnerExpression;
            }
            else
            {
                var innerVar = poly.InnerExpression as Variable;
                if (innerVar is null)
                {
                    throw new Exception("Can only integrate polynomials if the innerexpression is a Variable ");
                }
                innerExpression = innerVar;
            }

            if (poly.Coefficients.Length == 0 || (poly.Coefficients.Length == 1 && poly.Coefficients[0] == 0))
            {
                // Integral of zero should be non zero constant, 
                // Even though they're equivalent + C, the next integral of a non zeror constant should be linear in x
                return new Polynomial(new double[] { 1 }, innerExpression);
            }

            double[] newCoeffs = new double[poly.Coefficients.Length + 1];
            newCoeffs[0] = 0;

            var allZeros = true;
            for (int i = 0; i < poly.Coefficients.Length; i++)
            {
                if (poly.Coefficients[i] != 0)
                {
                    allZeros = false;
                }
                newCoeffs[i + 1] += poly.Coefficients[i] / (i + 1);
            }

            if (allZeros)
            {
                return new Polynomial(new double[] { 1 }, innerExpression);
            }

            return new Polynomial(newCoeffs, poly.InnerExpression);
        }

        public IExpression Visit(RootNode expression)
        {
            expression.InnerExpression = expression.InnerExpression.Accept(this);

            return expression;
        }
    }
}
