﻿using AutoTest.TestGenerator.Generators.Constraints;
using AutoTest.TestGenerator.Generators.Interfaces;
using Microsoft.CodeAnalysis.CSharp;
using System.ComponentModel;

namespace AutoTest.CodeInterpreter.OperationAnalyzers.Helpers
{
    public static class NumericOperationHelper
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

        public static T ConvertToType<T>(object? value)
        {
            _ = ConvertToType<T>(value, out var convertedValue);
            return (T)convertedValue;
        }

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

    }
}