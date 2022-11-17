namespace UtilityLibraries
{
    public interface IExpressionTreeVisitor<TReturn>
    {
        TReturn Visit(Constant expression);
        TReturn Visit(Container expression);
        TReturn Visit(Quotient expression);
        TReturn Visit(Polynomial expression);
        TReturn Visit(Power expression);
        TReturn Visit(Product expression);
        TReturn Visit(RootNode expression);
        TReturn Visit(Sum expression);
        TReturn Visit(Variable expression);
    }
}
