using System.Globalization;
using System.Text.RegularExpressions;

namespace SFR.TemplateRandomizer.Parsers
{
    internal class DateRangeParser : IArgumentParser<(DateTimeOffset start, DateTimeOffset end)>
    {
        private readonly Regex rangeRegex = new(@"(?<start>\d{4}\/\d{1,2}\/\d{1,2})(\.{2})?(?<end>\d{4}\/\d{1,2}\/\d{1,2})", RegexOptions.Compiled);
        private readonly DateTimeOffset defaultMin;
        private readonly DateTimeOffset defaultMax;

        public DateRangeParser(DateTimeOffset? defaultMin = null, DateTimeOffset? defaultMax = null)
        {
            this.defaultMin = defaultMin ?? new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
            this.defaultMax = defaultMax ?? new DateTimeOffset(2100, 12, 31, 23, 59, 59, TimeSpan.Zero);
        }

        public (DateTimeOffset start, DateTimeOffset end) Parse(string input)
        {
            if (input is null)
                return (defaultMin, defaultMax);

            try
            {
                var match = rangeRegex.Match(input);

                var start = DateTimeOffset.Parse(match.Groups["start"].Value);
                var end = match.Groups["end"].Value == string.Empty ? this.defaultMax : DateTimeOffset.Parse(match.Groups["end"].Value);

                return (start, end);
            }
            catch (FormatException fe)
            {
                throw new FormatException($"Cannot parse input '{input}' using {nameof(DateRangeParser)} ", fe);
            }
        }
    }
}