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
    }
}
