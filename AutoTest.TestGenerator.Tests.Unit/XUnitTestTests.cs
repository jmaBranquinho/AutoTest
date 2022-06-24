using AutoFixture;
using AutoTest.CodeGenerator.Tests.Unit;
using AutoTest.CodeInterpreter.Wrappers;
using AutoTest.TestGenerator.Generators.XUnit.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AutoTest.TestGenerator.Tests.Unit
{
    public class XUnitTestTests
    {
        private readonly Fixture _fixture;

        public XUnitTestTests()
        {
            _fixture = new Fixture();
        }

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
    
    // Assert
    
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
    
    // Assert
    
}
".Trim();
            var parameters = new List<List<(string Name, string Type, object Value)>>
            {
                new List<(string Name, string Type, object Value)>
                {
                    ("x", XUnitTest.TypeToString(typeof(int)), 1 ),
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
public class TestClass
{
    public int TestMethod(int x) 
    {
        return x;
    }
}
".Trim();

        private string _simpleClassAndMethodWithParameters = @"
public class TestClass
{
    public int TestMethod() 
    {
        return 0;
    }
}
".Trim();

    }
}
