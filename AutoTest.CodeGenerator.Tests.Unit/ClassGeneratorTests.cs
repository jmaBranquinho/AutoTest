using AutoTest.CodeGenerator.Generators;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
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
".Trim();

            var @class = ClassGenerator.NewClass()
                .WithClassName("UnitTestClass")
                .WithAnnotations(Enumerable.Empty<string>())
                .Generate();

            expected = ReOrderReformatSpaces(expected);
            @class = ReOrderReformatSpaces(@class);

            @class.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void SingleUsing()
        {
            var expected = @"
using System;

public class UnitTestClass
{
    
}
".Trim();

            var @class = ClassGenerator.NewClass()
                .WithClassName("UnitTestClass")
                .WithUsing("using System;")
                .WithAnnotations(Enumerable.Empty<string>())
                .Generate();

            expected = ReOrderReformatSpaces(expected);
            @class = ReOrderReformatSpaces(@class);

            @class.Should().BeEquivalentTo(expected);
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
".Trim();

            var @class = ClassGenerator.NewClass()
                .WithClassName("UnitTestClass")
                .WithUsings(new List<string> { "using System;", "using System.Linq;", "using Xunit;" } )
                .WithAnnotations(Enumerable.Empty<string>())
                .Generate();

            expected = ReOrderReformatSpaces(expected);
            @class = ReOrderReformatSpaces(@class);

            @class.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void SingleAnnotation()
        {
            var expected = @"
[SomeAnnotation]
public class UnitTestClass
{
    
}
".Trim();

            var @class = ClassGenerator.NewClass()
                .WithClassName("UnitTestClass")
                .WithAnnotation("[SomeAnnotation]")
                .Generate();

            expected = ReOrderReformatSpaces(expected);
            @class = ReOrderReformatSpaces(@class);

            @class.Should().BeEquivalentTo(expected);
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
".Trim();

            var @class = ClassGenerator.NewClass()
                .WithClassName("UnitTestClass")
                .WithAnnotations(new List<string> { "[SomeAnnotation1]", "[SomeAnnotation2]", "[SomeAnnotation3]" })
                .Generate();

            expected = ReOrderReformatSpaces(expected);
            @class = ReOrderReformatSpaces(@class);

            @class.Should().BeEquivalentTo(expected);
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
".Trim();

            var @class = ClassGenerator.NewClass()
                .WithClassName("UnitTestClass")
                .WithAnnotations(Enumerable.Empty<string>())
                .WithDIParameter("param1", "int")
                .Generate();

            expected = ReOrderReformatSpaces(expected);
            @class = ReOrderReformatSpaces(@class);

            @class.Should().BeEquivalentTo(expected);
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
".Trim();

            var @class = ClassGenerator.NewClass()
                .WithClassName("UnitTestClass")
                .WithAnnotations(Enumerable.Empty<string>())
                .WithDIParameter("param1", "int")
                .WithDIParameter("param2", "SomeObj")
                .Generate();

            expected = ReOrderReformatSpaces(expected);
            @class = ReOrderReformatSpaces(@class);

            @class.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void SingleParameter()
        {
            var expected = @"
public class UnitTestClass
{
    private int _param1;
    
    
}
".Trim();

            var @class = ClassGenerator.NewClass()
                .WithClassName("UnitTestClass")
                .WithAnnotations(Enumerable.Empty<string>())
                .WithParameter("param1", "int")
                .Generate();

            expected = ReOrderReformatSpaces(expected);
            @class = ReOrderReformatSpaces(@class);

            @class.Should().BeEquivalentTo(expected);
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
".Trim();

            var @class = ClassGenerator.NewClass()
                .WithClassName("UnitTestClass")
                .WithAnnotations(Enumerable.Empty<string>())
                .WithParameter("param1", "int")
                .WithParameter("param2", "SomeObj")
                .Generate();

            expected = ReOrderReformatSpaces(expected);
            @class = ReOrderReformatSpaces(@class);

            @class.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void SingleMethod()
        {
            var method = @"
public int Return1()
{
    return 1;
}
".Trim();

            var expected = @"
public class UnitTestClass
{
    public int Return1()
    {
        return 1;
    }
}
".Trim();

            var @class = ClassGenerator.NewClass()
                .WithClassName("UnitTestClass")
                .WithAnnotations(Enumerable.Empty<string>())
                .WithMethod(method)
                .Generate();

            expected = ReOrderReformatSpaces(expected);
            @class = ReOrderReformatSpaces(@class);

            @class.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void MultipleMethod()
        {
            var method = @"
public int Return1()
{
    return 1;
}
".Trim();

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
".Trim();

            var @class = ClassGenerator.NewClass()
                .WithClassName("UnitTestClass")
                .WithAnnotations(Enumerable.Empty<string>())
                .WithMethod(method)
                .WithMethod(method)
                .Generate();

            expected = ReOrderReformatSpaces(expected);
            @class = ReOrderReformatSpaces(@class);

            @class.Should().BeEquivalentTo(expected);
        }

        // TODO: extract to some helper class
        private string ReOrderReformatSpaces(string text) => text
            .Replace("    ", "\t")
            .Replace("\r\n\t", "\t\r\n")
            .Replace("\r\n\t\r\n", "\t\r\n\r\n")
            .Replace("\t\r\n\t", "\t\t\r\n");
    }
}