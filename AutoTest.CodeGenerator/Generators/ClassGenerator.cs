using AutoTest.Core.Enums;
using AutoTest.Core.Models;

namespace AutoTest.CodeGenerator.Generators
{
    // TODO add support for namespace
    public class ClassGenerator :
            IClassNameSelectionStage,
            IClassModifiersSelectionStage,
            IClassUsingsSelectionStage,
            IClassPropertiesSelectionStage,
            IClassAnnotationSelectionStage,
            IClassMethodsSelectionStage,
            IClassGenerateSelectionStage
    {
        private string _className;
        private List<ClassModifiers> _modifiers = new();
        private List<string> _usings = new();
        private List<string> _annotations = new();
        private List<(string Name, string Type, bool IsInjected)> _parameters = new();
        private List<Method> _methods = new();

        private ClassGenerator() { }

        public static IClassNameSelectionStage NewClass() => new ClassGenerator();

        public IClassModifiersSelectionStage WithClassName(string name)
        {
            _className = name;
            return this;
        }

        public IClassUsingsSelectionStage WithNoModifiers() => this;

        public IClassUsingsSelectionStage WithModifiers(params ClassModifiers[] modifiers)
        {
            _modifiers.AddRange(modifiers);
            return this;
        }

        public IClassAnnotationSelectionStage WithNoUsings() => this;

        public IClassAnnotationSelectionStage WithUsings(params string[] usings)
        {
            _usings.AddRange(usings);
            return this;
        }

        public IClassPropertiesSelectionStage WithNoAnnotations() => this;

        public IClassPropertiesSelectionStage WithAnnotations(params string[] annotations)
        {
            _annotations.AddRange(annotations);
            return this;
        }

        public IClassMethodsSelectionStage WithNoParameters() => this;

        public IClassMethodsSelectionStage WithParameters(params (string parameter, string type)[] parameters)
        {
            _parameters.AddRange(parameters.Select(p => (p.parameter, p.type, false)));
            return this;
        }

        public IClassMethodsSelectionStage WithDIParameters(params (string parameter, string type)[] parameters)
        {
            _parameters.AddRange(parameters.Select(p => (p.parameter, p.type, true)));
            return this;
        }

        public IClassGenerateSelectionStage WithMethods(params Method[] methods)
        {
            _methods.AddRange(methods);
            return this;
        }

        public IClassGenerateSelectionStage WithNoMethods() => this;

        public Class Generate() => new(_className, @namespace: string.Empty, _usings, _annotations, _modifiers, _parameters, _methods);
    }

    public interface IClassNameSelectionStage
    {
        public IClassModifiersSelectionStage WithClassName(string name);
    }

    public interface IClassModifiersSelectionStage
    {
        public IClassUsingsSelectionStage WithNoModifiers();

        public IClassUsingsSelectionStage WithModifiers(params ClassModifiers[] modifiers);
    }

    public interface IClassUsingsSelectionStage
    {
        public IClassAnnotationSelectionStage WithNoUsings();

        public IClassAnnotationSelectionStage WithUsings(params string[] usings);
    }

    public interface IClassAnnotationSelectionStage
    {
        public IClassPropertiesSelectionStage WithNoAnnotations();

        public IClassPropertiesSelectionStage WithAnnotations(params string[] annotations);
    }

    public interface IClassPropertiesSelectionStage
    {
        public IClassMethodsSelectionStage WithNoParameters();

        public IClassMethodsSelectionStage WithParameters(params (string parameter, string type)[] parameters);

        public IClassMethodsSelectionStage WithDIParameters(params (string parameter, string type)[] parameters);
    }

    public interface IClassMethodsSelectionStage
    {
        public IClassGenerateSelectionStage WithNoMethods();

        public IClassGenerateSelectionStage WithMethods(params Method[] method);
    }

    public interface IClassGenerateSelectionStage
    {
        public Class Generate();
    }
}
