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

        var polynomial = new Polynomial(new Double[] { 1, 2 }, new Variable("x"));

        double value = polynomial.Accept(visitor);

        Assert.AreEqual(21, value, "Alpha should be a variable, and it should be the only one.");
    }

    [TestMethod]
    public void EvaluationVisitor_LogTest()
    {
        var transformationMap = new Dictionary<string, double> { { "x", 1 } };
        var visitor = new EvaluationVisitor(transformationMap);

        var ln = new Log(new Variable("x"));

        Assert.AreEqual(0, visitor.Visit(ln), "ln(1) should equal to 0");

        transformationMap["x"] = 2;
        Assert.AreEqual(0.6931, visitor.Visit(ln), .0001, "x should be a variable, and it should be the only one.");

        transformationMap["x"] = 2.718;
        Assert.AreEqual(1, visitor.Visit(ln), .001, "ln(e) should be 0");
    }

    [TestMethod]
    public void EvaluationVisitor_ExpTest()
    {
        var transformationMap = new Dictionary<string, double> { { "x", 0 } };
        var visitor = new EvaluationVisitor(transformationMap);

        var exp = new Exp(new Variable("x"));

        Assert.AreEqual(1, visitor.Visit(exp), "Exp(0) should equal to 1");

        transformationMap["x"] = 2;
        Assert.AreEqual(Math.Exp(2), visitor.Visit(exp), .001, "the actual value should be close to e^2");

        transformationMap["x"] = Math.Log(711);
        Assert.AreEqual(711, visitor.Visit(exp), .001, "e^ln(711) should be 7111");
    }


    [TestMethod]
    public void EvaluationVisitor_ExpExpTest()
    {
        var transformationMap = new Dictionary<string, double> { { "x", 0 } };
        var visitor = new EvaluationVisitor(transformationMap);

        // create expression for e^(e^x)
        Exp expression = new Exp(new Exp(new Variable("x")));

        Assert.AreEqual(Math.E, visitor.Visit(expression), "e^(e^0) should equal to e");

        transformationMap["x"] = 1;
        Assert.AreEqual(Math.Exp(Math.E), visitor.Visit(expression), "Exp(1) should equal to e^e");

        transformationMap["x"] = 2;
        Assert.AreEqual(Math.Exp(Math.Exp(2)), visitor.Visit(expression), .001, "the actual value should be close to e^2");

        transformationMap["x"] = Math.Log(11);
        Assert.AreEqual(Math.Exp(11), visitor.Visit(expression), .001, "ln(e) should be 1");
    }
}
