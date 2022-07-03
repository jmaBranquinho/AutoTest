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
            var allowedChars = new List<char>();
            if (!_stringBuildingRules.Any())
            {
                _stringBuildingRules.Add(CharacterGroups.All);
            }
            foreach (var rule in _stringBuildingRules)
            {
                switch (rule)
                {
                    case CharacterGroups.UpperCaseLetters:
                        allowedChars.AddRange(GenerateUpperCaseLetters());
                        break;
                    case CharacterGroups.LowerCaseLetters:
                        allowedChars.AddRange(GenerateLowerCaseLetters());
                        break;
                    case CharacterGroups.Numbers:
                        allowedChars.AddRange(GenerateNumbers());
                        break;
                    case CharacterGroups.SpecialCharacters:
                        allowedChars.AddRange(GenerateSpecialCharacters());
                        break;
                    case CharacterGroups.Spaces:
                        allowedChars.Add(' ');
                        break;
                    default:
                        break;
                }
            }
            allowedChars = allowedChars.Except(_excludedCharacters).ToList();

            var size = CalculateStringSize();

            var random = new Random();
            var builder = new StringBuilder();
            for (int i = 0; i < size; i++)
            {
                builder.Append(allowedChars.ElementAt(random.Next(0, allowedChars.Count + 1)));
            }
            return builder.ToString();
        }
    }
}
