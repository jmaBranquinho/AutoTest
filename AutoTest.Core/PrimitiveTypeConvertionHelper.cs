namespace AutoTest.Core
{
    public static class PrimitiveTypeConvertionHelper
    {
        public static Type GetTypeFromString(string type) 
            => type switch
            {
                "int" => typeof(int),
                "double" => typeof(double),
                "decimal" => typeof(decimal),
                _ => throw new NotImplementedException(),
            };

        // TODO complete list
        public static IEnumerable<Type> NumericalTypes
            => new[]
            {
                typeof(int),
                typeof(double),
                typeof(decimal),
            };

        // TODO complete list
        public static IEnumerable<Type> TextTypes
            => new[]
            {
                typeof(string),
                typeof(char),
            };
    }
}
