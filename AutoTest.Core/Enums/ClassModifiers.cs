namespace AutoTest.Core.Enums
{
    [Flags]
    public enum ClassModifiers
    {
        None,
        Public,
        Protected,
        Private,
        Internal,
        Static,
        Abstract,
        Partial,
    }

    public static class ClassModifiersExtensions
    {
        public static bool HasFlag(this ClassModifiers op, ClassModifiers checkflag)
        {
            return (op & checkflag) == checkflag;
        }
    }
}
