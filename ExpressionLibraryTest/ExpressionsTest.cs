using Microsoft.VisualStudio.TestTools.UnitTesting;
using UtilityLibraries;
using System.Diagnostics;

namespace ExpressionLibraryTest;

[TestClass]
public class ExpressionTest
{
    [TestMethod]
    public void MatchSumTest()
    {
        // TODO: Get this passing

        // var constant = new Constant(2.5);
        // var variable = new Variable("α");

        // var expression = new Sum(constant, variable);   
        // var candidate = new Sum(constant, new Constant(2.5));
        // Debug.WriteLine($"Expression: {expression.ToString()}");
        
        // var areEqual = true;
        // Assert.IsTrue(areEqual);
    }

    [TestMethod]
    public void ConstantTest()
    {
        var constant = new Constant(3);

        Debug.WriteLine($"Constant: {constant.ToString()}");
        Assert.AreEqual("3", constant.ToString());
    }
    
    [TestMethod]
    public void SumTest()
    {
        var constant = new Constant(2.5);
        var variable = new Variable("α");

        var sum = new Sum(constant, variable);   

        Debug.WriteLine($"Sum: {sum.ToString()}");     
        Assert.AreEqual("(2.5 + α)", sum.ToString());
    }

    [TestMethod]
    public void ProductTest()
    {
        var constant = new Constant(2.5);
        var variable = new Variable("α");
        var product = new Product(constant, variable); 

        Debug.WriteLine($"Product: {product.ToString()}");       
        Assert.AreEqual("2.5*α", product.ToString());
    }

    [TestMethod]
    public void SumAndProductTest()
    {
        var constant1 = new Constant(2);
        var constant2 = new Constant(11);
        var variable = new Variable("α");
        var sum = new Sum(constant2, variable);
        var product = new Product(constant1, sum);

        Debug.WriteLine(product.ToString());
        Assert.AreEqual("2*(11 + α)", product.ToString());
    }

    [TestMethod]
    public void PrimativePowerTest()
    {
        var x = new Variable("x");
        var c = new Constant(3);
        
        var power = new Power(x, c);

        Debug.WriteLine(power.ToString());
        Assert.AreEqual("x^3", power.ToString());
    }

    [TestMethod]
    public void CompositePowerTest()
    {
        var x = new Variable("x");        
        var c = new Constant(3);
        var sum = new Sum(x, c);
        var product = new Product(c, x);

        var power = new Power(sum, product);

        Debug.WriteLine(power.ToString());
        Assert.AreEqual("((x + 3))^(3*x)", power.ToString());
    }

    [TestMethod]
    public void PowerSumAndProductTest()
    {
        var sum = new Sum(new Constant(11), new Variable("α"));
        var product = new Product(new Constant(2), sum);
        var power = new Power(product, new Constant(3));
        Debug.WriteLine(power.ToString());
        Assert.AreEqual("(2*(11 + α))^3", power.ToString());
    }
}