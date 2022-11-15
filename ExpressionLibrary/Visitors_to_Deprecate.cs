using UtilityLibraries;

namespace UtilityLibraries
{
    public interface ISimpleExpressionVisitor 
    {
        void Visit(Constant expression);
        void Visit(Variable expression);
        void Visit(BinaryOperation expression);
        void Visit(Container expression);
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

        public void Visit(Container container)
        {
            container.Expression.Accept(this);
        }
    }
}
