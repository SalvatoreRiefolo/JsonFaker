using System.Text.RegularExpressions;

namespace SFR.TemplateRandomizer.Parsers
{
    internal class IntegerRangeParser : IArgumentParser<(int start, int end)>
    {
        private readonly Regex rangeRegex = new(@"(?<start>\d+)(\.{2})?(?<end>\d*)", RegexOptions.Compiled);
        private readonly int defaultMin;
        private readonly int defaultMax;

        public IntegerRangeParser(int defaultMin = int.MinValue, int defaultMax = int.MaxValue)
        {
            this.defaultMin = defaultMin;
            this.defaultMax = defaultMax;
        }

        public (int start, int end) Parse(string input)
        {
            if (input is null)
                return (defaultMin, defaultMax);

            try
            {
                var match = rangeRegex.Match(input);

                var start = int.Parse(match.Groups["start"].Value);
                var end = match.Groups["end"].Value == string.Empty ? start : int.Parse(match.Groups["end"].Value) + 1;

                return (start, end);
            } catch (FormatException fe)
            {
                throw new FormatException($"Cannot parse input '{input}' using {nameof(IntegerRangeParser)} ", fe);
            }
        }
    }
}
