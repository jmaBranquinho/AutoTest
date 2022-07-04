namespace AutoTest.CodeGenerator.Enums
{
    [Flags]
    public enum MethodModifiers
    {
        None,
        Public,
        Protected,
        Private,
        Internal,
        Static,
        Abstract,
        Override,
    }

    public static class MethodModifiersExtensions
    {
        public static bool HasFlag(this MethodModifiers op, MethodModifiers checkflag)
        {
            return (op & checkflag) == checkflag;
        }
    }
}
