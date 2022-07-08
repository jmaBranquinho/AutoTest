using AutoTest.CodeGenerator.Enums;
using AutoTest.CodeGenerator.Generators;
using AutoTest.CodeGenerator.Models;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace AutoTest.CodeGenerator.Tests.Unit
{
    public class MethodGeneratorTests
    {
        [Fact]
        public void MethodGenerator_GivenOnlyName_ShouldGenerateEmptyMethod()
        {
            var expected = new Method
            {
                Name = "UnitTestMethod",
            };

            var method = MethodGenerator.NewMethod()
                .WithMethodName("UnitTestMethod")
                .WithNoAnnotations()
                .WithNoModifiers()
                .WithReturnType(null)
                .WithNoParameters()
                .WithBody(string.Empty)
                .Generate();

            method.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void MethodGenerator_GivenModifiers_ShouldGenerateMethodWithModifiers()
        {
            var expected = new Method
            {
                Name = "UnitTestMethod",
                Modifiers = new List<MethodModifiers> { MethodModifiers.Public, MethodModifiers.Static }
            };

            var method = MethodGenerator.NewMethod()
                .WithMethodName("UnitTestMethod")
                .WithNoAnnotations()
                .WithModifiers(MethodModifiers.Public, MethodModifiers.Static)
                .WithReturnType(null)
                .WithNoParameters()
                .WithBody(string.Empty)
                .Generate();

            method.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void MethodGenerator_GivenSingleAnnotation_ShouldGenerateMethodWithAnnotation()
        {
            var expected = new Method
            {
                Name = "UnitTestMethod",
                Annotations = new List<string> { "[SomeAnnotation]" }
            };

            var method = MethodGenerator.NewMethod()
                .WithMethodName("UnitTestMethod")
                .WithAnnotations("[SomeAnnotation]")
                .WithNoModifiers()
                .WithReturnType(null)
                .WithNoParameters()
                .WithBody(string.Empty)
                .Generate();

            method.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void MethodGenerator_GivenSeveralAnnotations_ShouldGenerateMethodWithAnnotations()
        {
            var expected = new Method
            {
                Name = "UnitTestMethod",
                Annotations = new List<string> { "[SomeAnnotation]", "[SomeAnnotation2]" }
            };

            var method = MethodGenerator.NewMethod()
                .WithMethodName("UnitTestMethod")
                .WithAnnotations("[SomeAnnotation1]", "[SomeAnnotation2]")
                .WithNoModifiers()
                .WithReturnType(null)
                .WithNoParameters()
                .WithBody(string.Empty)
                .Generate();

            method.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void MethodGenerator_GivenSingleParameter_ShouldGenerateMethodWithParameter()
        {
            var expected = new Method
            {
                Name = "UnitTestMethod",
                Parameters = new List<(string, string)> { ("param1", "int") }
            };

            var method = MethodGenerator.NewMethod()
                .WithMethodName("UnitTestMethod")
                .WithNoAnnotations()
                .WithNoModifiers()
                .WithReturnType(null)
                .WithParameters(("param1", "int"))
                .WithBody(string.Empty)
                .Generate();

            method.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void MethodGenerator_GivenMultipleParameter_ShouldGenerateMethodWithParameters()
        {
            var expected = new Method
            {
                Name = "UnitTestMethod",
                Parameters = new List<(string, string)> { ("param1", "int"), ("param2", "SomeObj") }
            };

            var method = MethodGenerator.NewMethod()
                .WithMethodName("UnitTestMethod")
                .WithNoAnnotations()
                .WithNoModifiers()
                .WithReturnType(null)
                .WithParameters(("param1", "int"), ("param2", "SomeObj"))
                .WithBody(string.Empty)
                .Generate();

            method.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void MethodGenerator_GivenBody_ShouldGenerateMethodWithBody()
        {
            var methodBody = @"
var x = 2;
var y = 6;
return x * y;
".GetDefaultNewLineCharAndReplaceIt().Trim();
            var expected = new Method
            {
                Name = "UnitTestMethod",
                Body = methodBody
            };

            var method = MethodGenerator.NewMethod()
                .WithMethodName("UnitTestMethod")
                .WithNoAnnotations()
                .WithNoModifiers()
                .WithReturnType(null)
                .WithNoParameters()
                .WithBody(methodBody)
                .Generate();

            method.Should().BeEquivalentTo(expected);
        }

    }
}