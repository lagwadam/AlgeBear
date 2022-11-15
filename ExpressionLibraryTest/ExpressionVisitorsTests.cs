using Microsoft.VisualStudio.TestTools.UnitTesting;
using UtilityLibraries;
using System.Diagnostics;

namespace ExpressionLibraryTest;

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

        var actualVariables = visitor.Variables;
        // Debug.WriteLine();
        Debug.WriteLine(actualVariables);
        Debug.WriteLine(expression.ToString());

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

        Debug.WriteLine(expression.ToString());

        var vars = visitor.Variables;

        Assert.AreEqual(4, visitor.Variables.Count);
        Assert.IsTrue(vars.Contains("α"));
        Assert.IsTrue(vars.Contains("β"));
        Assert.IsTrue(vars.Contains("γ"));
        Assert.IsTrue(vars.Contains("δ"));
    }

    [TestMethod]
    public void CompareVisitorSumTest()
    {
        var visitor = new EquivalencyVisitor();

        var expression = new Sum(new Constant(2.5), new Variable("α"));
        
        var matched = new Sum(new Constant(2.5), new Constant(1.99));

        bool isValid = visitor.Visit(expression, matched);
        foreach(string transformation in visitor.Transformations)
        {
            Debug.WriteLine(transformation);
        }

        Assert.IsTrue(isValid);
        Assert.AreEqual("α ↦ 1.99", visitor.Transformations.Single(), "There should be one variable with alpha goes to 2.5");
    }

    [TestMethod]
    public void CompareVisitorSumTest_Fails_When_Match_Differs()
    {
        var visitor = new EquivalencyVisitor();
        
        var expression = new Sum(new Constant(2.5), new Variable("α"));
        
        var matched = new Sum(new Constant(4), new Constant(3.5));

        bool isValid = visitor.Visit(expression, matched);
        Assert.IsFalse(isValid);
    }

    public void CompareVisitorSumTest_Fails_When_Target_constant_needs_to_be_a_variable()
    {
        var visitor = new EquivalencyVisitor();
        
        var expression = new Sum(new Constant(4), new Constant(3.5));
        var matched = new Sum(new Constant(2.5), new Variable("α"));
        bool isValid = visitor.Visit(expression, matched);
        Assert.IsFalse(isValid);
    }

        [TestMethod]
    public void CompareVisitorProductSumTest()
    {
        var visitor = new EquivalencyVisitor();
        var constant = new Constant(2.5);
        var variable = new Variable("α");

        var left = new Sum(new Constant(2.5), new Variable("α"));
        var right = new Sum(new Constant(7), new Variable("β"));

        var matchedLeft = new Sum(new Constant(2.5), new Constant(2.5));
        var matchedRight = new Sum(new Constant(7), new Constant(7));

        var expression = new Product(left, right);
        var matched = new Product(matchedLeft, matchedRight);

        bool isValid = visitor.Visit(expression, matched);
        foreach(string transformation in visitor.Transformations)
        {
            Debug.WriteLine(transformation);
        }

        Assert.IsTrue(isValid);
        Assert.AreEqual("α ↦ 2.5", visitor.Transformations.First(), "First transformation needs alpha goes to 2.5");
        Assert.AreEqual("β ↦ 7", visitor.Transformations.Last(), "The other transformation needs beta goes to 2.5");
    }
}