using AutoTest.CodeGenerator.Enums;
using AutoTest.CodeGenerator.Generators;
using AutoTest.CodeGenerator.Models;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AutoTest.CodeGenerator.Tests.Unit
{
    public class ClassGeneratorTests
    {
        [Fact]
        public void ClassGenerator_GivenOnlyName_ShouldGenerateEmptyClass()
        {
            var expected = new Class
            {
                Name = "UnitTestClass"
            };

            var @class = ClassGenerator.NewClass()
                .WithClassName(expected.Name)
                .WithNoModifiers()
                .WithNoUsings()
                .WithNoAnnotations()
                .WithNoParameters()
                .WithNoMethods()
                .GenerateClass();

            @class.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ClassGenerator_GivenSingleUsing_ShouldGenerateClassWithUsing()
        {
            var expected = new Class
            {
                Name = "UnitTestClass",
                Imports = new List<string> { "using System;" }
            };

            var @class = ClassGenerator.NewClass()
                .WithClassName(expected.Name)
                .WithNoModifiers()
                .WithUsings(expected.Imports.ToArray())// TODO change how imports are added and rename usings to imports
                .WithNoAnnotations()
                .WithNoParameters()
                .WithNoMethods()
                .GenerateClass();

            @class.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ClassGenerator_GivenMultipleUsings_ShouldGenerateClassWithUsings()
        {
            var expected = new Class
            {
                Name = "UnitTestClass",
                Imports = new List<string> { "using System;", "using System.Linq;", "using Xunit;" }
            };

            var @class = ClassGenerator.NewClass()
                .WithClassName(expected.Name)
                .WithModifiers()
                .WithUsings(expected.Imports.ToArray())
                .WithNoAnnotations()
                .WithNoParameters()
                .WithNoMethods()
                .GenerateClass();

            @class.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ClassGenerator_GivenSingleAnnotation_ShouldGenerateClassWithAnnotation()
        {
            var expected = new Class
            {
                Name = "UnitTestClass",
                Annotations = new List<string> { "[SomeAnnotation]" } 
            };

            var @class = ClassGenerator.NewClass()
                .WithClassName(expected.Name)
                .WithNoModifiers()
                .WithNoUsings()
                .WithAnnotations(expected.Annotations.ToArray())// TODO change how Annotations are added
                .WithNoParameters()
                .WithNoMethods()
                .GenerateClass();

            @class.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ClassGenerator_GivenMultipleAnnotations_ShouldGenerateClassWithAnnotations()
        {
            var expected = new Class
            {
                Name = "UnitTestClass",
                Annotations = new List<string> { "[SomeAnnotation1]", "[SomeAnnotation2]", "[SomeAnnotation3]" }
            };

            var @class = ClassGenerator.NewClass()
                .WithClassName(expected.Name)
                .WithNoModifiers()
                .WithNoUsings()
                .WithAnnotations(expected.Annotations.ToArray())
                .WithNoParameters()
                .WithNoMethods()
                .GenerateClass();

            @class.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ClassGenerator_GivenSingleDependencyInjectionParameter_ShouldGenerateClassWithDependencyInjectionParameter()
        {
            var expected = new Class
            {
                Name = "UnitTestClass",
                Parameters = new List<(string, string, bool)> { ("param1", "int", true) }
            };

            var @class = ClassGenerator.NewClass()
                .WithClassName(expected.Name)
                .WithModifiers()
                .WithNoUsings()
                .WithNoAnnotations()
                .WithDIParameters(expected.Parameters.Select(x => (x.Name, x.Type)).ToArray())
                .WithNoMethods()
                .GenerateClass();

            @class.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ClassGenerator_GivenMultipleDependencyInjectionParameters_ShouldGenerateClassWithDependencyInjectionParameters()
        {
            var expected = new Class
            {
                Name = "UnitTestClass",
                Parameters = new List<(string, string, bool)> { ("param1", "int", true), ("param2", "SomeObj", true) }
            };

            var @class = ClassGenerator.NewClass()
                .WithClassName(expected.Name)
                .WithModifiers()
                .WithNoUsings()
                .WithNoAnnotations()
                .WithDIParameters(expected.Parameters.Select(x => (x.Name, x.Type)).ToArray())
                .WithNoMethods()
                .GenerateClass();

            @class.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ClassGenerator_GivenSingleParameter_ShouldGenerateClassWithParameter()
        {
            var expected = new Class
            {
                Name = "UnitTestClass",
                Parameters = new List<(string, string, bool)> { ("param1", "int", false) }
            };

            var @class = ClassGenerator.NewClass()
                .WithClassName(expected.Name)
                .WithModifiers()
                .WithNoUsings()
                .WithNoAnnotations()
                .WithParameters(expected.Parameters.Select(x => (x.Name, x.Type)).ToArray())
                .WithNoMethods()
                .GenerateClass();

            @class.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ClassGenerator_GivenMultipleParameters_ShouldGenerateClassWithParameters()
        {
            var expected = new Class
            {
                Name = "UnitTestClass",
                Parameters = new List<(string, string, bool)> { ("param1", "int", false), ("param2", "SomeObj", false) }
            };

            var @class = ClassGenerator.NewClass()
                .WithClassName(expected.Name)
                .WithModifiers()
                .WithNoUsings()
                .WithNoAnnotations()
                .WithParameters(expected.Parameters.Select(x => (x.Name, x.Type)).ToArray())
                .WithNoMethods()
                .GenerateClass();

            @class.Should().BeEquivalentTo(expected);
        }

//        [Fact]
//        public void ClassGenerator_GivenSingleMethod_ShouldGenerateClassWithMethod()
//        {
//            var method = @"
//public int Return1()
//{
//    return 1;
//}
//".GetDefaultNewLineCharAndReplaceIt().Trim();

//            var expected = @"
//public class UnitTestClass
//{
//    public int Return1()
//    {
//        return 1;
//    }
//}
//".GetDefaultNewLineCharAndReplaceIt().Trim();

//            var @class = ClassGenerator.NewClass()
//                .WithClassName(expected.Name)
//                .WithModifiers(ClassModifiers.Public)
//                .WithNoUsings()
//                .WithNoAnnotations()
//                .WithNoParameters()
//                .WithMethods(method)
//                .Generate();

//            UnitTestHelper.AssertSimilarStrings(expected, @class);
//        }

//        [Fact]
//        public void ClassGenerator_GivenMultipleMethods_ShouldGenerateClassWithMethods()
//        {
//            var method = @"
//public int Return1()
//{
//    return 1;
//}
//".GetDefaultNewLineCharAndReplaceIt().Trim();

//            var expected = @"
//public class UnitTestClass
//{
//    public int Return1()
//    {
//        return 1;
//    }
    
//    public int Return1()
//    {
//        return 1;
//    }
//}
//".GetDefaultNewLineCharAndReplaceIt().Trim();

//            var @class = ClassGenerator.NewClass()
//                .WithClassName(expected.Name)
//                .WithModifiers(ClassModifiers.Public)
//                .WithNoUsings()
//                .WithNoAnnotations()
//                .WithNoParameters()
//                .WithMethods(method, method)
//                .Generate();

//            UnitTestHelper.AssertSimilarStrings(expected, @class);
//        }
    }
}