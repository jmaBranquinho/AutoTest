using AutoTest.CodeGenerator.Enums;
using AutoTest.CodeGenerator.Generators;
using AutoTest.CodeGenerator.Models;

namespace AutoTest.TestGenerator.Generators.Abstracts
{
    public abstract class UnitTestClassGenerator
    {
        protected abstract string ClassAnnotation { get; }

        protected string _className;

        public Class Generate() 
            => ClassGenerator.NewClass()
                .WithClassName(_className)
                .WithModifiers(ClassModifiers.Public)
                .WithNoUsings()
                .WithAnnotations(ClassAnnotation)
                .WithNoParameters()
                .WithNoMethods()
                .Generate();
    }
}
