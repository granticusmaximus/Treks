using System.Text;

namespace Treks.Shared
{
    public static class Formatters
    {
        public static string? FormatPhone(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            var digits = new StringBuilder();
            foreach (var ch in input)
            {
                if (char.IsDigit(ch))
                    digits.Append(ch);
            }

            var raw = digits.ToString();
            if (raw.Length == 11 && raw.StartsWith("1", StringComparison.Ordinal))
                raw = raw[1..];

            if (raw.Length != 10)
                return input;

            return $"({raw[..3]}) {raw.Substring(3, 3)}-{raw.Substring(6, 4)}";
        }
    }
}
