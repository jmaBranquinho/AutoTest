using AutoTest.TestGenerator.Generators.Enums;

namespace AutoTest.TestGenerator.Generators.Interfaces
{
    public interface INumericalConstraint<T> : IConstraint
    {
        public T ParseStringToType(string text);

        public T SumToUndeterminedValue(T value, SumModifications modifier = SumModifications.NoModification);

        public void SumToValue(T value);

        public INumericalConstraint<T> SetMaxValue(T value);

        public INumericalConstraint<T> SetMinValue(T value);

        public INumericalConstraint<T> Excluding(params T[] values);

        public INumericalConstraint<T> SetInitialValue(object value);

        public bool IsUndeterminedValue();
    }
}
