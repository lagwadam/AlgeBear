namespace UtilityLibraries
{
    public interface IPrimative : IExpression
    {
    }

    public interface IComposite : IExpression
    {
    }

    public enum ExpressionTypeEnum
    {
        Constant,
        Difference,
        Power,
        Product,
        Quotient,
        Sum,
        Variable
    }

    public interface IExpression
    {
        void Accept(IExpressionVisitor v);
        ExpressionTypeEnum ExpressionType { get; }
    }

    public interface IBinaryOperation : IComposite
    {
        IExpression Left { get; }
        IExpression Right { get; }
    }

    public abstract class BinaryOperation : IBinaryOperation
    {
        public IExpression Left { get; private set; }
        public IExpression Right { get; private set; }

        public abstract ExpressionTypeEnum ExpressionType { get; }

        public BinaryOperation(IExpression left, IExpression right)
        {
            Left = left;
            Right = right;
        }

        public void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void Accept(ICompareVisitor visitor, IExpression source)
        {
            // visitor.Visit(this, source);
        }

        public override string ToString()
        {
            return $"({Left.ToString()}, {Right.ToString()})";
        }
    }

    public class Constant : IPrimative
    {

        public double Val { get; private set; }

        public ExpressionTypeEnum ExpressionType => ExpressionTypeEnum.Constant;

        public Constant(double val)
        {
            Val = val;
        }

        public void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return Val.ToString();
        }
    }

    public class Variable : IPrimative
    {
        public string Symbol { get; private set; }
        public ExpressionTypeEnum ExpressionType => ExpressionTypeEnum.Variable;
        public Variable(string symbol)
        {
            Symbol = symbol;
        }
        public override string ToString()
        {
            return Symbol;
        }
        public void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class Sum : BinaryOperation
    {
        public override ExpressionTypeEnum ExpressionType => ExpressionTypeEnum.Sum;
        public Sum(IExpression left, IExpression right) : base(left, right)
        {
        }
        public override string ToString()
        {
            return $"({Left.ToString()} + {Right.ToString()})";
        }
    }

    public class Difference : BinaryOperation
    {
        public override ExpressionTypeEnum ExpressionType => ExpressionTypeEnum.Difference;
        public Difference(IExpression left, IExpression right) : base(left, right)
        {
        }
        public override string ToString()
        {
            return $"({Left.ToString()} - {Right.ToString()})";
        }
    }

    public class Product : BinaryOperation
    {
        public override ExpressionTypeEnum ExpressionType => ExpressionTypeEnum.Product;
        public Product(IExpression left, IExpression right) : base(left, right)
        {
        }

        public override string ToString()
        {
            return $"({Left.ToString()}*{Right.ToString()})";
        }
    }

    public class Quotient : BinaryOperation
    {
        public override ExpressionTypeEnum ExpressionType => ExpressionTypeEnum.Quotient;
        public Quotient(IExpression left, IExpression right) : base(left, right)
        {
        }
        public override string ToString()
        {
            return $"({Left.ToString()} / {Right.ToString()})";
        }
    }

    public class Power : BinaryOperation
    {
        public override ExpressionTypeEnum ExpressionType => ExpressionTypeEnum.Power;
        public Power(IExpression left, IExpression right) : base(left, right)
        {
        }
        public override string ToString()
        {
            return $"({Left.ToString()}^{Right.ToString()})";
        }
    }
}
