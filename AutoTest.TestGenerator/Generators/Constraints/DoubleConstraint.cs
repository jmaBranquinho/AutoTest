using AutoTest.TestGenerator.Generators.Enums;
using System.Diagnostics;

namespace AutoTest.TestGenerator.Generators.Constraints
{
    public class DoubleConstraint : NumericalConstraint<double>
    {
        protected override double HumanPreferenceMin => 0;

        protected override double HumanPreferenceMax => 100;

        protected new double _maxValue = double.MaxValue;

        protected new double _minValue = double.MinValue;

        public override double ParseStringToType(string text) => double.Parse(text);

        public override double SumWithType(double value, SumModifications modifier) => value + ResolveSumModifier(modifier);

        public override NumericalConstraint<double> Excluding(params double[] values)
        {
            _exclusions.AddRange(values);
            return this;
        }

        public override NumericalConstraint<double> SetMaxValue(double value)
        {
            if (value < _maxValue)
            {
                _maxValue = value;
            }

            return this;
        }

        public override NumericalConstraint<double> SetMinValue(double value)
        {
            if (value > _minValue)
            {
                _minValue = value;
            }

            return this;
        }

        protected override (double min, double max) AdjustRangeToHumanPreference()
            => ((HumanPreferenceMin >= _minValue ? HumanPreferenceMin : _minValue), HumanPreferenceMax < _maxValue ? HumanPreferenceMax : _maxValue);

        protected override double GenerateRandomWithExclusions(double min, double max)
        {
            var exclusionsWithoutDuplicates = new HashSet<double>(_exclusions);
            var stopwatch = Stopwatch.StartNew();
            var timeout5Sec = 5000;
            while (stopwatch.ElapsedMilliseconds < timeout5Sec)
            {
                var randomValue = GenerateRandomBetweenRange(min, max);
                if(!exclusionsWithoutDuplicates.Contains(randomValue))
                {
                    return randomValue;
                }
            }
            throw new Exception();//TODO
        }

        protected override double GenerateRandomBetweenRange(double min, double max) => new Random().NextDouble() * (max - min) + min;

        private static double ResolveSumModifier(SumModifications modifier) 
            => modifier switch
            {
                SumModifications.IncrementUnit => double.Epsilon,
                SumModifications.DecrementUnit => -double.Epsilon,
                _ => 0,
            };
    }
}
