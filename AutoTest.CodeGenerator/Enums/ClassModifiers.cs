namespace AutoTest.CodeGenerator.Enums
{
    [Flags]
    public enum ClassModifiers
    {
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
