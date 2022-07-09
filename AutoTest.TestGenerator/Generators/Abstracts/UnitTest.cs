using AutoTest.CodeGenerator.Models;

namespace AutoTest.TestGenerator.Generators.Abstracts
{
    public abstract class UnitTest : Method
    {
        public virtual IEnumerable<string> Arrange { get; set; }

        public virtual IEnumerable<string> Act { get; set; }

        public virtual IEnumerable<string> Assert { get; set; }

        public override string ReturnType => "void";

        public override string Body 
            => string.Join(Environment.NewLine, "// Arrange", Arrange, "// Act", Act, "// Assert", Assert);

        public override IEnumerable<string> Annotations => GenerateParameters();

        protected abstract string ParameterlessMethodAnnotation { get; }
        protected abstract string ParameterMethodAnnotation { get; }
        protected abstract string ParameterAnnotationTemplate { get; }

        private IEnumerable<string> GenerateParameters()
        {
            if(Parameters.Any())
            {
                var annotations = new List<string>();
                annotations.Add(ParameterMethodAnnotation);

                //var isNotUsingBuiltInTypes = Parameters.Any(x1 => x1.Any(x2 => !IsBuiltInType(x2.Type)));

                //if (isNotUsingBuiltInTypes)
                //{
                //    // TODO
                //    throw new NotImplementedException();
                //}

                //foreach (var parametersForMethod in Parameters)
                //{
                //    annotations.Add(string.Format(ParameterAnnotationTemplate, string.Join(", ", parametersForMethod.Select(p => p.Value))));
                //}

                return annotations;
            }

            return new List<string> { ParameterlessMethodAnnotation };
        }

        //protected abstract string ParameterlessMethodAnnotation { get; }
        //protected abstract string ParameterMethodAnnotation { get; }
        //protected abstract string ParameterAnnotationTemplate { get; }

        //private readonly IEnumerable<IEnumerable<(string Name, Type Type, object Value)>> _unitTestParameters;

        //public UnitTest(string name, IEnumerable<IEnumerable<(string Name, Type Type, object Value)>> parameters, IEnumerable<StatementWrapper> methodStatements) 
        //    : base(name, Enumerable.Empty<string>(), new List<MethodModifiers> { MethodModifiers.Public }, "void", Enumerable.Empty<(string Name, Type Type) >(), string.Empty)
        //{
        //    var isParameterless = !parameters?.Any() ?? true;
        //    _unitTestParameters = parameters ?? new List<List<(string Name, Type Type, object Value)>>();

        //    _annotations = isParameterless
        //        ? new List<string>() { ParameterlessMethodAnnotation }
        //        : FormatXUnitParameterTestAnnotations(_unitTestParameters);

        //    _parameters = isParameterless
        //        ? Enumerable.Empty<string>()
        //        : FormatXUnitTestMethodParameter(_unitTestParameters);

        //    _body = FormatXUnitTestBody(methodStatements);
        //}

        //private string FormatXUnitTestBody(IEnumerable<StatementWrapper> methodStatements) 
        //    => string.Join(Environment.NewLine, GenerateArrangeSection(), GenerateActSection(methodStatements), GenerateAssertSection(methodStatements));

        //private string GenerateActSection(IEnumerable<StatementWrapper> methodStatements)
        //{
        //    var stringBuilder = new StringBuilder();
        //    stringBuilder.AppendLine("// Act");

        //    if (methodStatements is null || !methodStatements.Any())
        //    {
        //        throw new Exception("no method statements"); // TODO
        //    }

        //    var methodDeclaration = (MethodDeclarationSyntax)methodStatements.First().SyntaxNode;
        //    stringBuilder
        //        .Append($"var actual = _sut.{methodDeclaration.Identifier.Text}"
        //            .AddNewContext(
        //                !IsParameterless() 
        //                ? string.Join(", ", _unitTestParameters.First().Select(p => p.Name).ToList()) 
        //                : string.Empty, Symbols.Parentheses)
        //            .EndStatement(hasLineBreak: true));

        //    return stringBuilder.ToString();
        //}

        //// TODO: implement
        //// TODO: assert property changes
        //private string GenerateAssertSection(IEnumerable<StatementWrapper> methodStatements)
        //{
        //    var stringBuilder = new StringBuilder();
        //    stringBuilder.AppendLine("// Assert");
        //    if (methodStatements is not null && methodStatements.Any())
        //    {
        //        stringBuilder.Append("Assert.Equal(");
        //        stringBuilder.Append("Assert.Equal(expected, actual");
        //        stringBuilder.Append(");");
        //    }

        //    return stringBuilder.ToString();
        //}

        //// TODO: implement
        //private IEnumerable<string> FormatXUnitParameterTestAnnotations(IEnumerable<IEnumerable<(string Name, Type Type, object Value)>> parametersList)
        //{
        //    if (parametersList is null || !parametersList.Any())
        //    {
        //        return new List<string>() { ParameterlessMethodAnnotation };
        //    }

        //    var annotations = new List<string>();
        //    annotations.Add(ParameterMethodAnnotation);

        //    var isNotUsingBuiltInTypes = parametersList.Any(x1 => x1.Any(x2 => !IsBuiltInType(x2.Type)));

        //    if (isNotUsingBuiltInTypes)
        //    {
        //        // TODO
        //        throw new NotImplementedException();
        //    }

        //    foreach (var parametersForMethod in parametersList)
        //    {
        //        annotations.Add(string.Format(ParameterAnnotationTemplate, string.Join(", ", parametersForMethod.Select(p => p.Value))));
        //    }

        //    return annotations;
        //}

        //// TODO: implement
        //private static string GenerateArrangeSection()
        //{
        //    var stringBuilder = new StringBuilder();
        //    stringBuilder.AppendLine("// Arrange");
        //    // TODO: implement

        //    return stringBuilder.ToString();
        //}

        //private static IEnumerable<string> FormatXUnitTestMethodParameter(IEnumerable<IEnumerable<(string Name, Type Type, object Value)>> parametersList) 
        //    => FormatParameters(parametersList.First().Select(p => (p.Name, p.Type)).ToList());

        //private static bool IsBuiltInType(Type type) => PrimitiveTypeConvertionHelper.PrimitiveTypes.Any(t => t == type);
    }
}
