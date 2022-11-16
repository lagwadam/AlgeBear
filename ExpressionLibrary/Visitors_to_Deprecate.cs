using UtilityLibraries;

namespace UtilityLibraries
{
    public interface ISimpleExpressionVisitor 
    {
        void Visit(BinaryOperation expression);
        void Visit(Constant expression);
        void Visit(Container expression);
        void Visit(Polynomial expression);
        void Visit(RootNode expression);
        void Visit(Variable expression);
    }
    public class ExpressionVariablesVisitor : ISimpleExpressionVisitor
    {
        public ISet<string> Variables { get; private set; }
        
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

        public void Visit(BinaryOperation expression)
        {
            expression.Left.Accept(this);
            expression.Right.Accept(this);
        }

        public void Visit(Container expression)
        {
            expression.InnerExpression.Accept(this);
        }

        public void Visit(Polynomial expression)
        {
            expression.InnerExpression.Accept(this);
        }

        public void Visit(RootNode expression)
        {
            expression.InnerExpression.Accept(this);
        }
    }
}
