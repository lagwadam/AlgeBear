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
        ExpressionTypeEnum ExpressionType { get; }
        IExpression Parent { get; set; }
        void Accept(ISimpleExpressionVisitor visitor);
        T Accept<T>(IExpressionTreeVisitor<T> visitor);
        T Accept<T>(ITreeComparisonVisitor<T> visitor, IExpression expression);
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
