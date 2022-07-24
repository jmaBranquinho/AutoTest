namespace AutoTest.Core.Helpers
{
    public static class PrimitiveTypeConvertionHelper
    {
        public static Type GetTypeFromString(string typeAsString) 
            => _typeAndCorrespondingString.TryGetValue(typeAsString, out var @type) 
            ? @type 
            : throw new NotImplementedException();

        public static string GetStringFromType(Type type) 
            => _typeAndCorrespondingString
                    .FirstOrDefault(x => x.Value == type, 
                        new KeyValuePair<string, Type>(type.ToString(), type))
                    .Key;

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

        private static Dictionary<string, Type> _typeAndCorrespondingString = new Dictionary<string, Type>()
        {
            { "int", typeof(int) },
            { "double", typeof(double) },
            { "decimal", typeof(decimal) },
            { "float", typeof(float) },
            { "string", typeof(string) },
            { "char", typeof(char) },
        };
    }
}
