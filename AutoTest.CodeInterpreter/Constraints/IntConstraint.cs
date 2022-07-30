using AutoTest.CodeInterpreter.Enums;
using AutoTest.TestGenerator.Generators.Enums;

namespace AutoTest.TestGenerator.Generators.Constraints
{
    public class IntConstraint : NumericalConstraint<int>
    {
        public override int PerformMathOperation(MathOperations mathOperation, int value1, int value2)
        {
            return mathOperation switch
            {
                MathOperations.Sum => value1 + value2,
                MathOperations.Multiplication => value1 * value2,
                MathOperations.Subtraction => PerformMathOperation(MathOperations.Sum, value1, -value2),
                MathOperations.Division => PerformMathOperation(MathOperations.Multiplication, value1, 1 / value2),
                _ => throw new NotImplementedException(),
            };
            ;
        }

        protected override (int min, int max) AdjustRangeToHumanPreference()
            => (HumanPreferenceMin >= _minValue ? HumanPreferenceMin : _minValue, HumanPreferenceMax < _maxValue ? HumanPreferenceMax : _maxValue);

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

        protected override int GetValueFromAbstraction(AbstractedNumericValues abstractedValue)
            => abstractedValue switch
            {
                AbstractedNumericValues.One => 1,
                AbstractedNumericValues.MinusOne => -1,
                _ => 0,
            };

        protected override void SetMaxAndMinValues()
        {
            _maxValue = int.MaxValue;
            _minValue = int.MinValue;
        }

        private List<int> HandleElseStatements(HashSet<int> exclusionsWithoutDuplicates)
        {
            return Enumerable.Range(HumanPreferenceMin, HumanPreferenceMax).Where(i => !exclusionsWithoutDuplicates.Contains(i)).ToList();
            // TODO: handle if range is still 0, need to keep increasing the range - never use max/min int, takes too much memory
        }
    }
}
