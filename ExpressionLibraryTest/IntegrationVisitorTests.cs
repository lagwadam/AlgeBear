using Microsoft.VisualStudio.TestTools.UnitTesting;
using UtilityLibraries;
using System.Diagnostics;

namespace ExpressionLibraryTest;

[TestClass]
public class IntegrationVisitorTests
{
    [TestMethod]
    public void IntegrationVisitor_Zero_Returns_Constant_Test()
    {
        var visitor = new IntegrationVisitor();

        var constant = new Constant(0);
        var integral = constant.Accept(visitor);

        var root = new RootNode(constant);
        Debug.WriteLine(root);
        new IntegrationVisitor().Visit(root);
        Debug.WriteLine(root);

        Assert.AreEqual(ExpressionTypeEnum.Polynomial, root.InnerExpression.ExpressionType);
        Assert.AreEqual("1", root.ToString(), "Integral of 1.23 should be 1.23x");
    }

    [TestMethod]
    public void IntegrationVisitor_Constant_Returns_Multiple_of_x_Test()
    {
        var visitor = new IntegrationVisitor();

        var constant = new Constant(1.23);
        var integral = constant.Accept(visitor);

        var root = new RootNode(constant);
        Debug.WriteLine(root);
        new IntegrationVisitor().Visit(root);
        Debug.WriteLine(root);

        Assert.AreEqual(ExpressionTypeEnum.Polynomial, root.InnerExpression.ExpressionType);
        Assert.AreEqual("1.23x", root.ToString(), "Integral of 1.23 should be 1.23x");
    }


    [TestMethod]
    public void IntegrationVisitor_Variable_Test()
    {
        var visitor = new IntegrationVisitor();

        var variable = new Variable("x");
        var root = new RootNode(variable);

        Debug.WriteLine(root);
        new IntegrationVisitor().Visit(root);
        Debug.WriteLine(root);

        Assert.AreEqual(ExpressionTypeEnum.Polynomial, root.InnerExpression.ExpressionType);
        Assert.AreEqual(".5x^2", root.ToString(), "Integral of x should be .5x^2");
    }


    [TestMethod]
    public void IntegrationVisitor_ZeroPolynomail_Test()
    {
        var visitor = new IntegrationVisitor();

        var poly = new Polynomial(new double[] { 0 }, new Variable("x"));
        var root = new RootNode(poly);

        Debug.WriteLine(root);
        new IntegrationVisitor().Visit(root);
        Debug.WriteLine(root);

        Assert.AreEqual(ExpressionTypeEnum.Polynomial, root.InnerExpression.ExpressionType);
        Assert.AreEqual("1", root.ToString(), "Integral of zero should be constant");
    }

    [TestMethod]
    public void IntegrationVisitor_Constant_Polynomail_Test()
    {
        var visitor = new IntegrationVisitor();

        var poly = new Polynomial(new double[] { 4.44 }, new Variable("x"));
        var root = new RootNode(poly);

        Debug.WriteLine(root);
        new IntegrationVisitor().Visit(root);
        Debug.WriteLine(root);

        Assert.AreEqual(ExpressionTypeEnum.Polynomial, root.InnerExpression.ExpressionType);
        Assert.AreEqual("4.44x", root.ToString(), "Integral of constant should be a multiple of x");
    }

    [TestMethod]
    public void IntegrationVisitor_Quintic_Polynomail_Test()
    {
        var visitor = new IntegrationVisitor();

        var poly = new Polynomial(new double[] { 1, -2, 0, 3, 0, -24 }, new Variable("x"));
        var root = new RootNode(poly);

        Debug.WriteLine(root);
        new IntegrationVisitor().Visit(root);
        Debug.WriteLine(root);

        Assert.AreEqual(ExpressionTypeEnum.Polynomial, root.InnerExpression.ExpressionType);
        Assert.AreEqual("x - x^2 + .75x^4 - 4x^6", root.ToString(), "Integral of quintic should have degree 6");
    }

    [TestMethod]
    public void IntegrationVisitor_Constant_Returns_Constant_Times_x()
    {
        var visitor = new IntegrationVisitor();

        var constant = new Constant(1.23);
        var integral = constant.Accept(visitor);

        var root = new RootNode(constant);
        Debug.WriteLine(root);
        new IntegrationVisitor().Visit(root);
        Debug.WriteLine(root);

        Assert.AreEqual(ExpressionTypeEnum.Polynomial, root.InnerExpression.ExpressionType);
        Assert.AreEqual("1.23x", root.ToString(), "Integral of 1.23 should be 1.23x");
    }

    [TestMethod]
    public void IntegrationVisitor_Product_of_zero_test()
    {
        var visitor = new IntegrationVisitor();

        var constant = new Constant(0);

        var rightConstant = new Constant(0);

        var product = new Product(constant, rightConstant);
        RootNode root = new RootNode(product);
        Debug.WriteLine(root);
        new IntegrationVisitor().Visit(root);
        Debug.WriteLine(root);

        Polynomial? integral = root.InnerExpression as Polynomial;

        Assert.AreEqual(ExpressionTypeEnum.Polynomial, root.InnerExpression.ExpressionType);
        Assert.AreEqual("1", root.ToString(), "Integral of 0 should be 1");
    }

    [TestMethod]
    public void IntegrationVisitor_Product_of_zero_and_constant_test()
    {
        var visitor = new IntegrationVisitor();

        var constant = new Constant(0);
        var rightConstant = new Constant(2.23);

        RootNode root = new RootNode(new Product(constant, rightConstant));
        Debug.WriteLine(root);
        new IntegrationVisitor().Visit(root);
        Debug.WriteLine(root);

        Polynomial? integral = root.InnerExpression as Polynomial;

        Assert.AreEqual(ExpressionTypeEnum.Polynomial, root.InnerExpression.ExpressionType);
        Assert.AreEqual("1", root.ToString(), "Integral of 0 should be 1");
    }

    [TestMethod]
    public void IntegrationVisitor_Product_of_zero_and_variable_test()
    {
        var visitor = new IntegrationVisitor();

        var constant = new Constant(0);
        var variable = new Variable("x");

        RootNode root = new RootNode(new Product(constant, variable));
        Debug.WriteLine(root);
        new IntegrationVisitor().Visit(root);
        Debug.WriteLine(root);

        Polynomial? integral = root.InnerExpression as Polynomial;

        Assert.AreEqual(ExpressionTypeEnum.Polynomial, root.InnerExpression.ExpressionType);
        Assert.AreEqual("1", root.ToString(), "Integral of 0 should be 1");
    }

    [TestMethod]
    public void IntegrationVisitor_Product_of_zero_and_Polynomial_test()
    {
        var visitor = new IntegrationVisitor();

        var constant = new Constant(0);
        var polynomial = new Polynomial(new double[] { -3, 2, -1 }, new Variable("x"));

        RootNode root = new RootNode(new Product(constant, polynomial));
        Debug.WriteLine(root);
        new IntegrationVisitor().Visit(root);
        Debug.WriteLine(root);

        Polynomial? integral = root.InnerExpression as Polynomial;

        Assert.AreEqual(ExpressionTypeEnum.Polynomial, root.InnerExpression.ExpressionType);
        Assert.AreEqual("1", root.ToString(), "Integral of 0 should be 1");
    }

    [TestMethod]
    public void IntegrationVisitor_Product_of_zero_and_Expression_test()
    {
        var visitor = new IntegrationVisitor();

        var constant = new Constant(0);
        var exp = new Exp(new Variable("x"));

        RootNode root = new RootNode(new Product(constant, exp));
        Debug.WriteLine(root);
        new IntegrationVisitor().Visit(root);
        Debug.WriteLine(root);

        Polynomial? integral = root.InnerExpression as Polynomial;

        Assert.AreEqual(ExpressionTypeEnum.Polynomial, root.InnerExpression.ExpressionType);
        Assert.AreEqual("1", root.ToString(), "Integral of 0 should be 1");
    }

    [TestMethod]
    public void IntegrationVisitor_Product_of_NonZero_and_Zero_test()
    {
        var visitor = new IntegrationVisitor();

        var constant = new Constant(17);
        var rightConstant = new Constant(0);

        RootNode root = new RootNode(new Product(constant, rightConstant));
        Debug.WriteLine(root);
        new IntegrationVisitor().Visit(root);
        Debug.WriteLine(root);

        Polynomial? integral = root.InnerExpression as Polynomial;

        Assert.AreEqual(ExpressionTypeEnum.Polynomial, root.InnerExpression.ExpressionType);
        Assert.AreEqual("1", root.ToString(), "Integral of 0 should be 1");
    }


    [TestMethod]
    public void IntegrationVisitor_Product_of_NonZero_and_constant_test()
    {
        var visitor = new IntegrationVisitor();

        var constant = new Constant(17);
        var rightConstant = new Constant(2);

        RootNode root = new RootNode(new Product(constant, rightConstant));
        Debug.WriteLine(root);
        new IntegrationVisitor().Visit(root);
        Debug.WriteLine(root);

        Polynomial? integral = root.InnerExpression as Polynomial;

        Assert.AreEqual(ExpressionTypeEnum.Polynomial, root.InnerExpression.ExpressionType);
        Assert.AreEqual("34x", root.ToString(), "Integral of 34 should be 34x");
    }

    [TestMethod]
    public void IntegrationVisitor_Product_of_NonZero_and_variable_test()
    {
        var visitor = new IntegrationVisitor();

        var constant = new Constant(17);
        var variable = new Variable("x");

        RootNode root = new RootNode(new Product(constant, variable));
        Debug.WriteLine(root);
        new IntegrationVisitor().Visit(root);
        Debug.WriteLine(root);

        Polynomial? integral = root.InnerExpression as Polynomial;

        Assert.AreEqual(ExpressionTypeEnum.Polynomial, root.InnerExpression.ExpressionType);
        Assert.AreEqual("8.5x^2", root.ToString(), "Integral of 17x should be 8.5x^2");
    }

    [TestMethod]
    public void IntegrationVisitor_Product_of_NonZero_and_Polynomial_test()
    {
        var visitor = new IntegrationVisitor();

        var constant = new Constant(5);
        var polynomial = new Polynomial(new double[] { -3, 2, -21 }, new Variable("x"));

        RootNode root = new RootNode(new Product(constant, polynomial));
        Debug.WriteLine(root);
        new IntegrationVisitor().Visit(root);
        Debug.WriteLine(root);

        Polynomial? integral = root.InnerExpression as Polynomial;

        Assert.AreEqual(ExpressionTypeEnum.Polynomial, root.InnerExpression.ExpressionType);
        Assert.AreEqual("-15x + 5x^2 - 35x^3", root.ToString(), "Wrong expression for integral.");
    }

    [TestMethod]
    public void IntegrationVisitor_Product_of_NonZero_and_Expression_test()
    {
        var visitor = new IntegrationVisitor();

        var constant = new Constant(17);
        var exp = new Exp(new Variable("x"));

        RootNode root = new RootNode(new Product(constant, exp));
        Debug.WriteLine(root);
        new IntegrationVisitor().Visit(root);
        Debug.WriteLine(root);

        IExpression integral = root.InnerExpression;

        Assert.AreEqual(ExpressionTypeEnum.Product, root.InnerExpression.ExpressionType);
        Assert.AreEqual("(17*(Exp(x)))", integral.ToString(), "Wrong expression for integral.");
    }

    [TestMethod]
    public void IntegrationVisitor_Sum_With_Auto_SimplificationTest()
    {
        var visitor = new IntegrationVisitor();

        var leftPoly = new Polynomial(new Double[] { 1, 1, 1 }, new Variable("x"));
        var rightPoly = new Polynomial(new Double[] { 1, -1, 2 }, new Variable("x"));

        var root = new RootNode(new Sum(leftPoly, rightPoly));

        Debug.WriteLine(root);
        new IntegrationVisitor().Visit(root);

        Debug.WriteLine(root);

        Assert.AreEqual("2x + x^3", root.ToString(), "integral of (2 + 3x^2) should be 2x + x^3");
    }

    [TestMethod]
    public void IntegrationVisitor_PolyTest()
    {
        var visitor = new IntegrationVisitor();

        var expression = new Polynomial(new Double[] { 1, 2, 3, 4, 5, 6 }, new Variable("x"));
        var root = new RootNode(expression);
        Debug.WriteLine(root);
        new IntegrationVisitor().Visit(root);
        Debug.WriteLine(root);

        Assert.AreEqual("x + x^2 + x^3 + x^4 + x^5 + x^6", root.ToString(), "Wrong value for integral.");
    }


    [TestMethod]
    public void IntegrationVisitor_Product_With_Linear_and_Constant()
    {
        var visitor = new IntegrationVisitor();
        var leftPoly = new Polynomial(new Double[] { 1, 3 }, new Variable("x"));
        var rightPoly = new Polynomial(new Double[] { 2 }, new Variable("x"));

        var root = new RootNode(new Product(leftPoly, rightPoly));

        Debug.WriteLine(root);
        new IntegrationVisitor().Visit(root);

        var simplifier = new SimplificationVisitor();
        simplifier.Visit(root);
        Debug.WriteLine(root);

        Assert.AreEqual("2x + 3x^2", root.ToString(), "Integral of 2 + 6x should be 2x + 3x^3");
    }

    [TestMethod]
    public void IntegrationVisitor_Product_With_Auto_SimplificationTest()
    {
        var visitor = new IntegrationVisitor();
        var leftPoly = new Polynomial(new Double[] { 1, 3 }, new Variable("x"));
        var rightPoly = new Polynomial(new Double[] { 4, 2 }, new Variable("x"));

        var root = new RootNode(new Product(leftPoly, rightPoly));

        Debug.WriteLine(root);
        new IntegrationVisitor().Visit(root);
        // 1 + 3x * 4 + 2x = 4 + 14x + 6x^2 => 4x + 7x^2 + 2x^3 
        Debug.WriteLine(root);

        Assert.AreEqual("4x + 7x^2 + 2x^3", root.ToString(), "Incorrect value of Integral.");
    }

    [TestMethod]
    public void IntegrationVisitor_Series_Exp_2x_at_zero_Test()
    {
        var integrator = new IntegrationVisitor(0);
        var arg = new Polynomial(new Double[] { 0, 2 }, new Variable("x"));

        var root = new RootNode(new Exp(arg));

        Debug.WriteLine(root);
        var integral = new IntegrationVisitor().IntegrateBySeries(new Exp(arg));
        Debug.WriteLine(integral);

        //Assert.AreEqual("2x", integral.ToString(), "Incorrect value of Integral.");
    }

    [TestMethod]
    public void IntegrationVisitor_Series_Centered_at_zero_Test()
    {
        var integrator = new IntegrationVisitor(0);
        var arg = new Polynomial(new Double[] { 0, 0, 1 }, new Variable("x"));

        var root = new RootNode(new Exp(arg));

        Debug.WriteLine(root);
        var integral = new IntegrationVisitor().IntegrateBySeries(new Exp(arg));
        Debug.WriteLine(integral);

        //Assert.AreEqual("2x", integral.ToString(), "Incorrect value of Integral.");
    }

    [TestMethod]
    public void IntegrationVisitor_Series_Centered_at_1_Test()
    {
        var integrator = new IntegrationVisitor(1);
        var arg = new Polynomial(new Double[] { 0, 0, 1 }, new Variable("x"));

        var root = new RootNode(new Exp(arg));

        Debug.WriteLine(root);
        var integral = integrator.IntegrateBySeries(root);
        Debug.WriteLine(integral);

        //Assert.AreEqual("2x", integral.ToString(), "Incorrect value of Integral.");
    }
}