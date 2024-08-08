using JsonFaker.Models;
using JsonFaker.Parsers;

namespace JsonFaker.TypeGenerators;

internal class DateGenerator : TypeGenerator
{
    private readonly DateTimeOffset min;
    private readonly DateTimeOffset max;

    private readonly IArgumentParser<(DateTimeOffset, DateTimeOffset)> argumentParser = new DateRangeParser();
    private readonly string formatting;

    public DateGenerator(Random random, RangeSegment range, string formatting = "yyyy-MM-ddThh:mm:ssZ")
        : base(random)
    {
        (min, max) = argumentParser.Parse(range);
        this.formatting = formatting;
    }

    public override object Execute()
    {
        var range = max - min;
        var randTimeSpan = new TimeSpan((long)(Random.NextDouble() * range.Ticks));

        return (min + randTimeSpan).ToString(formatting);
    }
}