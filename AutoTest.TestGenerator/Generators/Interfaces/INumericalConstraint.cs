namespace AutoTest.TestGenerator.Generators.Interfaces
{
    public interface INumericalConstraint<T> : IConstraint
    {
        public T ParseStringToType(string text);

        public T SumWithType(T value, int valueToSum);

        public INumericalConstraint<T> SetMaxValue(T value);

        public INumericalConstraint<T> SetMinValue(T value);

        public INumericalConstraint<T> Excluding(params T[] values);
    }
}
