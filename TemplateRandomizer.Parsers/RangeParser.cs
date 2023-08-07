using TemplateGenerator.Models;

namespace TemplateGenerator.Parsers;

public abstract class RangeParser<T> : IArgumentParser<(T, T)>
    where T : IComparable
{
    private readonly T defaultMin;
    private readonly T defaultMax;
    private readonly Func<string, T> parser;
    private const string Delimiter = "..";

    protected RangeParser(T defaultMin, T defaultMax, Func<string, T> parser)
    {
        if (defaultMin.CompareTo(defaultMax) > 0)
            throw new InvalidOperationException(
                $"Cannot define range with 'min' {defaultMin} greater than 'max' {defaultMax}");

        this.defaultMin = defaultMin;
        this.defaultMax = defaultMax;
        this.parser = parser;
    }

    public (T, T) Parse(RangeSegment segment)
    {
        try
        {
            return
            (
                segment.LowerBound is null ? defaultMin : parser(segment.LowerBound),
                segment.UpperBound is null ? defaultMax : parser(segment.UpperBound)
            );
        }
        catch (FormatException fe)
        {
            throw new FormatException(
                $"Cannot parse input '{segment}' using {GetType().Name} ", fe);
        }
    }
}