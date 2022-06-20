using AutoTest.CodeGenerator.Helpers;
using System.Text;

namespace AutoTest.CodeGenerator.Generators
{
    // TODO: add alternative to change accessability
    // TODO: add alternative to change return type
    public class MethodGenerator :
            IMethodNameSelectionStage,
            IMethodAnnotationSelectionStage,
            IMethodParametersSelectionStage,
            IMethodBodySelectionStage,
            IMethodGenerationSelectionStage
    {
        private string _name;
        private List<string> _annotations;
        private List<(string Name, string Type)> _parameters;
        private string _body;

        private MethodGenerator()
        {
            _annotations = new();
            _parameters = new();
        }

        public static IMethodNameSelectionStage NewMethod() => new MethodGenerator();

        public IMethodAnnotationSelectionStage WithMethodName(string name)
        {
            _name = name;
            return this;
        }

        public IMethodParametersSelectionStage WithNoAnnotations()
        {
            return this;
        }

        public IMethodParametersSelectionStage AddAnnotations(List<string> annotations)
        {
            _annotations = annotations;
            return this;
        }

        public IMethodBodySelectionStage WithNoParameters()
        {
            return this;
        }

        public IMethodBodySelectionStage AddParameters(List<(string Name, string Type)> parameters)
        {
            _parameters = parameters;
            return this;
        }

        public IMethodGenerationSelectionStage AddBody(string body)
        {
            _body = body;
            return this;
        }

        public string Generate()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendJoin(Environment.NewLine, _annotations);
            stringBuilder.Append(_annotations.Any() ? Environment.NewLine : string.Empty);
            stringBuilder.Append($"public void {_name}(");
            stringBuilder.AppendJoin(", ", AddParameters());
            stringBuilder.Append(")");

            return stringBuilder.ToString().AddNewContext(_body);
        }

        private IEnumerable<string> AddParameters()
        {
            foreach (var parameter in _parameters)
            {
                yield return $"{parameter.Type} {parameter.Name.FormatAsVariable()}";
            }
        }
    }

    public interface IMethodNameSelectionStage
    {
        public IMethodAnnotationSelectionStage WithMethodName(string name);
    }

    public interface IMethodAnnotationSelectionStage
    {
        public IMethodParametersSelectionStage WithNoAnnotations();

        public IMethodParametersSelectionStage AddAnnotations(List<string> annotations);
    }

    public interface IMethodParametersSelectionStage
    {
        public IMethodBodySelectionStage AddParameters(List<(string Name, string Type)> parameters);

        public IMethodBodySelectionStage WithNoParameters();
    }

    public interface IMethodBodySelectionStage
    {
        public IMethodGenerationSelectionStage AddBody(string body);
    }

    public interface IMethodGenerationSelectionStage
    {
        public string Generate();
    }
}
