using AutoTest.CodeInterpreter;
using AutoTest.CodeInterpreter.Models.Wrappers;
using AutoTest.TestGenerator.Generators.UnitTest.XUnit;
using System.Collections.Generic;
using System.Linq;

namespace AutoTest.TestGenerator.Tests.Unit.TestMethodGeneration.XUnit
{
    public partial class XUnitMethodGeneratorTests
    {
        private readonly XUnitMethodGenerator _sut;

        public XUnitMethodGeneratorTests() => _sut = new XUnitMethodGenerator();

        private static MethodWrapper GetMethodFromExample(string exampleCode) => GetMethodsFromExample(exampleCode).First();

        private static IEnumerable<MethodWrapper> GetMethodsFromExample(string exampleCode)
        {
            var analyzer = new CodeAnalyzer();
            var solution = analyzer.AnalyzeCode(_classAndNamespaceWrapperTemplate.Replace("{0}", exampleCode));
            return solution.Namespaces.First().Classes.First().Methods;
        }

        private static string _classAndNamespaceWrapperTemplate = @"
namespace TestNameSpace
{
    public class TestClass
    {
        {0}
    }
}
".Trim();

        private static string _simpleMethodWith1ParameterIfStatementType1 = @"
        public int TestMethod(int x) 
        {
            if (x > 5)
            {
                return x;
            } 
            else
            {
                return 0;
            }
        }
".Trim();

        private static string _simpleMethodWith1ParameterIfStatementType2 = @"
        public int TestMethod(int x) 
        {
            if (x > 5) 
            {
                return x;
            }
            return 0;
        }
".Trim();

        private static string _simpleMethodWith1ParameterIfStatementType3 = @"
        public int TestMethod(int x) 
        {
            return x > 5 ? x : 0;
        }
".Trim();
    }
}