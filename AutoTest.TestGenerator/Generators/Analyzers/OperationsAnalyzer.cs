using AutoTest.TestGenerator.Generators.Constraints;
using AutoTest.TestGenerator.Generators.Interfaces;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTest.TestGenerator.Generators.Analyzers
{
    public static class OperationsAnalyzer
    {
        public static IConstraint GetConstraintFromType(Type type)
        {
            if (type == typeof(int))
            {
                return new IntConstraint();
            }

            throw new NotImplementedException();//TODO
        }

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
            if (IsNumericOperation(kind))
            {
                var NumericalConstraintAnalyzerType = typeof(NumericOperationAnalyzer<>).MakeGenericType(type);
                var analyzer = (INumericalAnalyzer) Activator.CreateInstance(NumericalConstraintAnalyzerType);
                analyzer.AdjustConstraint(constraint, kind, binaryExpression, isElseStatement, operators);
                return;
            }

            throw new NotImplementedException();//TODO
        }

        // TODO migrate IsSupported so no dummy type needs to be passed
        private static bool IsNumericOperation(SyntaxKind kind)
        {
            return NumericOperationAnalyzer<int>.IsSupported(kind);
        }

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
