namespace UtilityLibraries
{
    public interface IExpressionMatchingVisitor<TReturn>
    {
        TReturn Visit(Constant expression, IExpression source);
        TReturn Visit(Container expression, IExpression source);
        TReturn Visit(BinaryOperation expression, IExpression source);
        TReturn Visit(Polynomial expression, IExpression source);
        TReturn Visit(RootNode expression, IExpression source);
        TReturn Visit(Variable expression, IExpression source);
    }
}
