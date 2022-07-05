using AutoTest.CodeGenerator.Enums;
using AutoTest.CodeGenerator.Models;

namespace AutoTest.CodeGenerator.Generators
{
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
        private List<string> _methods = new();

        private ClassGenerator() { }

        public static IClassNameSelectionStage NewClass() => new ClassGenerator();

        public IClassModifiersSelectionStage WithClassName(string name)
        {
            _className = name;
            return this;
        }

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

        public IClassGenerateSelectionStage WithMethods(params string[] methods)
        {
            _methods.AddRange(methods);
            return this;
        }

        public IClassGenerateSelectionStage WithNoMethods() => this;

        public string Generate() => new Class(_className, _usings, _annotations, _modifiers, _parameters, _methods).ToString();
    }

    public interface IClassNameSelectionStage
    {
        public IClassModifiersSelectionStage WithClassName(string name);
    }

    public interface IClassModifiersSelectionStage
    {
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

        public IClassGenerateSelectionStage WithMethods(params string[] method);
    }

    public interface IClassGenerateSelectionStage
    {
        public string Generate();
    }
}
