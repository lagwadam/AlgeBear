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
        void Accept(IExpressionVisitor visitor);
        bool Accept(ICompareVisitor visitor, IExpression expression);
        ExpressionTypeEnum ExpressionType { get; }
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

        public bool Accept(ICompareVisitor visitor, IExpression expression)
        {
            return visitor.Visit(this, expression);
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
        public bool Accept(ICompareVisitor visitor, IExpression expression)
        {
            return visitor.Visit(this, expression);
        }
    }

    public interface IBinaryOperation : IComposite
    {
        IExpression Left { get; }
        IExpression Right { get; }
        string Operation { get; }
    }

public abstract class BinaryOperation : IBinaryOperation
    {
        public IExpression Left { get; private set; }
        public IExpression Right { get; private set; }
        public abstract ExpressionTypeEnum ExpressionType { get; }
        public abstract string Operation { get; } 

        public BinaryOperation(IExpression left, IExpression right)
        {
            Left = left;
            Right = right;
        }

        public void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public virtual bool Accept(ICompareVisitor visitor, IExpression source)
        {
            return visitor.Visit(this, source);
        }
        
        public override string ToString()
        {
            return $"({Left.ToString()}{Operation}{Right.ToString()})";
        }
    }

    public class Sum : BinaryOperation
    {
        public override ExpressionTypeEnum ExpressionType => ExpressionTypeEnum.Sum;
        public override string Operation => " + ";
        public Sum(IExpression left, IExpression right) : base(left, right) {}
        public override bool Accept(ICompareVisitor visitor, IExpression source) { return visitor.Visit(this, source); }
    }

    public class Difference : BinaryOperation
    {
        public override ExpressionTypeEnum ExpressionType => ExpressionTypeEnum.Difference;
        public override string Operation => " - ";
        public Difference(IExpression left, IExpression right) : base(left, right) {}

        public override bool Accept(ICompareVisitor visitor, IExpression source) { return visitor.Visit(this, source); }
    }

    public class Product : BinaryOperation
    {
        public override ExpressionTypeEnum ExpressionType => ExpressionTypeEnum.Product;     
        public override string Operation => "*";
        public Product(IExpression left, IExpression right) : base(left, right) {}
        public override bool Accept(ICompareVisitor visitor, IExpression source) { return visitor.Visit(this, source); }
    }

    public class Quotient : BinaryOperation
    {
        public override ExpressionTypeEnum ExpressionType => ExpressionTypeEnum.Quotient;
        public override string Operation => " / ";
        public Quotient(IExpression left, IExpression right) : base(left, right) {}
        public override bool Accept(ICompareVisitor visitor, IExpression source) { return visitor.Visit(this, source); }        
    }

    public class Power : BinaryOperation
    {
        public override ExpressionTypeEnum ExpressionType => ExpressionTypeEnum.Power;
        public override string Operation => "^";
        public Power(IExpression left, IExpression right) : base(left, right) {}
        public override bool Accept(ICompareVisitor visitor, IExpression source) { return visitor.Visit(this, source); }
    }
}
