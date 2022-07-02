﻿using AutoTest.TestGenerator.Generators.Interfaces;

namespace AutoTest.TestGenerator.Generators.Constraints
{
    public abstract class NumericalConstraint<T> : INumericalConstraint<T>
    {
        protected abstract T _humanPreferenceMin { get; }

        protected abstract T _humanPreferenceMax { get; }

        protected T _maxValue;

        protected T _minValue;

        protected List<T> _exclusions = new();

        public abstract T ParseStringToType(string text);

        public abstract T SumWithType(T value, int valueToSum);

        public abstract INumericalConstraint<T> SetMaxValue(T value);

        public abstract INumericalConstraint<T> SetMinValue(T value);

        public abstract INumericalConstraint<T> Excluding(params T[] values);

        public object Generate()
        {
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
