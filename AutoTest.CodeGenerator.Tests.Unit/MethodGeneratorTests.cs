using AutoTest.CodeGenerator.Enums;
using AutoTest.CodeGenerator.Generators;
using Xunit;

namespace AutoTest.CodeGenerator.Tests.Unit
{
    public class MethodGeneratorTests
    {
        [Fact]
        public void MethodGenerator_GivenOnlyName_ShouldGenerateEmptyMethod()
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

            UnitTestHelper.AssertSimilarStrings(expected, method.ToString());
        }

        [Fact]
        public void MethodGenerator_GivenModifiers_ShouldGenerateMethodWithModifiers()
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

            UnitTestHelper.AssertSimilarStrings(expected, method.ToString());
        }

        [Fact]
        public void MethodGenerator_GivenSingleAnnotation_ShouldGenerateMethodWithAnnotation()
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

            UnitTestHelper.AssertSimilarStrings(expected, method.ToString());
        }

        [Fact]
        public void MethodGenerator_GivenSeveralAnnotations_ShouldGenerateMethodWithAnnotations()
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

            UnitTestHelper.AssertSimilarStrings(expected, method.ToString());
        }

        [Fact]
        public void MethodGenerator_GivenSingleParameter_ShouldGenerateMethodWithParameter()
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

            UnitTestHelper.AssertSimilarStrings(expected, method.ToString());
        }

        [Fact]
        public void MethodGenerator_GivenMultipleParameter_ShouldGenerateMethodWithParameters()
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

            UnitTestHelper.AssertSimilarStrings(expected, method.ToString());
        }

        [Fact]
        public void MethodGenerator_GivenBody_ShouldGenerateMethodWithBody()
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

            UnitTestHelper.AssertSimilarStrings(expected, method.ToString());
        }

    }
}