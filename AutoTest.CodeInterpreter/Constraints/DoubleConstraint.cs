using AutoTest.CodeInterpreter.Enums;
using AutoTest.TestGenerator.Generators.Enums;
using System.Diagnostics;

namespace AutoTest.TestGenerator.Generators.Constraints
{
    public class DoubleConstraint : NumericalConstraint<double>
    {
        public override double PerformMathOperation(MathOperations mathOperation, double value1, double value2)
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

        protected override double GetValueFromAbstraction(AbstractedNumericValues abstractedValue)
            => abstractedValue switch
            {
                AbstractedNumericValues.One => 1d,
                AbstractedNumericValues.MinusOne => -1d,
                _ => 0d,
            };
        protected override void SetMaxAndMinValues()
        {
            _maxValue = double.MaxValue;
            _minValue = double.MinValue;
        }
    }
}
