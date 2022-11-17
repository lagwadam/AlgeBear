using Microsoft.VisualStudio.TestTools.UnitTesting;
using UtilityLibraries;
using System.Diagnostics;

namespace ExpressionLibraryTest;

[TestClass]
public class EvaluationVisitorTests
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
}