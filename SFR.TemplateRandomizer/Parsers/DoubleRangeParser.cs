using System.Globalization;
using System.Text.RegularExpressions;

namespace SFR.TemplateRandomizer.Parsers
{
    internal class DoubleRangeParser : IArgumentParser<(double start, double end)>
    {
        private readonly Regex rangeRegex = new(@"(?<start>\d+,?\d*)(\.{2})?(?<end>\d+,?\d*)", RegexOptions.Compiled);
        private readonly double defaultMin;
        private readonly double defaultMax;

        public DoubleRangeParser(double defaultMin = double.MinValue, double defaultMax = double.MaxValue)
        {
            this.defaultMin = defaultMin;
            this.defaultMax = defaultMax;
        }

        public (double start, double end) Parse(string input)
        {
            if (input is null)
                return (defaultMin, defaultMax);

            try
            {
                var match = rangeRegex.Match(input);

                var start = double.Parse(match.Groups["start"].Value, NumberStyles.Float);
                var end = match.Groups["end"].Value == string.Empty ? this.defaultMax : double.Parse(match.Groups["end"].Value, NumberStyles.Float);

                return (start, end);
            }
            catch (FormatException fe)
            {
                throw new FormatException($"Cannot parse input '{input}' using {nameof(DoubleRangeParser)} ", fe);
            }
        }
    }
}
