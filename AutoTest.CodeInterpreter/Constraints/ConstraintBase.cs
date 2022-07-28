using AutoTest.TestGenerator.Generators.Interfaces;

namespace AutoTest.TestGenerator.Generators.Constraints
{
    // TODO: split into several constrains under a base or interface
    public abstract class ConstraintBase : IConstraint
    {
        public abstract object Generate();
        public abstract Type GetVariableType();

        public abstract bool IsUndeterminedValue();
    }
}
