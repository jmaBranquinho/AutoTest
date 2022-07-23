using AutoTest.TestGenerator.Generators.Enums;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace AutoTest.TestGenerator.Tests.Unit.TestMethodGeneration.XUnit
{
    public partial class XUnitMethodGeneratorTests
    {
        [Fact]
        public void ForStatements_SimpleForLoop()
        {
            var method = GetMethodFromExample(_simpleFor);

            var result = _sut.GenerateUnitTests(method, TestNamingConventions.MethodName_WhenCondition_ShouldResult);

            var countIncrement4Times = 4;
            var returnStatement = 1;
            var varDeclarationSkipped = 0;
            var methodStatement = 1;
            var pathSize = methodStatement + varDeclarationSkipped + countIncrement4Times + returnStatement;
            method.ExecutionPaths.Count().Should().Be(1);  
            method.ExecutionPaths.First().Count().Should().Be(pathSize);
        }

        private static readonly string _simpleFor = @"
        public int TestMethod() 
        {
            var count = 0;
            for (int i = 0; i < 5; i++)
            {
                count++;
            }
            return count;
        }
".Trim();
    }
}
