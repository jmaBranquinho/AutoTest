using AutoTest.TestGenerator.Generators.Constraints;
using AutoTest.TestGenerator.Generators.Interfaces;
using Microsoft.CodeAnalysis.CSharp;
using System.ComponentModel;

namespace AutoTest.CodeInterpreter.Analyzers
{
    public static class AnalyzerHelper
    {
        private static readonly Dictionary<Type, Func<IConstraint>> TypeToConstraintDictionary =
            new()
            {
                { typeof(int), () => new IntConstraint() },
                { typeof(double), () => new DoubleConstraint() },
                { typeof(decimal), () => new DecimalConstraint() },
                { typeof(string), () => new StringConstraint() },
            };

        public static IConstraint GetConstraintFromType(Type type) => TypeToConstraintDictionary[type]();

        public static IEnumerable<SyntaxKind> EqualityOperations
            => new List<SyntaxKind>()
            {
                SyntaxKind.EqualsExpression,
                SyntaxKind.NotEqualsExpression,
                SyntaxKind.GreaterThanExpression,
                SyntaxKind.GreaterThanOrEqualExpression,
                SyntaxKind.LessThanExpression,
                SyntaxKind.LessThanOrEqualExpression
            };

        // TODO: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/how-to-determine-whether-a-string-represents-a-numeric-value
        public static Type GetNumericTypeFromValue(object? value)
        {
            try
            {
                Type type = value.GetType();

                while (type.IsNested)
                {
                    type = type.BaseType;
                }

                return type;
            }
            catch (Exception)
            {
                throw new Exception("type not convertible");//TODO
            }
        }

        //public static object ConvertToType<T>(T type, object? input)
        //{
        //    ConvertToType<T>(input, out var output);
        //    return output;
        //}

        public static bool ConvertToType<T>(object? value, out object convertedValue)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                if (converter != null)
                {
                    convertedValue = (T)converter.ConvertFromString(value!.ToString()!)!;
                    return true;
                }
            }
            catch (ArgumentException)
            {
                var nonIntNumberWithoutSuffix = new string(value!.ToString()!.Where(c => char.IsDigit(c)).ToArray());
                return ConvertToType<T>(nonIntNumberWithoutSuffix, out convertedValue);
            }
            catch (NotSupportedException) { }

            convertedValue = default!;
            return false;
        }

        private static bool IsConvertibleToType<T>(T type, object? value) => ConvertToType<T>(value, out var _);

    }
}