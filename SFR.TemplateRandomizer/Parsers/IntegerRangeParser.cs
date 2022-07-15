using System.Text.RegularExpressions;

namespace Drafts.Parsers
{
    internal class IntegerRangeParser : IArgumentParser<(int start, int end)>
    {
        private readonly Regex rangeRegex = new(@"(?<start>\d+)(.{2})?(?<end>\d*)", RegexOptions.Compiled);
        private readonly int defaultMin;
        private readonly int defaultMax;

        public IntegerRangeParser()
        {
            this.defaultMin = int.MinValue;
            this.defaultMax = int.MaxValue;
        }

        public IntegerRangeParser((int defaultMin, int defaultMax) defaults)
        {
            (this.defaultMin, this.defaultMax) = defaults;
        }

        public (int start, int end) Parse(string input)
        {
            if (input is null)
                return (this.defaultMin, this.defaultMax);

            var match = this.rangeRegex.Match(input);

            var start = int.Parse(match.Groups["start"].Value);
            var end = match.Groups["end"].Value == string.Empty ? start : int.Parse(match.Groups["end"].Value) + 1;

            return (start, end);
        }
    }
}
