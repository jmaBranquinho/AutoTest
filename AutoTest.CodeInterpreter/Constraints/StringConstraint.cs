using AutoTest.TestGenerator.Generators.Enums;
using AutoTest.TestGenerator.Generators.Interfaces;
using System.Text;

namespace AutoTest.TestGenerator.Generators.Constraints
{
    public class StringConstraint : IConstraint
    {
        private readonly int _defaultStringSize = 10;

        private readonly int _defaultStringSizeIncreased = 1000;

        private new int? _maxValue;

        private new int? _minValue;

        private List<string> _excludedStrings = new();

        private List<char> _excludedCharacters = new();

        private List<CharacterGroups> _stringBuildingRules = new();

        private string _exactValue;

        public StringConstraint SetMaxLength(int value)
        {
            if (_maxValue == null || value < _maxValue)
            {
                _maxValue = value;
            }

            return this;
        }

        public StringConstraint SetMinLength(int value)
        {
            if (value > 0 && (_minValue == null || value > _minValue))
            {
                _minValue = value;
            }

            return this;
        }

        public StringConstraint Excluding(params string[] values)
        {
            _excludedStrings.AddRange(values);
            return this;
        }

        public StringConstraint Excluding(params char[] values)
        {
            _excludedCharacters.AddRange(values);
            return this;
        }

        public StringConstraint WithRules(params CharacterGroups[] values)
        {
            _stringBuildingRules.AddRange(values);
            return this;
        }

        public StringConstraint Exactly(string value)
        {
            _exactValue = value;
            return this;
        }

        public object Generate()
        {
            if (!string.IsNullOrWhiteSpace(_exactValue))
            {
                return _exactValue;
            }

            string result;
            do
            {
                result = GenerateRandomString();
            } 
            while (_excludedStrings.Contains(result));
            return result;
        }

        public Type GetVariableType() => typeof(string);

        private IEnumerable<char> GenerateUpperCaseLetters()
            => Enumerable.Range('A', 'Z' - 'A' + 1).Select(c => (char)c).ToList();

        private IEnumerable<char> GenerateLowerCaseLetters()
            => Enumerable.Range('a', 'z' - 'a' + 1).Select(c => (char)c).ToList();

        private IEnumerable<char> GenerateNumbers()
            => Enumerable.Range(0, 9).Select(c => (char)c).ToList();

        private IEnumerable<char> GenerateSpecialCharacters()
            => new List<char> { '!', '"', '#', '$', '%', '&', '\'', '(', ')', '*', '+', 
                ',', '.', '/', ':', ';', '<', '>', '=', '?', '@', '[', ']', '\\', '{', '}',
                '|', '_', '^', '~',
            };

        private int CalculateStringSize()
        {
            if(_maxValue.HasValue && _minValue.HasValue)
            {
                return new Random().Next(_minValue.Value, _maxValue.Value + 1);
            }
            else if(_maxValue.HasValue)
            {
                return new Random().Next(1, _maxValue.Value + 1);
            }
            else if (_minValue.HasValue)
            {
                if(_minValue.Value > _defaultStringSize)
                {
                    return new Random().Next(_minValue.Value, _defaultStringSizeIncreased);
                } 
                else
                {
                    return new Random().Next(_minValue.Value, _defaultStringSize);
                }
            }

            return new Random().Next(1, _defaultStringSize);
        }

        private string GenerateRandomString()
        {

            if (!_stringBuildingRules.Any())
            {
                _stringBuildingRules.Add(CharacterGroups.All);
            }

            var allowedChars = GenerateAllowedCharacterList();

            var stringSize = CalculateStringSize();

            var random = new Random();
            var builder = new StringBuilder();
            for (int i = 0; i < stringSize; i++)
            {
                builder.Append(allowedChars.ElementAt(random.Next(0, allowedChars.Count())));
            }
            return builder.ToString();
        }

        private IEnumerable<char> GenerateAllowedCharacterList()
        {
            var allowedChars = new List<char>();
            foreach (var rule in _stringBuildingRules)
            {
                if (HasKey(rule, CharacterGroups.UpperCaseLetters))
                {
                    allowedChars.AddRange(GenerateUpperCaseLetters());
                }
                if (HasKey(rule, CharacterGroups.LowerCaseLetters))
                {
                    allowedChars.AddRange(GenerateLowerCaseLetters());
                }
                if (HasKey(rule, CharacterGroups.Numbers))
                {
                    allowedChars.AddRange(GenerateNumbers());
                }
                if (HasKey(rule, CharacterGroups.SpecialCharacters))
                {
                    allowedChars.AddRange(GenerateSpecialCharacters());
                }
                if (HasKey(rule, CharacterGroups.Spaces))
                {
                    allowedChars.Add(' ');
                }
            }
            return allowedChars.Except(_excludedCharacters).ToList();
        }

        private static bool HasKey(CharacterGroups value, CharacterGroups flag) => (value & flag) != 0;

        public bool IsUndeterminedValue()
        {
            throw new NotImplementedException();
        }
    }
}
