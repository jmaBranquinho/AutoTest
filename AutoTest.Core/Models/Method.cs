using AutoTest.Core.Enums;
using AutoTest.Core.Helpers;
using System.Text;

namespace AutoTest.Core.Models
{
    public class Method
    {
        protected string _name = null!;
        protected IEnumerable<string> _annotations = null!;
        protected IEnumerable<MethodModifiers> _modifiers = null!;
        protected Type? _returnType = null!;
        protected IEnumerable<string> _parameters = null!;
        protected string _body = null!;

        protected bool IsParameterless() => !_parameters?.Any() ?? true;

        public Method(string name, IEnumerable<string> annotations, IEnumerable<MethodModifiers> modifiers, Type? returnType, IEnumerable<ParameterDefinition> parameters, string body)
        {
            InitializeLists(name, annotations, modifiers, returnType, body);
            _parameters = FormatParameters(parameters);
        }

        public Method(string name, IEnumerable<string> annotations, IEnumerable<MethodModifiers> modifiers, Type? returnType, IEnumerable<string> parameters, string body)
        {
            InitializeLists(name, annotations, modifiers, returnType, body);
            _parameters = parameters;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendJoin(Environment.NewLine, _annotations);
            stringBuilder.Append(_annotations.Any() ? Environment.NewLine : string.Empty);
            stringBuilder.Append($"{AddMethodModifiers()} {GetFormattedReturnType()} {_name}".AddNewContext(_parameters.JoinWithComma(), Symbols.Parentheses));

            return stringBuilder.ToString().AddNewContext(_body);
        }

        protected static IEnumerable<string> FormatParameters(IEnumerable<ParameterDefinition> parameters)
        {
            foreach (var parameter in parameters)
            {
                yield return $"{PrimitiveTypeConvertionHelper.GetStringFromType(parameter.Type)} {parameter.Name.FormatAsVariable()}";
            }
        }

        private string AddMethodModifiers() => _modifiers.Select(m => m.ToString().ToLowerInvariant()).JoinWithSpaces();

        private void InitializeLists(string name, IEnumerable<string> annotations, IEnumerable<MethodModifiers> modifiers, Type? returnType, string body)
        {
            _name = name;
            _annotations = annotations;
            _modifiers = modifiers;
            _returnType = returnType;
            _body = body;
        }

        private string GetFormattedReturnType() => _returnType == null ? "void" : _returnType.ToString();
    }
}
