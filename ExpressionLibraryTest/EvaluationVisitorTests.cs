using Microsoft.VisualStudio.TestTools.UnitTesting;
using UtilityLibraries;
using System.Diagnostics;

namespace ExpressionLibraryTest;

[TestClass]
public class EvaluationVisitorTests
{
    [TestMethod]
    public void EvaluationVisitor_SumTest()
    {
        var transformationMap = new Dictionary<string, double> { { "α", 7 } };

        var visitor = new EvaluationVisitor(transformationMap);

        var constant = new Constant(2.5);
        var variable = new Variable("α");

        var expression = new Sum(constant, variable);

        double sum = expression.Accept(visitor);

        Assert.AreEqual(9.5, sum, "Alpha should be a variable, and it should be the only one.");
    }

    [TestMethod]
    public void EvaluationVisitor_PolynomialTest()
    {
        var transformationMap = new Dictionary<string, double> { { "x", 10 } };

        var visitor = new EvaluationVisitor(transformationMap);

        var polynomial = new Polynomial(new Double[] {1, 2}, new Variable("x"));

        double value = polynomial.Accept(visitor);

        Assert.AreEqual(21, value, "Alpha should be a variable, and it should be the only one.");
    }

    [TestMethod]
    public void EvaluationVisitor_LogTest()
    {
        var transformationMap = new Dictionary<string, double> { { "x", 1 } };
        var visitor = new EvaluationVisitor(transformationMap);

        var ln = new ln(new Variable("x"));

        Assert.AreEqual(0, visitor.Visit(ln), "ln(1) should equal 0");

        transformationMap["x"] = 2;
        Assert.AreEqual(0.6931, visitor.Visit(ln), .0001,"Alpha should be a variable, and it should be the only one.");

        transformationMap["x"] = 2.718;
        Assert.AreEqual(1, visitor.Visit(ln), .001, "ln(e) should be 0");
    }
}
