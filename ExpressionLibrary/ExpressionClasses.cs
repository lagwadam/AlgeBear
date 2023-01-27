using System.Text;

namespace UtilityLibraries
{
    public enum ExpressionTypeEnum
    {
        Constant,
        Container,
        Polynomial,
        Power,
        Product,
        Quotient,
        RootNode,
        Sum,
        Variable,
    }

    public abstract class BinaryOperation : IBinaryOperation
    {
        public IExpression Left { get; set; }
        public IExpression Right { get; set; }
        public abstract ExpressionTypeEnum ExpressionType { get; }
        public abstract string Operation { get; }
        public IExpression Parent { get; set; }

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
        public IExpression Parent { get; set; }
        // Constructor
        public Constant(double value)
        {
            Value = value;
        }
        public override string ToString() { return Value.ToString(); }
        public T Accept<T>(IExpressionTreeVisitor<T> visitor) { return visitor.Visit(this); }
        public T Accept<T>(IExpressionMatchingVisitor<T> visitor, IExpression expression) { return visitor.Visit(this, expression); }
    }

    public class Container : IContainer
    {
        public IExpression Parent { get; set; }
        public IExpression InnerExpression { get; set; }
        public ExpressionTypeEnum ExpressionType => ExpressionTypeEnum.Container;
        public Container(IExpression expression)
        {
            InnerExpression = expression;
        }
        public virtual T Accept<T>(IExpressionTreeVisitor<T> visitor) { return visitor.Visit(this); }
        public virtual T Accept<T>(IExpressionMatchingVisitor<T> visitor, IExpression source) { return visitor.Visit(this, source); }
        public override string ToString() { return $"({InnerExpression.ToString()})"; }
    }

    public class Polynomial : IComposite
    {
        public IExpression Parent { get; set; }
        public Double[] Coefficients { get; set; }
        public IExpression InnerExpression { get; set; }
        public ExpressionTypeEnum ExpressionType => ExpressionTypeEnum.Polynomial;
        public Polynomial(Double[] coefficients, IExpression innerExpression)
        {
            Coefficients = coefficients;
            InnerExpression = innerExpression;
        }
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
            for (int i = 0; i < Coefficients.Length; i++)
            {
                if (Coefficients[i] == 0)
                {
                    continue;
                }

                if (i == 0)
                {
                    results.Add(Coefficients[0].ToString());
                    continue;
                }

                if (i == 1)
                {
                    results.Add($"{Util.FormatCoeff(Coefficients[i])}{inner}");
                    continue;
                }

                results.Add($"{Util.FormatCoeff(Coefficients[i])}{inner}^{i}");
            }

            return String.Join(" + ", results);
        }
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

    public class RootNode : IContainer
    {
        public IExpression Parent
        {
            get => throw new NotImplementedException("Root expressions do not have a parent.");
            set => throw new NotImplementedException("Root expressions cannot have a parent.");
        }
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
        public IExpression Parent { get; set; }
        public Variable(string symbol)
        {
            Symbol = symbol;
        }
        public override string ToString() { return Symbol; }
        public T Accept<T>(IExpressionTreeVisitor<T> visitor) { return visitor.Visit(this); }
        public T Accept<T>(IExpressionMatchingVisitor<T> visitor, IExpression expression) { return visitor.Visit(this, expression); }
    }
}