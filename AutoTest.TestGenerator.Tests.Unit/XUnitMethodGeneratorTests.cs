using AutoFixture;
using AutoTest.CodeGenerator.Tests.Unit;
using AutoTest.CodeInterpreter.Wrappers;
using AutoTest.TestGenerator.Generators.XUnit;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace AutoTest.TestGenerator.Tests.Unit
{
    public class XUnitMethodGeneratorTests
    {
        private readonly Fixture _fixture;

        public XUnitMethodGeneratorTests()
        {
            _fixture = new Fixture();
            //_fixture
            //    .Customize<StatementWrapper>(sw => sw.With(x => x.SyntaxNode, _fixture.Create<MethodDeclarationSyntax>()));
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
            var method = _fixture
                .Build<MethodWrapper>()
                .Without(x => x.ExecutionPaths)
                .Create();

            var methodBody = new XUnitMethodGenerator().GenerateMethod("UnitTestMethod", method);

            UnitTestHelper.AssertSimilarStrings(expected, methodBody);
        }
    }
}