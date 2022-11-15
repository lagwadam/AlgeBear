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
        Container,
        Difference,
        Power,
        Product,
        Quotient,
        Sum,
        Variable,
    }

    public interface IExpression
    {
        void Accept(ISimpleExpressionVisitor visitor);
        T Accept<T>(IExpressionTreeVisitor<T> visitor);
        T Accept<T>(ITreeComparisonVisitor<T> visitor, IExpression expression);
        ExpressionTypeEnum ExpressionType { get; }
    }

    public class Constant : IPrimative
    {
        public double Value { get; private set; }

        public ExpressionTypeEnum ExpressionType => ExpressionTypeEnum.Constant;

        public Constant(double val)
        {
            Value = val;
        }

        public void Accept(ISimpleExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public T Accept<T>(IExpressionTreeVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public T Accept<T>(ITreeComparisonVisitor<T> visitor, IExpression expression)
        {
            return visitor.Visit(this, expression);
        }
    }

    public class Variable : IPrimative
    {
        public string Symbol { get; private set; }
        public ExpressionTypeEnum ExpressionType => ExpressionTypeEnum.Variable;
        public Variable(string symbol) { Symbol = symbol; }
        public override string ToString() { return Symbol; }
        public void Accept(ISimpleExpressionVisitor visitor) { visitor.Visit(this); }
        public T Accept<T>(IExpressionTreeVisitor<T> visitor) { return visitor.Visit(this); }
        public T Accept<T>(ITreeComparisonVisitor<T> visitor, IExpression expression) { return visitor.Visit(this, expression); }
    }

    public interface IContainer : IExpression
    {
        IExpression Expression { get; }
    }

    public class Container : IContainer
    {
        public IExpression Expression { get; private set; }
        public ExpressionTypeEnum ExpressionType => ExpressionTypeEnum.Container;
        public Container(IExpression expression) 
        { 
            Expression = expression; 
        }
        public void Accept(ISimpleExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
        public virtual T Accept<T>(IExpressionTreeVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
        public virtual T Accept<T>(ITreeComparisonVisitor<T> visitor, IExpression source)
        {
            return visitor.Visit(this, source);
        }
        public override string ToString()
        {
            return $"({Expression.ToString()})";
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
        public void Accept(ISimpleExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
        public abstract T Accept<T>(IExpressionTreeVisitor<T> visitor);
        public virtual T Accept<T>(ITreeComparisonVisitor<T> visitor, IExpression source)
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
        public Sum(IExpression left, IExpression right) : base(left, right) { }
        public override T Accept<T>(IExpressionTreeVisitor<T> visitor) 
        { 
            return visitor.Visit(this); 
        }
        public override T Accept<T>(ITreeComparisonVisitor<T> visitor, IExpression source) { return visitor.Visit(this, source); }
    }

    public class Difference : BinaryOperation
    {
        public override ExpressionTypeEnum ExpressionType => ExpressionTypeEnum.Difference;
        public override string Operation => " - ";
        public Difference(IExpression left, IExpression right) : base(left, right) { }
        public override T Accept<T>(IExpressionTreeVisitor<T> visitor) 
        { 
            return visitor.Visit(this); 
        }
        public override T Accept<T>(ITreeComparisonVisitor<T> visitor, IExpression source) { return visitor.Visit(this, source); }
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
        public override T Accept<T>(ITreeComparisonVisitor<T> visitor, IExpression source) { return visitor.Visit(this, source); }
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
        public override T Accept<T>(ITreeComparisonVisitor<T> visitor, IExpression source) { return visitor.Visit(this, source); }
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
        public override T Accept<T>(ITreeComparisonVisitor<T> visitor, IExpression source) 
        { 
            return visitor.Visit(this, source); 
        }
    }


    // public abstract class UnaryOperation : IUnaryOperation
    //     {
    //         public IExpression Value { get; private set; }
    //         public IExpression Right { get; private set; }
    //         public abstract ExpressionTypeEnum ExpressionType { get; }
    //         public abstract string Operation { get; } 

    //         public BinaryOperation(IExpression left, IExpression right)
    //         {
    //             Left = left;
    //             Right = right;
    //         }

    //         public void Accept(IExpressionVisitor visitor)
    //         {
    //             visitor.Visit(this);
    //         }

    //         public virtual bool Accept(ICompareVisitor visitor, IExpression source)
    //         {
    //             return visitor.Visit(this, source);
    //         }

    //         public override string ToString()
    //         {
    //             return $"({Left.ToString()}{Operation}{Right.ToString()})";
    //         }
    //     }


}
