using Microsoft.VisualStudio.TestTools.UnitTesting;
using UtilityLibraries;
using System.Diagnostics;

namespace ExpressionLibraryTest;

[TestClass]
public class DifferentiationVisitorTests
{
    [TestMethod]
    public void DifferentiationVisitor_PolyTest()
    {
        var visitor = new DifferentiationVisitor();

        var expression = new Polynomial(new Double[] {1, 2, 3, 4, 5, 6, 7, 8}, new Variable("x"));
        var root = new RootNode(expression);
        Debug.WriteLine(root);
        new DifferentiationVisitor().Visit(root);
        Debug.WriteLine(root);

        Assert.AreEqual("2 + 6x + 12x^2 + 20x^3 + 30x^4 + 42x^5 + 56x^6", root.ToString(), "Derivative should be 1 + 2x");
    }

    [TestMethod]
    public void DifferentiationVisitor_Sum_With_Auto_SimplificationTest()
    {
        var visitor = new DifferentiationVisitor();

        var leftPoly = new Polynomial(new Double[] {1, 1, 1}, new Variable("x"));
        var rightPoly = new Polynomial(new Double[] {1, -1, 1}, new Variable("x"));
        
        var root = new RootNode(new Sum(leftPoly, rightPoly));

        Debug.WriteLine(root);
        new DifferentiationVisitor().Visit(root);

        // var simplifier = new SimplificationVisitor();
        // simplifier.Visit(root);
        Debug.WriteLine(root);

        Assert.AreEqual("4x", root.ToString(), "Derivative of (2 + 2x^2) should be 4x");
    }

    [TestMethod]
    public void DifferentiationVisitor_Product_With_Linear_and_Constant()
    {
        var visitor = new DifferentiationVisitor();
        var leftPoly = new Polynomial(new Double[] {1, 2}, new Variable("x"));
        var rightPoly = new Polynomial(new Double[] {2}, new Variable("x"));

        var root = new RootNode(new Product(leftPoly, rightPoly));

        Debug.WriteLine(root);
        new DifferentiationVisitor().Visit(root);

        var simplifier = new SimplificationVisitor();
        simplifier.Visit(root);
        Debug.WriteLine(root);

        Assert.AreEqual("4", root.ToString(), "Derivative of 2 + 4x should be 4");
    }

    [TestMethod]
    public void DifferentiationVisitor_Product_With_Auto_SimplificationTest()
    {
        var visitor = new DifferentiationVisitor();
        // p(x) = (1 + 2x) * (2 + 3x) = 2 + 7x + 6x^2
        // d/dx = 3(1+2x) + 2 * (2 +3x) = 3 + 6x + 4 + 6x 
        var leftPoly = new Polynomial(new Double[] {1, 2}, new Variable("x"));
        var rightPoly = new Polynomial(new Double[] {2, 3}, new Variable("x"));

        var root = new RootNode(new Product(leftPoly, rightPoly));

        Debug.WriteLine(root);
        new DifferentiationVisitor().Visit(root);

        Debug.WriteLine(root);

        Assert.AreEqual("7 + 12x", root.ToString(), "Derivative should be 7 + 12x");
    }
}