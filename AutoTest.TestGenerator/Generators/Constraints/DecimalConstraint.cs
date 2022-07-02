using AutoTest.TestGenerator.Generators.Enums;
using System.Diagnostics;

namespace AutoTest.TestGenerator.Generators.Constraints
{
    public class DecimalConstraint : NumericalConstraint<decimal>
    {
        protected override decimal _humanPreferenceMin => 0;

        protected override decimal _humanPreferenceMax => 100;

        protected new decimal _maxValue = decimal.MaxValue;

        protected new decimal _minValue = decimal.MinValue;

        public override decimal ParseStringToType(string text) => decimal.Parse(text);

        public override decimal SumWithType(decimal value, SumModifications modifier) => value + ResolveSumModifier(modifier);

        public override NumericalConstraint<decimal> Excluding(params decimal[] values)
        {
            _exclusions.AddRange(values);
            return this;
        }

        public override NumericalConstraint<decimal> SetMaxValue(decimal value)
        {
            if (value < _maxValue)
            {
                _maxValue = value;
            }

            return this;
        }

        public override NumericalConstraint<decimal> SetMinValue(decimal value)
        {
            if (value > _minValue)
            {
                _minValue = value;
            }

            return this;
        }

        protected override (decimal min, decimal max) AdjustRangeToHumanPreference()
            => ((_humanPreferenceMin >= _minValue ? _humanPreferenceMin : _minValue), _humanPreferenceMax < _maxValue ? _humanPreferenceMax : _maxValue);

        protected override decimal GenerateRandomWithExclusions(decimal min, decimal max)
        {
            var exclusionsWithoutDuplicates = new HashSet<decimal>(_exclusions);
            var stopwatch = Stopwatch.StartNew();
            var timeout5Sec = 5000;
            while (stopwatch.ElapsedMilliseconds < timeout5Sec)
            {
                var randomValue = GenerateRandomBetweenRange(min, max);
                if (!exclusionsWithoutDuplicates.Contains(randomValue))
                {
                    return randomValue;
                }
            }
            throw new Exception();//TODO
        }

        protected override decimal GenerateRandomBetweenRange(decimal min, decimal max) => (decimal) new Random().NextDouble() * (max - min) + min;

        private static decimal ResolveSumModifier(SumModifications modifier)
            => modifier switch
            {
                SumModifications.IncrementUnit => (decimal) double.Epsilon,
                SumModifications.DecrementUnit => - (decimal)double.Epsilon,
                _ => 0,
            };
    }
}
