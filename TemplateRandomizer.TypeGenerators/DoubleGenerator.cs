using TemplateGenerator.Models;
using TemplateGenerator.Parsers;

namespace TemplateRandomizer.TypeGenerators;

internal class DoubleGenerator : TypeRandomGenerator
{
    private readonly double min;
    private readonly double max;

    private readonly IArgumentParser<(double, double)> argumentParser = new DoubleRangeParser();

    public DoubleGenerator(Random random, RangeSegment range)
        : base(random)
    {
        System.Console.WriteLine($"Double range {range}");
        (min, max) = argumentParser.Parse(range);
        System.Console.WriteLine($"Double min {min} max {max}");
    }

    public override object Execute()
    {
        return Random.NextDouble() * (max - min) + min;
    }
}