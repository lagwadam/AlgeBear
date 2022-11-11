using Microsoft.VisualStudio.TestTools.UnitTesting;
using UtilityLibraries;
using System.Diagnostics;

namespace StringLibraryTest;

[TestClass]
public class ExpressionVisitorsTest
{
    [TestMethod]
    public void ExpressionVariablesVisitorSumTest()
    {
        var visitor = new ExpressionVariablesVisitor();
        var constant = new Constant(2.5);
        var variable = new Variable("α");

        var expression = new Sum(constant, variable);

        expression.Accept(visitor);

        Assert.AreEqual("α", visitor.Variables.Single(), "Alpha should be a variable, and there should be one variable.");
    }

    [TestMethod]
    public void ExpressionVariablesVisitorSumAndProductTestTest()
    {
        var visitor = new ExpressionVariablesVisitor();
        var constant = new Constant(2.5);
        var variable = new Variable("α");
 
        // innerSum = α + 2(β + γ)
        var innerSum = new Sum(new Variable("α"), 
            new Product(new Constant(2), new Sum(new Variable("β"), new Variable("γ"))));

        // sum = 321 + δ(α + 2(β + γ))
        var sum = new Sum(new Constant(321), new Product(new Variable("δ"), innerSum));
        
        var expression = new Product(new Constant(3), sum);
        
        expression.Accept(visitor);

        var vars = visitor.Variables;

        Assert.AreEqual(4, visitor.Variables.Count);
        Assert.IsTrue(vars.Contains("α"));
        Assert.IsTrue(vars.Contains("β"));
        Assert.IsTrue(vars.Contains("γ"));
        Assert.IsTrue(vars.Contains("δ"));
    }
}