using System.Text.RegularExpressions;

namespace SFR.TemplateGenerator.Parsers
{
    public class DateRangeParser : BoundaryParser<DateTimeOffset>
    {
        public DateRangeParser(DateTimeOffset? defaultMin = null, DateTimeOffset? defaultMax = null)
            : base(
            defaultMin ?? new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero),
            defaultMax ?? new DateTimeOffset(9999, 12, 31, 23, 59, 59, TimeSpan.Zero),
            DateTimeOffset.Parse)
        { }
    }
}