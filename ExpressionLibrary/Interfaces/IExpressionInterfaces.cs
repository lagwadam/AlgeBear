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
        T Accept<T>(IExpressionTreeVisitor<T> visitor);
        T Accept<T>(IExpressionMatchingVisitor<T> visitor, IExpression expression);
    }

    public interface IBinaryOperation : IComposite
    {
        IExpression Left { get; }
        IExpression Right { get; }
        string Operation { get; }
    }

    public interface IFunction : IComposite
    {
        string Name { get; }
        IExpression Argument { get; }
        ExpressionTypeEnum InverseFunctionType { get; }
    }
}
