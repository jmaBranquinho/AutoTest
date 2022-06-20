using AutoTest.CodeGenerator.Generators;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace AutoTest.CodeGenerator.Tests.Unit
{
    // TODO: refactor these tests. They were done in a rush to ensure refactoring does not break anything
    // Plus they will help to show how to use this library
    public class MethodGeneratorTests
    {
        [Fact]
        public void OnlyName()
        {
            var expected = @"
public void UnitTestMethod()
{
    
}
".Trim();

            var method = MethodGenerator.NewMethod()
                .WithMethodName("UnitTestMethod")
                .WithNoAnnotations()
                .WithNoParameters()
                .AddBody(string.Empty)
                .Generate();

            AssertSimilarStrings(expected, method);
        }

        [Fact]
        public void SingleAnnotation()
        {
            var expected = @"
[SomeAnnotation]
public void UnitTestMethod()
{
    
}
".Trim();

            var method = MethodGenerator.NewMethod()
                .WithMethodName("UnitTestMethod")
                .AddAnnotations(new List<string> { "[SomeAnnotation]" })
                .WithNoParameters()
                .AddBody(string.Empty)
                .Generate();

            AssertSimilarStrings(expected, method);
        }

        [Fact]
        public void MultipleAnnotations()
        {
            var expected = @"
[SomeAnnotation1]
[SomeAnnotation2]
public void UnitTestMethod()
{
    
}
".Trim();

            var method = MethodGenerator.NewMethod()
                .WithMethodName("UnitTestMethod")
                .AddAnnotations(new List<string> { "[SomeAnnotation1]", "[SomeAnnotation2]" })
                .WithNoParameters()
                .AddBody(string.Empty)
                .Generate();

            AssertSimilarStrings(expected, method);
        }

        [Fact]
        public void SingleParameter()
        {
            var expected = @"
public void UnitTestMethod(int param1)
{
    
}
".Trim();

            var method = MethodGenerator.NewMethod()
                .WithMethodName("UnitTestMethod")
                .WithNoAnnotations()
                .AddParameters(new List<(string, string)> { ("param1", "int") })
                .AddBody(string.Empty)
                .Generate();

            AssertSimilarStrings(expected, method);
        }

        [Fact]
        public void MultipleParameters()
        {
            var expected = @"
public void UnitTestMethod(int param1, SomeObj param2)
{
    
}
".Trim();

            var method = MethodGenerator.NewMethod()
                .WithMethodName("UnitTestMethod")
                .WithNoAnnotations()
                .AddParameters(new List<(string, string)> { ("param1", "int"), ("param2", "SomeObj") })
                .AddBody(string.Empty)
                .Generate();

            AssertSimilarStrings(expected, method);
        }

        [Fact]
        public void WithBody()
        {
            var methodBody = @"
var x = 2;
var y = 6;
return x * y;
".Trim();
            var expected = @"
public void UnitTestMethod()
{
    var x = 2;
    var y = 6;
    return x * y;
}
".Trim();

            var method = MethodGenerator.NewMethod()
                .WithMethodName("UnitTestMethod")
                .WithNoAnnotations()
                .WithNoParameters()
                .AddBody(methodBody)
                .Generate();

            AssertSimilarStrings(expected, method);
        }

        // TODO: extract to some helper class
        private void AssertSimilarStrings(string expected, string actual)
        {
            expected = ReOrderReformatSpaces(expected);
            actual = ReOrderReformatSpaces(actual);

            actual.Should().BeEquivalentTo(expected);
        }

        // TODO: extract to some helper class
        private string ReOrderReformatSpaces(string text) => text
            .Replace("    ", "\t")
            .Replace("\r\n\t", "\t\r\n")
            .Replace("\r\n\t\r\n", "\t\r\n\r\n")
            .Replace("\t\r\n\t", "\t\t\r\n");
    }
}