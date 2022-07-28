using AutoTest.TestGenerator.Generators.Enums;
using AutoTest.TestGenerator.Generators.Interfaces;

namespace AutoTest.TestGenerator.Generators.Constraints
{
    public abstract class NumericalConstraint<T> : INumericalConstraint<T>
    {
        protected abstract T HumanPreferenceMin { get; }

        protected abstract T HumanPreferenceMax { get; }

        protected T _maxValue;

        protected T _minValue;

        protected T _value;

        protected bool _isValueSet;

        protected List<T> _exclusions = new();

        public abstract T ParseStringToType(string text);

        public abstract T SumToUndeterminedValue(T value, SumModifications modifier = SumModifications.NoModification);

        public abstract void SumToValue(T value);

        public abstract INumericalConstraint<T> SetMaxValue(T value);

        public abstract INumericalConstraint<T> SetMinValue(T value);

        public abstract INumericalConstraint<T> Excluding(params T[] values);

        public abstract INumericalConstraint<T> SetInitialValue(object value);

        public bool IsUndeterminedValue() => !_isValueSet;

        public object Generate()
        {
            if(!IsUndeterminedValue())
            {
                return _value!;
            }

            var (min, max) = AdjustRangeToHumanPreference();
            return _exclusions.Any()
                ? GenerateRandomWithExclusions(min, max)
                : GenerateRandomBetweenRange(min, max);
        }

        public Type GetVariableType() => typeof(T);

        protected abstract (T min, T max) AdjustRangeToHumanPreference();

        protected abstract T GenerateRandomWithExclusions(T min, T max);

        protected abstract T GenerateRandomBetweenRange(T min, T max);
    }
}
