﻿using AutoTest.CodeInterpreter.Models;
using AutoTest.CodeInterpreter.Wrappers;
using AutoTest.TestGenerator.Generators.Abstracts;
using AutoTest.TestGenerator.Generators.UnitTest.XUnit.Models;

namespace AutoTest.TestGenerator.Generators.UnitTest.XUnit
{
    public class XUnitMethodGenerator : UnitTestMethodGenerator
    {
        protected override Func<string, IEnumerable<(string Name, Type Type, object Value)>, CodeRunExecution, Abstracts.UnitTest> GenerateUnitTest(MethodWrapper method)
            => method.Parameters.Any()
                ? ((name, parameters, codeRun) => new XUnitTest(name, parameters, codeRun))
                : ((name, _, codeRun) => new XUnitTest(name, codeRun));
    }
}
