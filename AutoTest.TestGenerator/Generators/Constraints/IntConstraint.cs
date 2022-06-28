namespace AutoTest.TestGenerator.Generators.Constraints
{
    public class IntConstraint : ConstraintBase
    {
        private const int _humanPreferenceMin = 0;

        private const int _humanPreferenceMax = 100;

        private int _maxValue = int.MaxValue;

        private int _minValue = int.MinValue;

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

        public override object Generate()
        {
            var (min, max) = AdjustRangeToHumanPreference();
            return new Random().Next(min, max);
        }

        private (int min, int max) AdjustRangeToHumanPreference() 
            => ((_humanPreferenceMin >= _minValue ? _humanPreferenceMin : _minValue), _humanPreferenceMax < _maxValue ? _humanPreferenceMax : _maxValue);
    }
}
