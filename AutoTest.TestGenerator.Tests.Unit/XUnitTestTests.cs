using AutoTest.CodeGenerator.Tests.Unit;
using AutoTest.CodeInterpreter.Wrappers;
using AutoTest.TestGenerator.Generators.XUnit.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AutoTest.TestGenerator.Tests.Unit
{
    public class XUnitTestTests
    {

        [Fact]
        public void OnlyName()
        {
            var expected = @"
[Fact]
public void UnitTestMethod()
{
    // Arrange
    
    // Act
    
    // Assert
    
}
".Trim();
            var method = new XUnitTest("UnitTestMethod", Enumerable.Empty<StatementWrapper>()).ToString();

            UnitTestHelper.AssertSimilarStrings(expected, method);
        }

        [Fact]
        public void NameAndSimpleBodyWithoutParameters()
        {
            var methodSyntax = GetMethodSyntaxFromExample(_simpleClassAndMethodWithoutParameters);

            var expected = @"
[Fact]
public void UnitTestMethod()
{
    // Arrange
    
    // Act
    var actual = _sut.TestMethod();
    
    // Assert
    Assert.Equal(Assert.Equal(expected, actual);
}
".Trim();
            var method = new XUnitTest("UnitTestMethod", new List<StatementWrapper> { methodSyntax }).ToString();

            UnitTestHelper.AssertSimilarStrings(expected, method);
        }

        [Fact]
        public void NameAndSimpleBodyWithParameters()
        {
            var methodSyntax = GetMethodSyntaxFromExample(_simpleClassAndMethodWithParameters);

            var expected = @"
[Theory]
[InlineData(1)]
public void UnitTestMethod(int x)
{
    // Arrange
    
    // Act
    var actual = _sut.TestMethod(x);
    
    // Assert
    Assert.Equal(Assert.Equal(expected, actual);
}
".Trim();
            var parameters = new List<List<(string Name, Type Type, object Value)>>
            {
                new List<(string Name, Type Type, object Value)>
                {
                    ("x", typeof(int), 1 ),
                }
            };
            var method = new XUnitTest("UnitTestMethod", parameters, new List<StatementWrapper> { methodSyntax }).ToString();

            UnitTestHelper.AssertSimilarStrings(expected, method);
        }


        private static StatementWrapper GetMethodSyntaxFromExample(string exampleCode)
        {
            var tree = CSharpSyntaxTree.ParseText(exampleCode);
            var root = tree.GetCompilationUnitRoot();

            var @class = root.Members.Cast<SyntaxNode>().First();
            return new StatementWrapper { SyntaxNode = @class.DescendantNodes().OfType<MethodDeclarationSyntax>().First() };
        }

        private string _simpleClassAndMethodWithoutParameters = @"
namespace TestNameSpace
{
    public class TestClass
    {
        public int TestMethod(int x) 
        {
            return x;
        }
    }
}
".Trim();

        private string _simpleClassAndMethodWithParameters = @"
namespace TestNameSpace
{
    public class TestClass
    {
        public int TestMethod() 
        {
            return 0;
        }
    }
}
".Trim();

    }
}
