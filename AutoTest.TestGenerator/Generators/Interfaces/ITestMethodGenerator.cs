using AutoTest.CodeInterpreter.Wrappers;

namespace AutoTest.TestGenerator.Generators.Interfaces
{
    public interface ITestMethodGenerator
    {
        string GenerateMethod(string methodName, MethodWrapper method);
    }
}
