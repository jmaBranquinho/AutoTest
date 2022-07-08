using AutoTest.CodeGenerator.Enums;
using AutoTest.CodeGenerator.Generators;
using AutoTest.CodeGenerator.Models;

namespace AutoTest.TestGenerator.Generators.Abstracts
{
    public abstract class UnitTestClassGenerator
    {
        abstract protected string _classAnnotation { get; }

        protected string _className;

        public Class Generate()
        {
            return ClassGenerator.NewClass()
                .WithClassName(_className)
                .WithModifiers(ClassModifiers.Public)
                .WithNoUsings()
                .WithAnnotations(_classAnnotation)
                .WithNoParameters()
                .WithNoMethods()
                .GenerateClass();
        }
    }
}
