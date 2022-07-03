using AutoTest.Core;
using AutoTest.TestGenerator.Generators.Interfaces;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.TestGenerator.Generators.Analyzers
{
    public static class OperationsAnalyzer
    {
        public static void AdjustConstraints(Dictionary<string, IConstraint> constraints, BinaryExpressionSyntax binaryExpression, bool IsElseStatement)
        {
            var operators = GetOperators(binaryExpression);
            var kind = binaryExpression.Kind();
            var (type, constraint, variable) = GetOperationTypeAndConstraint(constraints, operators);

            // TODO Check if constraint is null
            ProcessOperation(kind, binaryExpression, constraint, type, IsElseStatement, operators.Where(op => op != variable).ToList());
        }

        private static void ProcessOperation(SyntaxKind kind, BinaryExpressionSyntax binaryExpression,IConstraint constraint, Type type, bool isElseStatement, IEnumerable<string> operators)
        {
            IOperationsAnalyzer analyzer;
            if (IsNumericOperation(kind, type))
            {
                var NumericalConstraintAnalyzerType = typeof(NumericOperationAnalyzer<>).MakeGenericType(type);
                analyzer = (INumericalAnalyzer) Activator.CreateInstance(NumericalConstraintAnalyzerType);
            }
            else if(IsTextOperation(kind, type))
            {
                analyzer = new TextOperationAnalyzer();
            }
            else
            {
                throw new NotImplementedException();//TODO
            }

            analyzer.AdjustConstraint(constraint, kind, binaryExpression, isElseStatement, operators);

            
        }

        // TODO migrate IsSupported so no dummy type needs to be passed
        private static bool IsNumericOperation(SyntaxKind kind, Type type) => PrimitiveTypeConvertionHelper.NumericalTypes.Contains(type) && NumericOperationAnalyzer<int>.IsSupported(kind);

        private static bool IsTextOperation(SyntaxKind _, Type type) => PrimitiveTypeConvertionHelper.NumericalTypes.Contains(type);

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
