using AutoTest.CodeGenerator.Enums;
using AutoTest.CodeGenerator.Helpers;
using AutoTest.CodeGenerator.Models;
using System.Text;

namespace AutoTest.CodeGenerator.Generators
{
    public class MethodGenerator :
            IMethodNameSelectionStage,
            IMethodAnnotationSelectionStage,
            IMethodModifiersSelectionStage,
            IMethodReturnTypeSelectionStage,
            IMethodParametersSelectionStage,
            IMethodBodySelectionStage,
            IMethodGenerationSelectionStage
    {
        private string _methodName;
        private List<string> _annotations = new();
        private List<MethodModifiers> _modifiers = new();
        private string _returnType;
        private List<(string Name, string Type)> _parameters = new();
        private string _body;

        private MethodGenerator() { }

        public static IMethodNameSelectionStage NewMethod() => new MethodGenerator();

        public IMethodAnnotationSelectionStage WithMethodName(string name)
        {
            _methodName = name;
            return this;
        }

        public IMethodModifiersSelectionStage WithNoAnnotations() => this;

        public IMethodModifiersSelectionStage WithAnnotations(params string[] annotations)
        {
            _annotations.AddRange(annotations);
            return this;
        }

        public IMethodReturnTypeSelectionStage WithNoModifiers() => this;

        public IMethodReturnTypeSelectionStage WithModifiers(params MethodModifiers[] modifiers)
        {
            _modifiers.AddRange(modifiers);
            return this;
        }

        public IMethodParametersSelectionStage WithReturnType(Type? returnType)
        {
            _returnType = returnType?.ToString() ?? "void";
            return this;
        }

        public IMethodBodySelectionStage WithNoParameters() => this;

        public IMethodBodySelectionStage WithParameters(params (string Name, string Type)[] parameters)
        {
            _parameters.AddRange(parameters);
            return this;
        }

        public IMethodGenerationSelectionStage WithBody(string body)
        {
            _body = body;
            return this;
        }

        public Method Generate() => new(_methodName, _annotations, _modifiers, _returnType, AddParameters(), _body);

        private IEnumerable<string> AddParameters()
        {
            foreach (var parameter in _parameters)
            {
                yield return $"{parameter.Type} {parameter.Name.FormatAsVariable()}";
            }
        }

        private string AddMethodModifiers() => string.Join(" ", _modifiers.Select(m => m.ToString().ToLowerInvariant()));
    }

    public interface IMethodNameSelectionStage
    {
        public IMethodAnnotationSelectionStage WithMethodName(string name);
    }

    public interface IMethodAnnotationSelectionStage
    {
        public IMethodModifiersSelectionStage WithNoAnnotations();

        public IMethodModifiersSelectionStage WithAnnotations(params string[] annotations);
    }

    public interface IMethodModifiersSelectionStage
    {
        public IMethodReturnTypeSelectionStage WithNoModifiers();

        public IMethodReturnTypeSelectionStage WithModifiers(params MethodModifiers[] modifiers);
    }

    public interface IMethodReturnTypeSelectionStage
    {
        public IMethodParametersSelectionStage WithReturnType(Type? returnType);
    }

    public interface IMethodParametersSelectionStage
    {
        public IMethodBodySelectionStage WithParameters(params (string Name, string Type)[] parameters);

        public IMethodBodySelectionStage WithNoParameters();
    }

    public interface IMethodBodySelectionStage
    {
        public IMethodGenerationSelectionStage WithBody(string body);
    }

    public interface IMethodGenerationSelectionStage
    {
        public Method Generate();
    }
}
