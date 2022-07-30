using AutoTest.CodeInterpreter.Analyzers;
using AutoTest.CodeInterpreter.Enums;
using AutoTest.TestGenerator.Generators.Enums;
using AutoTest.TestGenerator.Generators.Interfaces;

namespace AutoTest.TestGenerator.Generators.Constraints
{
    public abstract class NumericalConstraint<T> : INumericalConstraint<T>
        where T : struct,
            IComparable,
            IComparable<T>,
            IConvertible,
            IEquatable<T>,
            IFormattable
    {
        public NumericalConstraint() => SetMaxAndMinValues();

        protected int HumanPreferenceMin => 0;

        protected int HumanPreferenceMax => 100;

        protected T _maxValue;

        protected T _minValue;

        protected T _value;

        protected bool _isValueSet;

        protected List<T> _exclusions = new();

        public T ParseStringToType(string text) => NumericHelper.ConvertToType<T>(text);

        public Type GetVariableType() => typeof(T);

        public bool IsUndeterminedValue() => !_isValueSet;

        public void PerformMathOperationOnValue(MathOperations mathOperation, T value) => _value = PerformMathOperation(mathOperation, _value, value);

        public T PerformMathOperation(MathOperations mathOperation, T value1, AbstractedNumericValues value2) 
            => PerformMathOperation(mathOperation, value1, GetValueFromAbstraction(value2));

        public INumericalConstraint<T> SetMaxValue(T value)
        {
            _maxValue = value;
            return this;
        }

        public INumericalConstraint<T> SetMinValue(T value)
        {
            _minValue = value;
            return this;
        }

        public INumericalConstraint<T> Excluding(params T[] values)
        {
            _exclusions.AddRange(values);
            return this;
        }

        public INumericalConstraint<T> SetInitialValue(object value)
        {
            _value = (T)value;
            _isValueSet = true;
            return this;
        }

        public object Generate()
        {
            if (!IsUndeterminedValue())
            {
                return _value!;
            }

            var (min, max) = AdjustRangeToHumanPreference();
            return _exclusions.Any()
                ? GenerateRandomWithExclusions(min, max)
                : GenerateRandomBetweenRange(min, max);
        }

        public abstract T PerformMathOperation(MathOperations mathOperation, T value1, T value2);

        protected abstract (T min, T max) AdjustRangeToHumanPreference();

        protected abstract T GenerateRandomWithExclusions(T min, T max);

        protected abstract T GenerateRandomBetweenRange(T min, T max);

        protected abstract T GetValueFromAbstraction(AbstractedNumericValues abstractedValue);

        protected abstract void SetMaxAndMinValues();
    }
}
