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
".GetDefaultNewLineCharAndReplaceIt().Trim();

            var method = MethodGenerator.NewMethod()
                .WithMethodName("UnitTestMethod")
                .WithNoAnnotations()
                .WithNoParameters()
                .AddBody(string.Empty)
                .Generate();

            UnitTestHelper.AssertSimilarStrings(expected, method);
        }

        [Fact]
        public void SingleAnnotation()
        {
            var expected = @"
[SomeAnnotation]
public void UnitTestMethod()
{
    
}
".GetDefaultNewLineCharAndReplaceIt().Trim();

            var method = MethodGenerator.NewMethod()
                .WithMethodName("UnitTestMethod")
                .AddAnnotations(new List<string> { "[SomeAnnotation]" })
                .WithNoParameters()
                .AddBody(string.Empty)
                .Generate();

            UnitTestHelper.AssertSimilarStrings(expected, method);
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
".GetDefaultNewLineCharAndReplaceIt().Trim();

            var method = MethodGenerator.NewMethod()
                .WithMethodName("UnitTestMethod")
                .AddAnnotations(new List<string> { "[SomeAnnotation1]", "[SomeAnnotation2]" })
                .WithNoParameters()
                .AddBody(string.Empty)
                .Generate();

            UnitTestHelper.AssertSimilarStrings(expected, method);
        }

        [Fact]
        public void SingleParameter()
        {
            var expected = @"
public void UnitTestMethod(int param1)
{
    
}
".GetDefaultNewLineCharAndReplaceIt().Trim();

            var method = MethodGenerator.NewMethod()
                .WithMethodName("UnitTestMethod")
                .WithNoAnnotations()
                .AddParameters(new List<(string, string)> { ("param1", "int") })
                .AddBody(string.Empty)
                .Generate();

            UnitTestHelper.AssertSimilarStrings(expected, method);
        }

        [Fact]
        public void MultipleParameters()
        {
            var expected = @"
public void UnitTestMethod(int param1, SomeObj param2)
{
    
}
".GetDefaultNewLineCharAndReplaceIt().Trim();

            var method = MethodGenerator.NewMethod()
                .WithMethodName("UnitTestMethod")
                .WithNoAnnotations()
                .AddParameters(new List<(string, string)> { ("param1", "int"), ("param2", "SomeObj") })
                .AddBody(string.Empty)
                .Generate();

            UnitTestHelper.AssertSimilarStrings(expected, method);
        }

        [Fact]
        public void WithBody()
        {
            var methodBody = @"
var x = 2;
var y = 6;
return x * y;
".GetDefaultNewLineCharAndReplaceIt().Trim();
            var expected = @"
public void UnitTestMethod()
{
    var x = 2;
    var y = 6;
    return x * y;
}
".GetDefaultNewLineCharAndReplaceIt().Trim();

            var method = MethodGenerator.NewMethod()
                .WithMethodName("UnitTestMethod")
                .WithNoAnnotations()
                .WithNoParameters()
                .AddBody(methodBody)
                .Generate();

            UnitTestHelper.AssertSimilarStrings(expected, method);
        }

    }
}