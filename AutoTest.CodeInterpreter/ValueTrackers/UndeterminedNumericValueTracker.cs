using System.ComponentModel;

namespace AutoTest.CodeInterpreter.ValueTrackers
{
    public class UnderterminedNumericValue
    {
        public object? InitialValue { get; set; }

        public List<(UndeterminedNumericValueTracker.Operations operation, string value)> Operations { get; set; } = new();
    }

    public class UndeterminedNumericValueTracker : NumericalValueTracker<UnderterminedNumericValue>
    {
        public UndeterminedNumericValueTracker(UnderterminedNumericValue initialValue) : base(initialValue) { }

        public override void Decrement() => Value.Operations.Add((Operations.Subtractation, Unit));

        public override void Increment() => Value.Operations.Add((Operations.Sum, Unit));

        public override object TryConvertValue(Type type) => Convert.ChangeType(Value.InitialValue, type);

        public enum Operations
        {
            Sum,
            Subtractation
        }

        protected const string Unit = "unit";
    }
}
