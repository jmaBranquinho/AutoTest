using AutoTest.CodeInterpreter.Enums;
using AutoTest.TestGenerator.Generators.Enums;
using System.Diagnostics;

namespace AutoTest.TestGenerator.Generators.Constraints
{
    public class DecimalConstraint : NumericalConstraint<decimal>
    {
        public override decimal PerformMathOperation(MathOperations mathOperation, decimal value1, decimal value2)
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

        protected override (decimal min, decimal max) AdjustRangeToHumanPreference()
            => ((HumanPreferenceMin >= _minValue ? HumanPreferenceMin : _minValue), HumanPreferenceMax < _maxValue ? HumanPreferenceMax : _maxValue);

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

        protected override decimal GetValueFromAbstraction(AbstractedNumericValues abstractedValue)
            => abstractedValue switch
            {
                AbstractedNumericValues.One => 1m,
                AbstractedNumericValues.MinusOne => -1m,
                _ => 0m,
            };

        protected override void SetMaxAndMinValues()
        {
            _maxValue = decimal.MaxValue;
            _minValue = decimal.MinValue;
        }
    }
}
