using AutoTest.CodeGenerator.Generators;

namespace AutoTest.TestGenerator.Generators.Abstracts
{
    public abstract class UnitTestClassGenerator
    {
        abstract protected string _classAnnotation { get; }

        protected string _className;

        public string Generate()
        {
            return ClassGenerator.NewClass()
                .WithClassName(_className)
                .WithUsings(Enumerable.Empty<string>())
                .WithAnnotation(_classAnnotation)
                //.WithDIParameter()
                //.WithParameter()
                //.WithMethod()
                .Generate();
        }
    }
}
