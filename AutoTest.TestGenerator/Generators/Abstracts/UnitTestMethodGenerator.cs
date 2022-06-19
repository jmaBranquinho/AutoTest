namespace AutoTest.TestGenerator.Generators.Abstracts
{
    public abstract class UnitTestMethodGenerator
    {
        abstract protected string _parameterlessMethodAnnotation { get; }
        abstract protected string _parameterMethodAnnotation { get; }
        abstract protected string _parameterAnnotationTemplate { get; }

        protected string NewParameterlessMethod(string methodBody) => $"{_parameterlessMethodAnnotation}{Environment.NewLine}{methodBody}";

        // TODO: currently supporting only 1 scenario. Params only supports 1 dim array
        protected string NewParameterMethod(string methodBody, params (string Name, string Type, string Value)[] parameters)
        {
            string formattedParameters;

            var areAllBuiltInTypes = !parameters.Any(p => !IsBuiltInType(p.Type));

            if (areAllBuiltInTypes)
            {
                formattedParameters = string.Format(_parameterAnnotationTemplate, string.Join(", ", parameters.Select(p => p.Value)));
            }
            else
            {
                // TODO: implement this logic
                throw new NotImplementedException();
            }

            return $"{_parameterMethodAnnotation}{Environment.NewLine}{formattedParameters}{methodBody}";
        }

        private bool IsBuiltInType(string type) => BuiltInTypes.Any(t => t.Name == type);

        private static List<Type> BuiltInTypes = new()
        {
            // value types
            typeof(bool),
            typeof(byte),
            typeof(sbyte),
            typeof(char),
            typeof(decimal),
            typeof(double),
            typeof(float),
            typeof(int),
            typeof(uint),
            typeof(nint),
            typeof(nuint),
            typeof(long),
            typeof(ulong),
            typeof(short),
            typeof(ushort),
            // reference types
            typeof(object),
            typeof(string),
            // missing dynamic
        };
    }
}
