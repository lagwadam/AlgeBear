using Microsoft.VisualStudio.TestTools.UnitTesting;
using UtilityLibraries;
using System.Diagnostics;

namespace ExpressionLibraryTest;

[TestClass]
public class SimplificationVisitorTests
{
    [TestMethod]
    public void SimplicationVisitor_SumTest()
    {
        var visitor = new SimplificationVisitor();

        var expression = new Sum(new Constant(2.5), new Constant(3));

        var root = new RootNode(expression);
        Debug.WriteLine(expression);
        new SimplificationVisitor().Visit(root);
        Debug.WriteLine(root);

        Assert.AreEqual("5.5", root.ToString(), "The sum of two constant expressions should be 1 constant having the numerical sum.");
    }

    [TestMethod]
    public void SimplicationVisitor_SumAndProductTest()
    {
        var visitor = new SimplificationVisitor();

        var product = new Product(new Constant(2), new Constant(3));
        var expression = new Sum(new Constant(4), product);
        var rootNode = new RootNode(expression);
        
        Debug.WriteLine(rootNode);
        visitor.Visit(rootNode);
        Debug.WriteLine(rootNode);

        Assert.AreEqual("(4 + 6)", rootNode.ToString(), "The product should have broken down into a single constant.");

        visitor.Visit(rootNode);
        Debug.WriteLine(rootNode);
        
        Assert.AreEqual("10", rootNode.ToString(), "The sum of two constant expressions should be 1 constant having the numerical sum.");
    }

    [TestMethod]
    public void SimplicationVisitor_ZeroSumWithProductTest()
    {
        var visitor = new SimplificationVisitor();

        var product = new Product(new Constant(2), new Constant(3));
        var expression = new Sum(new Constant(0), product);
        var rootNode = new RootNode(expression);
        
        visitor.Visit(rootNode);

        Assert.AreEqual("6", rootNode.ToString(), "Since the left sum is zero, the simplified right expression should be returned.");
        Debug.WriteLine($"rootNode: {rootNode.ToString}");
    }

    [TestMethod]
    public void SimplicationVisitor_ZeroSumWithZeroProductTest()
    {
        var visitor = new SimplificationVisitor();

        var product = new Product(new Constant(2), new Constant(0));
        var expression = new Sum(new Constant(0), product);
        var rootNode = new RootNode(expression);
        
        visitor.Visit(rootNode);

        Assert.AreEqual("0", rootNode.ToString(), "Zero in sum returns simpflied product which is 2*0=0");
        Debug.WriteLine($"rootNode: {rootNode.ToString()}");
    }

    [TestMethod]
    public void SimplicationVisitor_multiply_by_1_Test()
    {
        var visitor = new SimplificationVisitor();

        var sum = new Sum(new Constant(5), new Constant(3));
        var product = new Product(new Constant(1), sum);
        var productReversed = new Product(sum, new Constant(1));

        var rootNode = new RootNode(product);
        var rootNode2 = new RootNode(productReversed);
        visitor.Visit(rootNode);
        visitor.Visit(rootNode2);

        Assert.AreEqual("8", rootNode.ToString(), "Multiplying on Left by 1 should return a simplfied Right expression which is 5+3=8");
        Assert.AreEqual("8", rootNode.ToString(), "Multiplying on Right by 1 should return a simplfied Left expression which is 5+3=8");
        
        Debug.WriteLine($"rootNode: {rootNode.ToString()}");
    }

    [TestMethod]
    public void SimplicationVisitor_Add_Polynomials_Test()
    {
        var leftArray = new Double[] {1, 1, 1};
        var rightArray = new Double[] {-1, 2, -1, 1};

        var leftPoly = new Polynomial(leftArray, new Variable("x"));
        var rightPoly = new Polynomial(rightArray, new Variable("x"));

        Debug.WriteLine(leftPoly.ToString());
        Debug.WriteLine(rightPoly.ToString());

        var root = new RootNode(new Sum(leftPoly, rightPoly));

        new SimplificationVisitor().Visit(root);

        Debug.WriteLine(root.ToString());
        Assert.AreEqual("3x + 1x^3", root.ToString(), "Expanded coeffs should be {0, 3, 0, 1}"); 
    }

    [TestMethod]
    public void SimplicationVisitor_Expand_Constant_Polynomials_Test()
    {
        var leftArray = new Double[] {2};
        var rightArray = new Double[] {-3};

        var leftPoly = new Polynomial(leftArray, new Variable("x"));
        var rightPoly = new Polynomial(rightArray, new Variable("x"));

        var root = new RootNode(new Product(leftPoly, rightPoly));

        new SimplificationVisitor().Visit(root);
        
        Debug.WriteLine(root.ToString());
        Assert.AreEqual("-6", root.ToString(), "Expanded coeffs should be {-6}");
    }

    [TestMethod]
    public void SimplicationVisitor_Expand_Linear_Polynomials_Test()
    {
        var leftArray = new Double[] {-1, 1};
        var rightArray = new Double[] {1, 1};

        var leftPoly = new Polynomial(leftArray, new Variable("x"));
        var rightPoly = new Polynomial(rightArray, new Variable("x"));

        var root = new RootNode(new Product(leftPoly, rightPoly));

        new SimplificationVisitor().Visit(root);
        
        Debug.WriteLine(root.ToString());
        Assert.AreEqual("-1 + 1x^2", root.ToString(), "Expanded coeffs should be {-6}");
    }

    [TestMethod]
    public void SimplicationVisitor_Expand_Linear_Times_Quadradic_Polynomials_Test()
    {
        var leftArray = new Double[] {-1, 1, 1};
        var rightArray = new Double[] {1, 1};

        var leftPoly = new Polynomial(leftArray, new Variable("x"));
        var rightPoly = new Polynomial(rightArray, new Variable("x"));

        var root = new RootNode(new Product(leftPoly, rightPoly));

        new SimplificationVisitor().Visit(root);
        
        Debug.WriteLine(root.ToString());
        Assert.AreEqual("-1 + 2x^2 + 1x^3", root.ToString(), "Expanded coeffs should be {-1, 2, 1}");
    }

    [TestMethod]
    public void SimplicationVisitor_ExpandPolynomial_Test()
    {
        var leftArray = new Double[] {1, 1, 1};
        var rightArray = new Double[] {-1, 2, -1, 1};

        var leftPoly = new Polynomial(leftArray, new Variable("x"));
        var rightPoly = new Polynomial(rightArray, new Variable("x"));

        Debug.WriteLine(leftPoly.ToString());
        Debug.WriteLine(rightPoly.ToString());

        var root = new RootNode(new Product(leftPoly, rightPoly));

        new SimplificationVisitor().Visit(root);

        Debug.WriteLine(root.ToString());
        Assert.AreEqual("-1 + 1x + 2x^3 + 1x^5", root.ToString(), "Expanded coeffs should be {-1, 1, 1, 1, 0, 0, 1}");
    }
}