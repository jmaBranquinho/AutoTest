using AutoTest.CodeGenerator.Enums;
using AutoTest.CodeGenerator.Generators;
using Xunit;

namespace AutoTest.CodeGenerator.Tests.Unit
{
    public class ClassGeneratorTests
    {
        [Fact]
        public void ClassGenerator_GivenOnlyName_ShouldGenerateEmptyClass()
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

            UnitTestHelper.AssertSimilarStrings(expected, @class.ToString());
        }

        [Fact]
        public void ClassGenerator_GivenSingleUsing_ShouldGenerateClassWithUsing()
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

            UnitTestHelper.AssertSimilarStrings(expected, @class.ToString());
        }

        [Fact]
        public void ClassGenerator_GivenMultipleUsings_ShouldGenerateClassWithUsings()
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

            UnitTestHelper.AssertSimilarStrings(expected, @class.ToString());
        }

        [Fact]
        public void ClassGenerator_GivenSingleAnnotation_ShouldGenerateClassWithAnnotation()
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

            UnitTestHelper.AssertSimilarStrings(expected, @class.ToString());
        }

        [Fact]
        public void ClassGenerator_GivenMultipleAnnotations_ShouldGenerateClassWithAnnotations()
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

            UnitTestHelper.AssertSimilarStrings(expected, @class.ToString());
        }

        [Fact]
        public void ClassGenerator_GivenSingleDependencyInjectionParameter_ShouldGenerateClassWithDependencyInjectionParameter()
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

            UnitTestHelper.AssertSimilarStrings(expected, @class.ToString());
        }

        [Fact]
        public void ClassGenerator_GivenMultipleDependencyInjectionParameters_ShouldGenerateClassWithDependencyInjectionParameters()
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

            UnitTestHelper.AssertSimilarStrings(expected, @class.ToString());
        }

        [Fact]
        public void ClassGenerator_GivenSingleParameter_ShouldGenerateClassWithParameter()
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

            UnitTestHelper.AssertSimilarStrings(expected, @class.ToString());
        }

        [Fact]
        public void ClassGenerator_GivenMultipleParameters_ShouldGenerateClassWithParameters()
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

            UnitTestHelper.AssertSimilarStrings(expected, @class.ToString());
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
//                .WithClassName("UnitTestClass")
//                .WithModifiers(ClassModifiers.Public)
//                .WithNoUsings()
//                .WithNoAnnotations()
//                .WithNoParameters()
//                .WithMethods(method)
//                .Generate();

//            UnitTestHelper.AssertSimilarStrings(expected, @class.ToString());
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
//                .WithClassName("UnitTestClass")
//                .WithModifiers(ClassModifiers.Public)
//                .WithNoUsings()
//                .WithNoAnnotations()
//                .WithNoParameters()
//                .WithMethods(method, method)
//                .Generate();

//            UnitTestHelper.AssertSimilarStrings(expected, @class.ToString());
//        }
    }
}