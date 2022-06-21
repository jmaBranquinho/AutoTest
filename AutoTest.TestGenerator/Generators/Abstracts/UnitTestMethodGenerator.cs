using AutoTest.CodeGenerator.Generators;
using AutoTest.CodeInterpreter.Wrappers;
using AutoTest.TestGenerator.Generators.Interfaces;

namespace AutoTest.TestGenerator.Generators.Abstracts
{
    public abstract class UnitTestMethodGenerator : ITestMethodGenerator
    {
        abstract protected string _parameterlessMethodAnnotation { get; }
        abstract protected string _parameterMethodAnnotation { get; }
        abstract protected string _parameterAnnotationTemplate { get; }

        public abstract string GenerateMethod(string methodName, MethodWrapper method);

        protected string GenerateMethod(string methodName, string methodBody, params (string Name, string Type, string Value)[] parameters)
        {
            var hasParameters = parameters.Count() > 0;
            var annotations = new List<string> { hasParameters ? _parameterMethodAnnotation : _parameterlessMethodAnnotation };
            // TODO: currently supporting only 1 scenario. Params only supports 1 dim array
            // TODO: currently only supports built-in types
            if (hasParameters)
            {
                annotations.AddRange(parameters.Select(p => string.Format(_parameterAnnotationTemplate, $"{p.Type} {p.Name}")));
            }

            var methodStage1 = MethodGenerator.NewMethod()
                .WithMethodName(methodName)
                .AddAnnotations(annotations);

            var methodStage2 = parameters.Length > 0 
                ? methodStage1.AddParameters(parameters.Select(x => (x.Name, x.Type)).ToList()) 
                : methodStage1.WithNoParameters();

            return methodStage2.AddBody(methodBody).Generate();
        }

        private static bool IsBuiltInType(string type) => BuiltInTypes.Any(t => t.Name == type);

        private static List<Type> BuiltInTypes = new()
        {
            // value types
            typeof(bool),
            typeof(byte),
            typeof(sbyte),
            typeof(char),
            typeof(decimal),
            typeof(double),
            typeof(float),
            typeof(int),
            typeof(uint),
            typeof(nint),
            typeof(nuint),
            typeof(long),
            typeof(ulong),
            typeof(short),
            typeof(ushort),
            // reference types
            typeof(object),
            typeof(string),
            // missing dynamic
        };
    }
}
