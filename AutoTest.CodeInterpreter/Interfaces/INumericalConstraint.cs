using AutoTest.CodeInterpreter.Enums;
using AutoTest.TestGenerator.Generators.Enums;

namespace AutoTest.TestGenerator.Generators.Interfaces
{
    public interface INumericalConstraint<T> : IConstraint
    {
        T ParseStringToType(string text);

        void PerformMathOperationOnValue(MathOperations mathOperation, T value);

        T PerformMathOperation(MathOperations mathOperation, T value1, AbstractedNumericValues value2);

        T PerformMathOperation(MathOperations mathOperation, T value1, T value2);

        INumericalConstraint<T> SetMaxValue(T value);

        INumericalConstraint<T> SetMinValue(T value);

        INumericalConstraint<T> Excluding(params T[] values);

        INumericalConstraint<T> SetInitialValue(object value);
    }
}
