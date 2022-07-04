using AutoTest.CodeGenerator.Enums;
using AutoTest.CodeGenerator.Generators;
using Xunit;

namespace AutoTest.CodeGenerator.Tests.Unit
{
    // TODO: refactor these tests. They were done in a rush to ensure refactoring does not break anything
    // Plus they will help to show how to use this library
    public class ClassGeneratorTests
    {
        [Fact]
        public void OnlyName()
        {
            var expected = @"
public class UnitTestClass
{
    
}
".GetDefaultNewLineCharAndReplaceIt().Trim();

            var @class = ClassGenerator.NewClass()
                .WithClassName("UnitTestClass")
                .WithModifiers(ClassModifiers.Public)
                .WithNoUsings()
                .WithNoAnnotations()
                .WithNoParameters()
                .WithNoMethods()
                .Generate();

            UnitTestHelper.AssertSimilarStrings(expected, @class);
        }

        [Fact]
        public void SingleUsing()
        {
            var expected = @"
using System;

public class UnitTestClass
{
    
}
".GetDefaultNewLineCharAndReplaceIt().Trim();

            var @class = ClassGenerator.NewClass()
                .WithClassName("UnitTestClass")
                .WithModifiers(ClassModifiers.Public)
                .WithUsings("using System;")
                .WithNoAnnotations()
                .WithNoParameters()
                .WithNoMethods()
                .Generate();

            UnitTestHelper.AssertSimilarStrings(expected, @class);
        }

        [Fact]
        public void MultipleUsings()
        {
            var expected = @"
using System;
using System.Linq;
using Xunit;

public class UnitTestClass
{
    
}
".GetDefaultNewLineCharAndReplaceIt().Trim();

            var @class = ClassGenerator.NewClass()
                .WithClassName("UnitTestClass")
                .WithModifiers(ClassModifiers.Public)
                .WithUsings("using System;", "using System.Linq;", "using Xunit;")
                .WithNoAnnotations()
                .WithNoParameters()
                .WithNoMethods()
                .Generate();

            UnitTestHelper.AssertSimilarStrings(expected, @class);
        }

        [Fact]
        public void SingleAnnotation()
        {
            var expected = @"
[SomeAnnotation]
public class UnitTestClass
{
    
}
".GetDefaultNewLineCharAndReplaceIt().Trim();

            var @class = ClassGenerator.NewClass()
                .WithClassName("UnitTestClass")
                .WithModifiers(ClassModifiers.Public)
                .WithNoUsings()
                .WithAnnotations("[SomeAnnotation]")
                .WithNoParameters()
                .WithNoMethods()
                .Generate();

            UnitTestHelper.AssertSimilarStrings(expected, @class);
        }

        [Fact]
        public void MultipleAnnotations()
        {
            var expected = @"
[SomeAnnotation1]
[SomeAnnotation2]
[SomeAnnotation3]
public class UnitTestClass
{
    
}
".GetDefaultNewLineCharAndReplaceIt().Trim();

            var @class = ClassGenerator.NewClass()
                .WithClassName("UnitTestClass")
                .WithModifiers(ClassModifiers.Public)
                .WithNoUsings()
                .WithAnnotations("[SomeAnnotation1]", "[SomeAnnotation2]", "[SomeAnnotation3]")
                .WithNoParameters()
                .WithNoMethods()
                .Generate();

            UnitTestHelper.AssertSimilarStrings(expected, @class);
        }

        [Fact]
        public void SingleDependencyInjectionParameter()
        {
            var expected = @"
public class UnitTestClass
{
    private int _param1;
    
    public UnitTestClass(int param1)
    {
        _param1 = param1;
    }
}
".GetDefaultNewLineCharAndReplaceIt().Trim();

            var @class = ClassGenerator.NewClass()
                .WithClassName("UnitTestClass")
                .WithModifiers(ClassModifiers.Public)
                .WithNoUsings()
                .WithNoAnnotations()
                .WithDIParameters(("param1", "int"))
                .WithNoMethods()
                .Generate();

            UnitTestHelper.AssertSimilarStrings(expected, @class);
        }

        [Fact]
        public void MultipleDependencyInjectionParameter()
        {
            var expected = @"
public class UnitTestClass
{
    private int _param1;
    private SomeObj _param2;
    
    public UnitTestClass(int param1, SomeObj param2)
    {
        _param1 = param1;
        _param2 = param2;
    }
}
".GetDefaultNewLineCharAndReplaceIt().Trim();

            var @class = ClassGenerator.NewClass()
                .WithClassName("UnitTestClass")
                .WithModifiers(ClassModifiers.Public)
                .WithNoUsings()
                .WithNoAnnotations()
                .WithDIParameters(("param1", "int"), ("param2", "SomeObj"))
                .WithNoMethods()
                .Generate();

            UnitTestHelper.AssertSimilarStrings(expected, @class);
        }

        [Fact]
        public void SingleParameter()
        {
            var expected = @"
public class UnitTestClass
{
    private int _param1;
    
    
}
".GetDefaultNewLineCharAndReplaceIt().Trim();

            var @class = ClassGenerator.NewClass()
                .WithClassName("UnitTestClass")
                .WithModifiers(ClassModifiers.Public)
                .WithNoUsings()
                .WithNoAnnotations()
                .WithParameters(("param1", "int"))
                .WithNoMethods()
                .Generate();

            UnitTestHelper.AssertSimilarStrings(expected, @class);
        }

        [Fact]
        public void MultipleParameter()
        {
            var expected = @"
public class UnitTestClass
{
    private int _param1;
    private SomeObj _param2;
    
    
}
".GetDefaultNewLineCharAndReplaceIt().Trim();

            var @class = ClassGenerator.NewClass()
                .WithClassName("UnitTestClass")
                .WithModifiers(ClassModifiers.Public)
                .WithNoUsings()
                .WithNoAnnotations()
                .WithParameters(("param1", "int"), ("param2", "SomeObj"))
                .WithNoMethods()
                .Generate();

            UnitTestHelper.AssertSimilarStrings(expected, @class);
        }

        [Fact]
        public void SingleMethod()
        {
            var method = @"
public int Return1()
{
    return 1;
}
".GetDefaultNewLineCharAndReplaceIt().Trim();

            var expected = @"
public class UnitTestClass
{
    public int Return1()
    {
        return 1;
    }
}
".GetDefaultNewLineCharAndReplaceIt().Trim();

            var @class = ClassGenerator.NewClass()
                .WithClassName("UnitTestClass")
                .WithModifiers(ClassModifiers.Public)
                .WithNoUsings()
                .WithNoAnnotations()
                .WithNoParameters()
                .WithMethods(method)
                .Generate();

            UnitTestHelper.AssertSimilarStrings(expected, @class);
        }

        [Fact]
        public void MultipleMethod()
        {
            var method = @"
public int Return1()
{
    return 1;
}
".GetDefaultNewLineCharAndReplaceIt().Trim();

            var expected = @"
public class UnitTestClass
{
    public int Return1()
    {
        return 1;
    }
    
    public int Return1()
    {
        return 1;
    }
}
".GetDefaultNewLineCharAndReplaceIt().Trim();

            var @class = ClassGenerator.NewClass()
                .WithClassName("UnitTestClass")
                .WithModifiers(ClassModifiers.Public)
                .WithNoUsings()
                .WithNoAnnotations()
                .WithNoParameters()
                .WithMethods(method, method)
                .Generate();

            UnitTestHelper.AssertSimilarStrings(expected, @class);
        }
    }
}