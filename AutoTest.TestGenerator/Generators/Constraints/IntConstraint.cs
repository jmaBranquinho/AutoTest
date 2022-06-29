namespace AutoTest.TestGenerator.Generators.Constraints
{
    public class IntConstraint : NumericalConstraint<int>
    {
        protected override int _humanPreferenceMin => 0;

        protected override int _humanPreferenceMax => 100;

        protected new int _maxValue = int.MaxValue;

        protected new int _minValue = int.MinValue;

        public override int ParseStringToType(string text) => int.Parse(text);

        public override int SumWithType(int value, int valueToSum) => value + valueToSum;

        public override NumericalConstraint<int> Excluding(params int[] values)
        {
            _exclusions.AddRange(values);
            return this;
        }

        public override NumericalConstraint<int> SetMaxValue(int value)
        {
            if (value < _maxValue)
            {
                _maxValue = value;
            }

            return this;
        }

        public override NumericalConstraint<int> SetMinValue(int value)
        {
            if (value > _minValue)
            {
                _minValue = value;
            }

            return this;
        }

        protected override (int min, int max) AdjustRangeToHumanPreference()
            => ((_humanPreferenceMin >= _minValue ? _humanPreferenceMin : _minValue), _humanPreferenceMax < _maxValue ? _humanPreferenceMax : _maxValue);

        protected override int GenerateRandomWithExclusions(int min, int max, Random random)
        {
            var exclusionsWithoutDuplicates = new HashSet<int>(_exclusions);
            var range = Enumerable.Range(min, max).Where(i => !exclusionsWithoutDuplicates.Contains(i)).ToList();
            return range.ElementAt(random.Next(min, max) - exclusionsWithoutDuplicates.Count);
        }

        protected override int GenerateRandomBetweenRange(int min, int max) => new Random().Next(min, max);
    }
}
