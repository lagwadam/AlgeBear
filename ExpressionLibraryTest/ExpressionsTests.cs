using Microsoft.VisualStudio.TestTools.UnitTesting;
using UtilityLibraries;
using System.Diagnostics;

namespace ExpressionLibraryTests;

[TestClass]
public class ExpressionTest
{
    [TestMethod]
    public void Constant_Test()
    {
        var constant = new Constant(3);

        Debug.WriteLine($"Constant: {constant.ToString()}");
        Assert.AreEqual("3", constant.ToString());
    }
    
    [TestMethod]
    public void Sum_Test()
    {
        var constant = new Constant(2.5);
        var innerSum = new Sum(new Variable("α"), new Constant(3.5));

        var sum = new Sum(constant, innerSum);   

        Debug.WriteLine($"Sum: {sum.ToString()}");     
        Assert.AreEqual("(2.5 + ((α + 3.5)))", sum.ToString());
    }

    [TestMethod]
    public void Product_Test()
    {
        var constant = new Constant(2.5);
        var variable = new Variable("α");
        var product = new Product(constant, variable); 

        Debug.WriteLine($"Product: {product.ToString()}");       
        Assert.AreEqual("(2.5*α)", product.ToString());
    }

    [TestMethod]
    public void SumAndProduct_Test()
    {
        var constant1 = new Constant(2);
        var constant2 = new Constant(11);
        var variable = new Variable("α");
        var sum = new Sum(constant2, variable);
        var product = new Product(constant1, sum);

        Debug.WriteLine(product.ToString());
        Assert.AreEqual("(2*((11 + α)))", product.ToString());
    }

    [TestMethod]
    public void PrimativePower_Test()
    {
        var x = new Variable("x");
        var c = new Constant(3);
        
        var power = new Power(x, c);

        Debug.WriteLine(power.ToString());
        Assert.AreEqual("(x^3)", power.ToString());
    }

    [TestMethod]
    public void CompositePower_Test()
    {
        var x = new Variable("x");        
        var c = new Constant(3);
        var sum = new Sum(x, c);
        var product = new Product(c, x);

        var power = new Power(sum, product);

        Debug.WriteLine(power.ToString());
        Assert.AreEqual("(((x + 3))^((3*x)))", power.ToString());
    }

    [TestMethod]
    public void PowerSumAndProduct_Test()
    {
        var sum = new Sum(new Constant(11), new Variable("α"));
        var product = new Product(new Constant(2), sum);
        var power = new Power(product, new Constant(3));
        Debug.WriteLine(power.ToString());
        Assert.AreEqual("(((2*((11 + α))))^3)", power.ToString());
    }

    [TestMethod]
    public void Quotient_Test()
    {
        var quotient = new Quotient(new Constant(11), new Variable("x"));
        
        Debug.WriteLine(quotient.ToString());

        Assert.AreEqual("(11 / x)", quotient.ToString());
    }
    
    [TestMethod]
    public void RationalFunction_Test()
    {
        var numeratorCoeffs = new Double[] {1, 2};
        var denominatorCoeffs = new Double[] {2, -1};
        var numerator = new Polynomial(numeratorCoeffs, new Variable("x"));
        var denominator = new Polynomial(denominatorCoeffs, new Variable("x"));
        var quotient = new Quotient(numerator, denominator);
        
        Debug.WriteLine(quotient.ToString());

        Assert.AreEqual("((1 + 2x) / (2 + -1x))", quotient.ToString());
    }

    [TestMethod]
    public void PowerOfPOwerTestFunction_Test()
    {
        var poly1 = new Polynomial(new Double[] {0, 1}, new Variable("x"));
        var poly2 = new Polynomial(new Double[] {0, 1}, new Variable("x"));
        var poly3 = new Polynomial(new Double[] {0, 1}, new Variable("x"));

        var power2 = new Power(poly2, poly3);
        var power1 = new Power(poly1, power2);

        Debug.WriteLine($"power2: {power2}");
        Debug.WriteLine($"power1: {power1}");

        Assert.AreEqual("((x)^(((x)^(x))))", power1.ToString());
    } 

    [TestMethod]
    public void PowerOfPower_WithComposition_Test()
    {
        var poly1 = new Polynomial(new Double[] {1, 1}, new Variable("x"));
        var poly2 = new Polynomial(new Double[] {1, 1}, new Variable("x"));
        var poly3 = new Polynomial(new Double[] {1, 1}, new Variable("x"));

        var power2 = new Power(poly2, poly3);
        var power1 = new Power(poly1, power2);

        Debug.WriteLine($"power2: {power2}");
        Debug.WriteLine($"power1: {power1}");

        Assert.AreEqual("((1 + x)^(((1 + x)^(1 + x))))", power1.ToString());
    } 

}