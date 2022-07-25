﻿using AutoTest.CodeInterpreter.Models;
using AutoTest.CodeInterpreter.Wrappers;

namespace AutoTest.TestGenerator.Generators.UnitTest.NUnit.Models
{
    public class NUnitTest : Abstracts.UnitTest
    {
        protected override string ParameterlessMethodAnnotation => "[Test]";
        protected override string ParameterMethodAnnotation => string.Empty;
        protected override string ParameterAnnotationTemplate => "[TestCase({0})]";

        public NUnitTest(string name, IEnumerable<(string Name, Type Type, object Value)> parameters, CodeRunExecution codeRun) : base(name, parameters, codeRun)
        { }

        public NUnitTest(string name, CodeRunExecution codeRun) : base(name, Enumerable.Empty<(string Name, Type Type, object Value)>(), codeRun)
        { }
    }
}
