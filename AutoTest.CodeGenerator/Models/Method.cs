using AutoTest.CodeGenerator.Enums;

namespace AutoTest.CodeGenerator.Models
{
    public class Method
    {
        public string Name { get; set; }

        public IEnumerable<string> Annotations { get; set; } = Enumerable.Empty<string>();

        public IEnumerable<MethodModifiers> Modifiers { get; set; } = Enumerable.Empty<MethodModifiers>();

        public string ReturnType { get; set; }

        public IEnumerable<(string Name, string Type)> Parameters { get; set; } = Enumerable.Empty<(string Name, string Type)>();

        public string Body { get; set; }


        //protected string _name;
        //protected IEnumerable<string> _annotations;
        //protected IEnumerable<MethodModifiers> _modifiers;
        //protected string _returnType;
        //protected IEnumerable<string> _parameters;
        //protected string _body;

        //protected bool IsParameterless() => !_parameters?.Any() ?? true;

        //public Method(string name, IEnumerable<string> annotations, IEnumerable<MethodModifiers> modifiers, string returnType, IEnumerable<(string Name, Type Type)> parameters, string body)
        //{
        //    InitializeLists(name, annotations, modifiers, returnType, body);
        //    _parameters = FormatParameters(parameters);
        //}

        //public Method(string name, IEnumerable<string> annotations, IEnumerable<MethodModifiers> modifiers, string returnType, IEnumerable<string> parameters, string body)
        //{
        //    InitializeLists(name, annotations, modifiers, returnType, body);
        //    _parameters = parameters;
        //}

        //public override string ToString()
        //{
        //    var stringBuilder = new StringBuilder();
        //    stringBuilder.AppendJoin(Environment.NewLine, _annotations);
        //    stringBuilder.Append(_annotations.Any() ? Environment.NewLine : string.Empty);
        //    stringBuilder.Append($"{AddMethodModifiers()} {_returnType} {_name}".AddNewContext(string.Join(", ", _parameters), Symbols.Parentheses));

        //    return stringBuilder.ToString().AddNewContext(_body);
        //}

        //protected static IEnumerable<string> FormatParameters(IEnumerable<(string Name, Type Type)> parameters)
        //{
        //    foreach (var parameter in parameters)
        //    {
        //        yield return $"{PrimitiveTypeConvertionHelper.GetStringFromType(parameter.Type)} {parameter.Name.FormatAsVariable()}";
        //    }
        //}

        //private string AddMethodModifiers() => string.Join(" ", _modifiers.Select(m => m.ToString().ToLowerInvariant()));

        //private void InitializeLists(string name, IEnumerable<string> annotations, IEnumerable<MethodModifiers> modifiers, string returnType, string body)
        //{
        //    _name = name;
        //    _annotations = annotations;
        //    _modifiers = modifiers;
        //    _returnType = returnType;
        //    _body = body;
        //}
    }
}
