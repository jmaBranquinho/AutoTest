using AutoTest.CodeGenerator.Helpers;
using System.Text;

namespace AutoTest.CodeGenerator.Models
{
    // TODO
    public class Method
    {
        private string _name;
        private IEnumerable<string> _annotations;
        private IEnumerable<(string Name, string Type)> _parameters;
        private string _body;

        public Method(string name, IEnumerable<string> annotations, IEnumerable<(string Name, string Type)> parameters, string body)
        {
            _name = name;
            _annotations = annotations;
            _parameters = parameters;
            _body = body;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendJoin(Environment.NewLine, _annotations);
            stringBuilder.Append(_annotations.Any() ? Environment.NewLine : string.Empty);
            stringBuilder.Append($"public void {_name}(");
            stringBuilder.AppendJoin(", ", FormatParameters(_parameters));
            stringBuilder.Append(")");

            return stringBuilder.ToString().AddNewContext(_body);
        }

        private static IEnumerable<string> FormatParameters(IEnumerable<(string Name, string Type)> parameters)
        {
            foreach (var parameter in parameters)
            {
                yield return $"{parameter.Type} {parameter.Name.FormatAsVariable()}";
            }
        }
    }
}
