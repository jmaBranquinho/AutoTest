namespace AutoTest.CodeInterpreter.ValueTrackers
{
    public class IntValueTracker : NumericalValueTracker<int>
    {
        public IntValueTracker(int initialValue) : base(initialValue) { }

        public override void Decrement() => Value--;

        public override void Increment() => Value++;
    }
}
