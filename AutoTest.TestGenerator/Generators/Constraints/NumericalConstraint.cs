namespace AutoTest.TestGenerator.Generators.Constraints
{
    public abstract class NumericalConstraint<T> : ConstraintBase
    {
        protected abstract T _humanPreferenceMin { get; }

        protected abstract T _humanPreferenceMax { get; }

        protected T _maxValue;

        protected T _minValue;

        protected List<T> _exclusions = new();

        public abstract NumericalConstraint<T> SetMaxValue(T value);

        public abstract NumericalConstraint<T> SetMinValue(T value);

        public abstract NumericalConstraint<T> Excluding(params T[] values);

        public override object Generate()
        {
            var (min, max) = AdjustRangeToHumanPreference();

            var random = new Random();
            return _exclusions.Any()
                ? GenerateRandomWithExclusions(min, max, random)
                : GenerateRandomBetweenRange(min, max);
        }

        protected abstract (T min, T max) AdjustRangeToHumanPreference();

        protected abstract T GenerateRandomWithExclusions(T min, T max, Random random);

        protected abstract T GenerateRandomBetweenRange(T min, T max);
    }
}
