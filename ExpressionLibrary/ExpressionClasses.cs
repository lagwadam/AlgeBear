using System.Text;

namespace UtilityLibraries
{
    public enum ExpressionTypeEnum
    {
        Constant,
        Cos,
        Exp,
        Function,
        Log, // Assumes base e
        Polynomial,
        Power,
        Product,
        Quotient,
        RootNode,
        Sin,
        Sum,
        Tan,
        Variable,
    }

    public abstract class BinaryOperation : IBinaryOperation
    {
        public IExpression Left { get; set; }
        public IExpression Right { get; set; }
        public abstract ExpressionTypeEnum ExpressionType { get; }
        public abstract string Operation { get; }

        public BinaryOperation(IExpression left, IExpression right)
        {
            Left = left;
            Right = right;
        }
        public abstract T Accept<T>(IExpressionTreeVisitor<T> visitor);
        public virtual T Accept<T>(IExpressionMatchingVisitor<T> visitor, IExpression source)
        {
            return visitor.Visit(this, source);
        }

        public override string ToString()
        {
            return $"({Util.FormatParens(Left)}{Operation}{Util.FormatParens(Right)})";
        }
    }

    public class Constant : IPrimative
    {
        public double Value { get; set; }
        public ExpressionTypeEnum ExpressionType => ExpressionTypeEnum.Constant;
        // Constructor
        public Constant(double value)
        {
            Value = value;
        }
        public override string ToString() { return Value.ToString(); }
        public T Accept<T>(IExpressionTreeVisitor<T> visitor) { return visitor.Visit(this); }
        public T Accept<T>(IExpressionMatchingVisitor<T> visitor, IExpression expression) { return visitor.Visit(this, expression); }
    }

    public class Polynomial : IComposite
    {
        public Double[] Coefficients { get; set; }
        public IExpression InnerExpression { get; set; }
        public ExpressionTypeEnum ExpressionType => ExpressionTypeEnum.Polynomial;
        public Polynomial(Double[] coefficients, IExpression innerExpression) { Coefficients = coefficients; InnerExpression = innerExpression; }
        public virtual T Accept<T>(IExpressionTreeVisitor<T> visitor) { return visitor.Visit(this); }
        public virtual T Accept<T>(IExpressionMatchingVisitor<T> visitor, IExpression source) { return visitor.Visit(this, source); }
        public override string ToString()
        {
            if (Coefficients is null || Coefficients.Length == 0)
            {
                return string.Empty;
            }
            string inner = InnerExpression.ToString();
            string coefficient = string.Empty;
            IList<string> results = new List<string>();
            if (Coefficients.Length == 1)
            {
                if(Coefficients[0] == 0)
                {
                    return "0";
                }
                return Util.Rounded(Coefficients[0]);
            }
            // Return zero if all coefficients are zero
            var allZeros = true;
            for (int i = 0; i < Coefficients.Length; i++)
            {
                if (Coefficients[i] == 0)
                {
                    continue;
                }

                allZeros = false;

                if (i == 0)
                {
                    results.Add(Util.Rounded(Coefficients[0]));
                    continue;
                }

                if (i == 1)
                {
                    results.Add($"{Util.FormatCoeff(Coefficients[i])}{inner}");
                    continue;
                }

                results.Add($"{Util.FormatCoeff(Coefficients[i])}{inner}^{i}");
            }

            if (allZeros)
            {
                return "0";
            }

            var result = String.Join(" + ", results);
            return result.Replace("+ -", "- ");
        }
    }

    public class Log : Function
    {
        public override ExpressionTypeEnum ExpressionType => ExpressionTypeEnum.Log;
        public override string Name => "Log";
        public override ExpressionTypeEnum InverseFunctionType => ExpressionTypeEnum.Exp;
        public Log(IExpression argument) : base(argument) { }
        public override T Accept<T>(IExpressionTreeVisitor<T> visitor) { return visitor.Visit(this); }
        public override T Accept<T>(IExpressionMatchingVisitor<T> visitor, IExpression source) { return visitor.Visit(this, source); }
    }

    public class Exp : Function
    {
        public override ExpressionTypeEnum ExpressionType => ExpressionTypeEnum.Exp;
        public override string Name => "Exp";
        public override ExpressionTypeEnum InverseFunctionType => ExpressionTypeEnum.Log;
        public Exp(IExpression argument) : base(argument) { }
        public override T Accept<T>(IExpressionTreeVisitor<T> visitor) { return visitor.Visit(this); }
        public override T Accept<T>(IExpressionMatchingVisitor<T> visitor, IExpression source) { return visitor.Visit(this, source); }
    }

    public abstract class Function : IFunction
    {
        public abstract ExpressionTypeEnum ExpressionType { get; }
        public abstract string Name { get; }
        public IExpression Argument { get; set; }
        public abstract ExpressionTypeEnum InverseFunctionType { get; }
        public Function(IExpression argument)
        {
            Argument = argument;
        }
        public abstract T Accept<T>(IExpressionTreeVisitor<T> visitor);
        public abstract T Accept<T>(IExpressionMatchingVisitor<T> visitor, IExpression source);

        public override string ToString() { return $"{Name}({Argument.ToString()})"; }
    }

    public class Power : BinaryOperation
    {
        public override ExpressionTypeEnum ExpressionType => ExpressionTypeEnum.Power;
        public override string Operation => "^";
        public Power(IExpression left, IExpression right) : base(left, right) { }
        public override T Accept<T>(IExpressionTreeVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
        public override T Accept<T>(IExpressionMatchingVisitor<T> visitor, IExpression source)
        {
            return visitor.Visit(this, source);
        }
    }

    public class Product : BinaryOperation
    {
        public override ExpressionTypeEnum ExpressionType => ExpressionTypeEnum.Product;
        public override string Operation => "*";
        public Product(IExpression left, IExpression right) : base(left, right) { }
        public override T Accept<T>(IExpressionTreeVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
        public override T Accept<T>(IExpressionMatchingVisitor<T> visitor, IExpression source) { return visitor.Visit(this, source); }
    }

    public class Quotient : BinaryOperation
    {
        public override ExpressionTypeEnum ExpressionType => ExpressionTypeEnum.Quotient;
        public override string Operation => " / ";
        public Quotient(IExpression left, IExpression right) : base(left, right) { }
        public override T Accept<T>(IExpressionTreeVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
        public override T Accept<T>(IExpressionMatchingVisitor<T> visitor, IExpression source) { return visitor.Visit(this, source); }
    }

    public class RootNode : IExpression
    {
        public IExpression InnerExpression { get; set; }
        public ExpressionTypeEnum ExpressionType => ExpressionTypeEnum.RootNode;
        public RootNode(IExpression expression)
        {
            InnerExpression = expression;
        }
        public virtual T Accept<T>(IExpressionTreeVisitor<T> visitor) { return visitor.Visit(this); }
        public virtual T Accept<T>(IExpressionMatchingVisitor<T> visitor, IExpression source) { return visitor.Visit(this, source); }
        public override string ToString() { return $"{InnerExpression.ToString()}"; }
    }

    public class Sum : BinaryOperation
    {
        public override ExpressionTypeEnum ExpressionType => ExpressionTypeEnum.Sum;
        public override string Operation => " + ";
        public Sum(IExpression left, IExpression right) : base(left, right) { }
        public override T Accept<T>(IExpressionTreeVisitor<T> visitor) { return visitor.Visit(this); }
        public override T Accept<T>(IExpressionMatchingVisitor<T> visitor, IExpression source) { return visitor.Visit(this, source); }
    }

    public class Variable : IPrimative
    {
        public string Symbol { get; set; }
        public ExpressionTypeEnum ExpressionType => ExpressionTypeEnum.Variable;
        public Variable(string symbol)
        {
            Symbol = symbol;
        }
        public override string ToString() { return Symbol; }
        public T Accept<T>(IExpressionTreeVisitor<T> visitor) { return visitor.Visit(this); }
        public T Accept<T>(IExpressionMatchingVisitor<T> visitor, IExpression expression) { return visitor.Visit(this, expression); }
    }
}
