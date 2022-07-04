using AutoTest.CodeGenerator.Enums;
using AutoTest.CodeGenerator.Generators;
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
                .WithModifiers(MethodModifiers.Public)
                .WithReturnType(null)
                .WithNoParameters()
                .WithBody(string.Empty)
                .Generate();

            UnitTestHelper.AssertSimilarStrings(expected, method);
        }

        [Fact]
        public void NameAndSeveralModifiers()
        {
            var expected = @"
public static void UnitTestMethod()
{
    
}
".GetDefaultNewLineCharAndReplaceIt().Trim();

            var method = MethodGenerator.NewMethod()
                .WithMethodName("UnitTestMethod")
                .WithNoAnnotations()
                .WithModifiers(MethodModifiers.Public, MethodModifiers.Static)
                .WithReturnType(null)
                .WithNoParameters()
                .WithBody(string.Empty)
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
                .WithAnnotations("[SomeAnnotation]")
                .WithModifiers(MethodModifiers.Public)
                .WithReturnType(null)
                .WithNoParameters()
                .WithBody(string.Empty)
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
                .WithAnnotations("[SomeAnnotation1]", "[SomeAnnotation2]")
                .WithModifiers(MethodModifiers.Public)
                .WithReturnType(null)
                .WithNoParameters()
                .WithBody(string.Empty)
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
                .WithModifiers(MethodModifiers.Public)
                .WithReturnType(null)
                .WithParameters(("param1", "int"))
                .WithBody(string.Empty)
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
                .WithModifiers(MethodModifiers.Public)
                .WithReturnType(null)
                .WithParameters(("param1", "int"), ("param2", "SomeObj"))
                .WithBody(string.Empty)
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
                .WithModifiers(MethodModifiers.Public)
                .WithReturnType(null)
                .WithNoParameters()
                .WithBody(methodBody)
                .Generate();

            UnitTestHelper.AssertSimilarStrings(expected, method);
        }

    }
}