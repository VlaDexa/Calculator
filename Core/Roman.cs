using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Core
{
    public struct Roman : IEquatable<Roman>
    {
        private readonly decimal Value;
        private static readonly Dictionary<char, int> romanNumerals = new Dictionary<char, int>
            {
                { 'I', 1 },
                { 'V', 5 },
                { 'X', 10 },
                { 'L', 50 },
                { 'C', 100 },
                { 'D', 500 },
                { 'M', 1000 },
            };

        public Roman(string input) : this(Parse(input))
        {
        }

        public Roman(decimal input)
        {
            Value = input;
        }

        public static bool TryParse(string input, out Roman roman)
        {
            roman = default;
            var valid = IsValid(input);
            if (valid)
                roman = new Roman(input);
            return valid;
        }

        private static decimal Parse(string input)
        {
            if (!IsValid(input))
                throw new ArgumentException("Given argument is not a valid roman numeral", nameof(input));

            return ParseRoman(input);
        }

        private static decimal ParseRoman(string input)
        {
            decimal value = 0;

            for (int i = 0; i < input.Length; i++)
            {
                char current = input[i];
                char next = i + 1 < input.Length ? input[i + 1] : '\0';

                int currentValue = romanNumerals[current];
                int nextValue = next != '\0' ? romanNumerals[next] : 0;

                if (currentValue >= nextValue)
                    value += currentValue;
                else
                    value -= currentValue;
            }

            return value;
        }

        private static readonly Regex Validator = new Regex("^(?:[IVXLCDM]+)$", RegexOptions.Compiled);

        public static bool IsValid(string input)
        {
            return Validator.IsMatch(input);
        }

        public static Roman operator +(Roman a, Roman b)
        {
            return new Roman(a.Value + b.Value);
        }

        public static Roman operator -(Roman a, Roman b)
        {
            return new Roman(a.Value - b.Value);
        }

        public static Roman operator *(Roman a, Roman b)
        {
            return new Roman(a.Value * b.Value);
        }

        public static Roman operator /(Roman a, Roman b)
        {
            return new Roman(a.Value / b.Value);
        }

        public static Roman operator %(Roman a, Roman b)
        {
            return new Roman(a.Value % b.Value);
        }

        public decimal ToDecimal()
        {
            return Value;
        }

        public override string ToString()
        {
            var encodeNumerals = new Dictionary<string, int>
            {
                {"I", 1},
                {"IV", 4},
                {"V", 5},
                {"IX", 9},
                {"X", 10},
                {"XL", 40},
                {"L", 50},
                {"XC", 90},
                {"C", 100},
                {"CD", 400},
                {"D", 500},
                {"CM", 900},
                {"M", 1000},
            };

            var sb = new StringBuilder();
            var remainder = Value;

            foreach (var item in encodeNumerals.OrderByDescending(x => x.Value))
                while (remainder >= item.Value)
                {
                    sb.Append(item.Key);
                    remainder -= item.Value;
                }

            return sb.ToString();
        }

        public bool Equals(Roman other)
        {
            return Value == other.Value;
        }
    }
}
