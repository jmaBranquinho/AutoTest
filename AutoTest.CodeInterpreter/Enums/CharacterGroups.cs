namespace AutoTest.TestGenerator.Generators.Enums
{
    [Flags]
    public enum CharacterGroups
    {
        All = UpperCaseLetters | LowerCaseLetters | Numbers | SpecialCharacters | Spaces,
        UpperCaseLetters = 1,
        LowerCaseLetters = 2,
        Numbers = 4,
        SpecialCharacters = 8,
        Spaces = 16,
    }
}
