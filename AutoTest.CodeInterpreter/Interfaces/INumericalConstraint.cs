using AutoTest.TestGenerator.Generators.Enums;

namespace AutoTest.TestGenerator.Generators.Interfaces
{
    public interface INumericalConstraint<T> : IConstraint
    {
        public T ParseStringToType(string text);

        public T SumWithType(T value, SumModifications modifier = SumModifications.NoModification);

        public INumericalConstraint<T> SetMaxValue(T value);

        public INumericalConstraint<T> SetMinValue(T value);

        public INumericalConstraint<T> Excluding(params T[] values);
    }
}
