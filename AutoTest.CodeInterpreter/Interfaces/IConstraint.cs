namespace AutoTest.TestGenerator.Generators.Interfaces
{
    public interface IConstraint
    {
        Type GetVariableType();

        bool IsUndeterminedValue();

        object Generate();

    }
}
