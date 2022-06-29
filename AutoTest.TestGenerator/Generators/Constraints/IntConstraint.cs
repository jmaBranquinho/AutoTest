namespace AutoTest.TestGenerator.Generators.Constraints
{
    public class IntConstraint : ConstraintBase
    {
        private const int _humanPreferenceMin = 0;

        private const int _humanPreferenceMax = 100;

        private int _maxValue = int.MaxValue;

        private int _minValue = int.MinValue;

        private List<int> _exclusions = new List<int>();

        public IntConstraint SetMaxValue(int value)
        {
            if (value < _maxValue)
            {
                _maxValue = value;
            }

            return this;
        }

        public IntConstraint SetMinValue(int value)
        {
            if (value > _minValue)
            {
                _minValue = value;
            }

            return this;
        }

        public IntConstraint Excluding(params int[] values)
        {
            _exclusions.AddRange(values);
            return this;
        }

        public override object Generate()
        {
            var (min, max) = AdjustRangeToHumanPreference();

            var random = new Random();
            return _exclusions.Any() 
                ? GenerateRandomWithExclusions(min, max, random)
                : random.Next(min, max);
        }

        private int GenerateRandomWithExclusions(int min, int max, Random random)
        {
            var exclusionsWithoutDuplicates = new HashSet<int>(_exclusions);
            var range = Enumerable.Range(min, max).Where(i => !exclusionsWithoutDuplicates.Contains(i)).ToList();
            return range.ElementAt(random.Next(min, max) - exclusionsWithoutDuplicates.Count);
        }

        private (int min, int max) AdjustRangeToHumanPreference() 
            => ((_humanPreferenceMin >= _minValue ? _humanPreferenceMin : _minValue), _humanPreferenceMax < _maxValue ? _humanPreferenceMax : _maxValue);
    }
}
