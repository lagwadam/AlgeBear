namespace UtilityLibraries
{
    public interface IPrimative : IExpression
    {
    }

    public interface IComposite : IExpression
    {
    }
    
    public interface IExpression
    {
        public void Accept(IExpressionVisitor v);
    }

    public class Constant : IPrimative
    {
        public double Val { get; private set; }
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

    public class Sum : IExpression
    {
        public IExpression Left { get; private set; }
        public IExpression Right { get; private set; }

        public Sum(IExpression left, IExpression right)
        {
            Left = left;
            Right = right;
        }

        public void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return $"({Left.ToString()} + {Right.ToString()})";
        }
    }

    public class Difference : IExpression
    {
        public IExpression Left { get; private set; }
        public IExpression Right { get; private set; }

        public Difference(IExpression left, IExpression right)
        {
            Left = left;
            Right = right;
        }

        public override string ToString()
        {
            return $"({Left.ToString()} - {Right.ToString()})";
        }

        public void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class Product : IExpression
    {
        public IExpression Left { get; private set; }
        public IExpression Right { get; private set; }

        public Product(IExpression left, IExpression right)
        {
            Left = left;
            Right = right;
        }

        public override string ToString()
        {
            return $"{Left.ToString()}*{Right.ToString()}";
        }

        public void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }


    public class Quotient : IExpression
    {
        public IExpression Dividend { get; private set; }
        public IExpression Divisor { get; private set; }

        public Quotient(IExpression dividend, IExpression divisor)
        {
            Dividend = dividend;
            Divisor = divisor;
        }

        public override string ToString()
        {
            return $"({Dividend.ToString()} / {Divisor.ToString()})";
        }

        public void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class Power : IExpression
    {
        public IExpression Radix { get; private set; }
        public IExpression Exponent { get; private set; }

        public Power(IExpression radix, IExpression exponent)
        {
            Radix = radix;
            Exponent = exponent;
        }

        public override string ToString()
        {
            var radixString = Radix.ToString();
            if (Radix is not IPrimative)
                radixString = $"({radixString})";
            var exponentString = Exponent.ToString();
            if (Exponent is not IPrimative)
                exponentString = $"({exponentString})";
            return $"{radixString}^{exponentString}";
        }

        public void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
