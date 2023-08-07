namespace TemplateGenerator.Parsers;

public class DateRangeParser : RangeParser<DateTimeOffset>
{
    public DateRangeParser(DateTimeOffset? defaultMin = null, DateTimeOffset? defaultMax = null)
        : base(
            defaultMin ?? DateTimeOffset.UnixEpoch,
            defaultMax ?? new DateTimeOffset(9999, 12, 31, 23, 59, 59, TimeSpan.Zero),
            DateTimeOffset.Parse)
    { }
}