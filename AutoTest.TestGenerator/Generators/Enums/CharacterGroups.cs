namespace AutoTest.TestGenerator.Generators.Enums
{
    [Flags]
    public enum CharacterGroups
    {
        UpperCaseLetters = 0,
        LowerCaseLetters = 1,
        Numbers = 2,
        SpecialCharacters = 4,
        Spaces = 8,
        All = UpperCaseLetters | LowerCaseLetters | Numbers | SpecialCharacters | Spaces,
    }
}
