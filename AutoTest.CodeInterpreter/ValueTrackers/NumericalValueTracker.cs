using AutoTest.CodeInterpreter.Interfaces;

namespace AutoTest.CodeInterpreter.ValueTrackers
{
    public abstract class NumericalValueTracker<T> : IValueTracker
    {
        public NumericalValueTracker(T initialValue) => Value = initialValue;

        public T Value { get; protected set; }

        public abstract void Increment();

        public abstract void Decrement();

        public virtual object TryConvertValue(Type type) => throw new NotImplementedException();

        //public abstract void Sum();

        //public abstract void Subtract();
    }
}
