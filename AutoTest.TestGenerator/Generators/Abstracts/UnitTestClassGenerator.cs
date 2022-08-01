using AutoTest.CodeGenerator.Generators;
using AutoTest.Core.Enums;
using AutoTest.Core.Models;

namespace AutoTest.TestGenerator.Generators.Abstracts
{
    public abstract class UnitTestClassGenerator
    {
        protected abstract string ClassAnnotation { get; }

        protected string _className = null!;

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
