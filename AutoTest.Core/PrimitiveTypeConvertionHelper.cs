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
                "string" => typeof(string),
                _ => throw new NotImplementedException(),
            };

        // TODO use dictionary for both methods and complete
        public static string GetStringFromType(Type type)
        {
            if(typeof(int) == type) { return "int"; }
            return type.ToString();
        }

        // TODO complete list
        public static IEnumerable<Type> NumericalTypes
            => new[]
            {
                typeof(int),
                typeof(double),
                typeof(decimal),
                typeof(float),
                typeof(long),
            };

        // TODO complete list
        public static IEnumerable<Type> TextTypes
            => new[]
            {
                typeof(string),
                typeof(char),
            };

        // TODO
        public static IEnumerable<Type> PrimitiveTypes 
            => new[]
            {
                // value types
                typeof(bool),
                typeof(byte),
                typeof(sbyte),
                typeof(uint),
                typeof(nint),
                typeof(nuint),
                typeof(ulong),
                typeof(short),
                typeof(ushort),
                // reference types
                typeof(object),
                // missing dynamic
            }
            .Concat(NumericalTypes)
            .Concat(TextTypes);
    }
}
