namespace SFR.TemplateGenerator.Parsers;

public abstract class BoundaryParser<T> : IArgumentParser<(T, T)>
{
    private readonly T defaultMin;
    private readonly T defaultMax;
    private readonly Func<string, T> parser;

    protected BoundaryParser(T defaultMin, T defaultMax, Func<string, T> parser)
    {
        this.defaultMin = defaultMin;
        this.defaultMax = defaultMax;
        this.parser = parser;
    }

    public (T, T) Parse(string input)
    {
        return ParseInternal(input);
    }

    private (T, T) ParseInternal(string input)
    {
        if (input is null)
            return (defaultMin, defaultMax);

        try
        {
            var (first, second) = ParserUtilities.SplitArguments(input, "..");
                
            var start = ParserUtilities.ParseArgument(first, parser, defaultMin);
            var end = ParserUtilities.ParseArgument(second, parser, defaultMax);

            return (start, end);
        }
        catch (FormatException fe)
        {
            throw new FormatException($"Cannot parse input '{input}' using {nameof(DateRangeParser)} ", fe);
        }
    }
}