namespace AutoTest.TestGenerator.Generators.Interfaces
{
    public interface IConstraint
    {
        public Type GetVariableType();
        public object Generate();
    }
}
