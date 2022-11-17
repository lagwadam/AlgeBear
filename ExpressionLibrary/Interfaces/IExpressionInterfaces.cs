namespace UtilityLibraries
{
    // Used as a way to distinguish simple expressions like Constant and Variable from composite expressions like sum, product, and power.
    public interface IPrimative : IExpression
    {
    }

    // Used as a way to distinguish Composite expressions from Primative expressions like Constant and Variable.
    public interface IComposite : IExpression
    {
    }
    
    public interface IExpression
    {
        ExpressionTypeEnum ExpressionType { get; }
        IExpression Parent { get; set; }
        T Accept<T>(IExpressionTreeVisitor<T> visitor);
        T Accept<T>(IExpressionMatchingVisitor<T> visitor, IExpression expression);
    }

    public interface IContainer : IExpression
    {
        IExpression InnerExpression { get; }
    }

    public interface IBinaryOperation : IComposite
    {
        IExpression Left { get; }
        IExpression Right { get; }
        string Operation { get; }
    }
}
