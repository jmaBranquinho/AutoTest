namespace AutoTest.CodeInterpreter.Interfaces
{
    public interface IValueTracker
    {
        object TryConvertValue(Type type);
        //T TryConvertValue<T>();
    }
}
