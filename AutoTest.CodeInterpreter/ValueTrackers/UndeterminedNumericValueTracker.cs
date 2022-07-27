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

        // TODO: this needs to be abstracted
        public override object TryConvertValue(Type type)
        {
            var convertedValue = Convert.ChangeType(Value.InitialValue, typeof(int));
            if (type == typeof(int))
            {
                var convertedValueAsInt = (int)convertedValue;
                foreach (var operation in Value.Operations)
                {
                    if(operation.operation == Operations.Sum)
                    {
                        if(operation.value == "unit")
                        {
                            convertedValueAsInt++;
                        } 
                        else
                        {
                            convertedValueAsInt += (int)Convert.ChangeType(operation.value, typeof(int));
                        }
                    }
                }
                return convertedValueAsInt;
            }

            throw new NotImplementedException();
        }

        //public override T TryConvertValue<T>() => (T) Convert.ChangeType(Value.InitialValue, typeof(T));

        public enum Operations
        {
            Sum,
            Subtractation
        }

        protected const string Unit = "unit";
    }
}
