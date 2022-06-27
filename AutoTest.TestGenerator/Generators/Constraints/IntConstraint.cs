namespace AutoTest.TestGenerator.Generators.Constraints
{
    public class IntConstraint : ConstraintBase
    {
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
        {
            (int, int) desiredRange = (1, 100);

            if (_minValue <= desiredRange.Item1 && desiredRange.Item1 < _maxValue
                && _minValue <= desiredRange.Item2 && desiredRange.Item2 < _maxValue)
            {
                return (desiredRange.Item1, desiredRange.Item2);
            }

            return (_minValue + 1, _maxValue);
        }
    }
}
