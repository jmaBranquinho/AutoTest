using AutoTest.TestGenerator.Generators.Enums;

namespace AutoTest.TestGenerator.Generators.Constraints
{
    public class IntConstraint : NumericalConstraint<int>
    {
        protected override int HumanPreferenceMin => 0;

        protected override int HumanPreferenceMax => 100;

        protected new int _maxValue = int.MaxValue;

        protected new int _minValue = int.MinValue;

        public override int ParseStringToType(string text) => int.Parse(text);

        public override int SumWithType(int value, SumModifications modifier) => value + ResolveSumModifier(modifier);

        public override NumericalConstraint<int> Excluding(params int[] values)
        {
            _exclusions.AddRange(values);
            return this;
        }

        public override NumericalConstraint<int> SetMaxValue(int value)
        {
            _maxValue = value;
            return this;
        }

        public override NumericalConstraint<int> SetMinValue(int value)
        {
            _minValue = value;
            return this;
        }

        protected override (int min, int max) AdjustRangeToHumanPreference()
            => ((HumanPreferenceMin >= _minValue ? HumanPreferenceMin : _minValue), HumanPreferenceMax < _maxValue ? HumanPreferenceMax : _maxValue);

        protected override int GenerateRandomWithExclusions(int min, int max)
        {
            var exclusionsWithoutDuplicates = new HashSet<int>(_exclusions);
            var range = Enumerable.Range(min, max - min).Where(i => !exclusionsWithoutDuplicates.Contains(i)).ToList();

            if(!range.Any())
            {
                range = HandleElseStatements(exclusionsWithoutDuplicates);
            }

            return range.ElementAt(new Random().Next(0, range.Count()));
        }

        protected override int GenerateRandomBetweenRange(int min, int max) => new Random().Next(min, max);

        private List<int> HandleElseStatements(HashSet<int> exclusionsWithoutDuplicates)
        {
            return Enumerable.Range(HumanPreferenceMin, HumanPreferenceMax).Where(i => !exclusionsWithoutDuplicates.Contains(i)).ToList();
            // TODO: handle if range is still 0, need to keep increasing the range - never use max/min int, takes too much memory
        }

        private static int ResolveSumModifier(SumModifications modifier)
            => modifier switch
            {
                SumModifications.IncrementUnit => 1,
                SumModifications.DecrementUnit => -1,
                _ => 0,
            };
    }
}
