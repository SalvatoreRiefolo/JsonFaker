using JsonFaker.Models;
using JsonFaker.Parsers;

namespace JsonFaker.TypeGenerators;

internal class DoubleGenerator : TypeGenerator
{
    private readonly double min;
    private readonly double max;

    private readonly IArgumentParser<(double, double)> argumentParser = new DoubleRangeParser();

    public DoubleGenerator(Random random, RangeSegment range)
        : base(random)
    {
        (min, max) = argumentParser.Parse(range);
    }

    public override object Execute()
    {
        return Random.NextDouble() * (max - min) + min;
    }
}