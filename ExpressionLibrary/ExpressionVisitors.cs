namespace UtilityLibraries
{
    public interface IExpressionVisitor
    {
        void Visit(Constant expression);
        void Visit(Variable expression);
        void Visit(Sum expression);
        void Visit(Difference expression);
        void Visit(Product expression);
        void Visit(Quotient expression);
        void Visit(Power expression);
    }

    public class ExpressionVariablesVisitor : IExpressionVisitor
    {
        public HashSet<string> Variables { get; private set; }
        
        public ExpressionVariablesVisitor()
        {
            Variables = new HashSet<string>();
        }
        public void Visit(Constant expression)
        {
            // No vars to add; no expressions to drill down into
        }

        public void Visit(Variable expression)
        {
            Variables.Add(expression.Symbol);
        }

        public void Visit(Sum expression)
        {
            expression.Left.Accept(this);
            expression.Right.Accept(this);
        }

        public void Visit(Difference expression)
        {
            expression.Left.Accept(this);
            expression.Right.Accept(this);
        }

        public void Visit(Product expression)
        {
            expression.Left.Accept(this);
            expression.Right.Accept(this);
        }

        public void Visit(Quotient expression)
        {
            expression.Dividend.Accept(this);
            expression.Divisor.Accept(this);
        }

        public void Visit(Power expression)
        {
            expression.Radix.Accept(this);
            expression.Exponent.Accept(this);
        }
    }
}
