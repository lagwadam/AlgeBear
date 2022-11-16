using Microsoft.VisualStudio.TestTools.UnitTesting;
using UtilityLibraries;
using System.Diagnostics;

namespace ExpressionLibraryTest;

[TestClass]
public class SimplificationVisitorTests
{
    [TestMethod]
    public void SimplicationVisitorSumTest()
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
    public void SimplicationVisitorSumAndProductTest()
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
    public void SimplicationVisitorZeroSumWithProductTest()
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
    public void SimplicationVisitorZeroSumWithZeroProductTest()
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
}