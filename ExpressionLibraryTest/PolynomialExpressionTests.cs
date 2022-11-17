using Microsoft.VisualStudio.TestTools.UnitTesting;
using UtilityLibraries;
using System.Diagnostics;

namespace ExpressionLibraryTest;

[TestClass]
public class PolynomialExpressionTests
{
    [TestMethod]
    public void Zero_Polynomial_ToString_Test()
    {
        Polynomial zero = new Polynomial(new Double[] {0}, new Variable("x"));
        var result = zero.ToString();
        Debug.WriteLine(result);
        Assert.AreEqual(string.Empty, result);
    }

    [TestMethod]
    public void Constant_Polynomial_ToString_Test()
    {
        Polynomial zero = new Polynomial(new Double[] {1.1}, new Variable("x"));
        var result = zero.ToString();
        Debug.WriteLine(result);
        Assert.AreEqual("1.1", result);
    }
    
    [TestMethod]
    public void Quadratic_Polynomial_ToString_Test()
    {
        Polynomial zero = new Polynomial(new Double[] {-2, 3, 4}, new Variable("x"));
        var result = zero.ToString();
        Debug.WriteLine(result);
        Assert.AreEqual("-2 + 3x + 4x^2", result);
    }

    [TestMethod]
    public void Cubic_Polynomial_with_inner_expression_ToString_Test()
    {
        var product = new Product(new Constant(2), new Variable("x"));
        var innerExpression = new Sum(product, new Constant(1));

        Polynomial zero = new Polynomial(new Double[] {1, 3, 0, 7}, innerExpression);
        var result = zero.ToString();
        Debug.WriteLine($"result: {result}");
        Debug.WriteLine($"expect: {"1 + 3(((2*x)) + 1) + 7(((2*x)) + 1)^3"}");
        Assert.AreEqual("1 + 3(((2*x)) + 1) + 7(((2*x)) + 1)^3", result);
    }
}