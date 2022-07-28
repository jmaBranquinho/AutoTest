using AutoTest.Core.Helpers;
using AutoTest.TestGenerator.Generators.Interfaces;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.TestGenerator.Generators.Analyzers
{
    public static class OperationsAnalyzerHelper
    {
        public static void AdjustConstraints(Dictionary<string, IConstraint> constraints, BinaryExpressionSyntax binaryExpression, bool IsElseStatement)
        {
            var operators = GetOperators(binaryExpression);
            var kind = binaryExpression.Kind();
            var (type, constraint, variable) = GetOperationTypeAndConstraint(constraints, operators);

            // TODO Check if constraint is null
            ProcessOperation(kind, binaryExpression, constraint, type, IsElseStatement, operators.Where(op => op != variable).ToList());
        }

        public static void SetInitialValue(Type type, IConstraint constraint, object value) 
            => GetOperationAnalyzer(type).AddInitialValue(constraint, value);

        public static void ModifyKnownValue(Type type, IConstraint constraint, object value)
            => GetOperationAnalyzer(type).ModifyKnownValue(constraint, value);

        private static void ProcessOperation(SyntaxKind kind, BinaryExpressionSyntax binaryExpression, IConstraint constraint, Type type, bool isElseStatement, IEnumerable<string> operators) 
            => GetOperationAnalyzer(type).AdjustConstraint(constraint, kind, binaryExpression, isElseStatement, operators);

        private static IOperationsAnalyzer GetOperationAnalyzer(Type type)
        {
            IOperationsAnalyzer analyzer;
            if (IsNumericOperation(type))
            {
                analyzer = GetNumericalAnalyzer(type);
            }
            else if (IsTextOperation(type))
            {
                analyzer = new TextOperationAnalyzer();
            }
            else
            {
                throw new NotImplementedException();//TODO
            }

            return analyzer;
        }

        private static IOperationsAnalyzer GetNumericalAnalyzer(Type type) 
            => (INumericalAnalyzer)Activator.CreateInstance(typeof(NumericOperationAnalyzer<>).MakeGenericType(type));

        // TODO migrate IsSupported so no dummy type needs to be passed
        private static bool IsNumericOperation(Type type) => PrimitiveTypeConvertionHelper.NumericalTypes.Contains(type);// && NumericOperationAnalyzer<int>.IsSupported(kind);

        private static bool IsTextOperation(Type type) => type == typeof(string) || type == typeof(char);

        private static (Type type, IConstraint? constraint, string variable) GetOperationTypeAndConstraint(Dictionary<string, IConstraint> constraints, IEnumerable<string> operators)
        {
            foreach(var @operator in operators)
            {
                if(constraints.TryGetValue(@operator, out var constraint)) {
                    return (constraint.GetVariableType(), constraint, @operator);
                }
            }

            throw new NotImplementedException();//TODO
        }

        // TODO: this may need to be refactored in the future for less and more operators
        private static IEnumerable<string> GetOperators(BinaryExpressionSyntax binaryExpression)
        {
            var operators = new List<string>
            {
                binaryExpression.Left.GetText().ToString().Trim(),
                binaryExpression.Right.GetText().ToString().Trim(),
            };

            return operators.ToList();
        }
    }
}
