using Microsoft.VisualStudio.TestTools.UnitTesting;
using UtilityLibraries;
using System.Diagnostics;

namespace ExpressionLibraryTest;

[TestClass]
public class ExpressionTreeVisitorTests
{
    [TestMethod]
    public void ExpressionTreeVisitorSumTest()
    {
        Dictionary<string, double> transformationMap = new Dictionary<string, double>();
        transformationMap["α"] = 7;
        
        var visitor = new EvaluationVisitor(transformationMap);

        var constant = new Constant(2.5);
        var variable = new Variable("α");

        var expression = new Sum(constant, variable);

        double sum = expression.Accept(visitor);

        Assert.AreEqual(9.5, sum, "Alpha should be a variable, and it should be the only one.");
    }
    [TestMethod]
    public void SimplicationVisitorSumTest()
    {
        var visitor = new SimplificationVisitor();

        var expression = new Sum(new Constant(2.5), new Constant(3));

        Debug.WriteLine(expression);

        IExpression result = visitor.Visit(expression);

        Debug.WriteLine(result);

        Assert.AreEqual("5.5", result.ToString(), "The sum of two constant expressions should be 1 constant having the numerical sum.");
    }
}