using Microsoft.VisualStudio.TestTools.UnitTesting;
using UtilityLibraries;
using System.Diagnostics;

namespace ExpressionLibraryTest;

[TestClass]
public class ExpressionMatchingVisitorTests
{
    [TestMethod]
    public void ExpressionMatchingVisitor_SumTest()
    {
        var visitor = new ExpressionMatchingVisitor();

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
    public void ExpressionMatchingVisitor_Sum_Fails_When_Match_Differs()
    {
        var visitor = new ExpressionMatchingVisitor();
        
        var expression = new Sum(new Constant(2.5), new Variable("α"));
        
        var matched = new Sum(new Constant(4), new Constant(3.5));

        bool isValid = visitor.Visit(expression, matched);
        Assert.IsFalse(isValid);
    }

    public void CompareVisitorSumTest_Fails_When_Target_constant_needs_to_be_a_variable()
    {
        var visitor = new ExpressionMatchingVisitor();
        
        var expression = new Sum(new Constant(4), new Constant(3.5));
        var matched = new Sum(new Constant(2.5), new Variable("α"));
        bool isValid = visitor.Visit(expression, matched);
        Assert.IsFalse(isValid);
    }

        [TestMethod]
    public void CompareVisitorProductSumTest()
    {
        var visitor = new ExpressionMatchingVisitor();
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