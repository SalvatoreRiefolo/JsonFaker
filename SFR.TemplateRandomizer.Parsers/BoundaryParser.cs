namespace SFR.TemplateGenerator.Parsers;

public abstract class BoundaryParser<T> : IArgumentParser<(T, T)>
    where T : IComparable
{
    private readonly T defaultMin;
    private readonly T defaultMax;
    private readonly Func<string, T> parser;
    private readonly string delimiter = "..";

    protected BoundaryParser(T defaultMin, T defaultMax, Func<string, T> parser)
    {
        if (defaultMin.CompareTo(defaultMax) > 0)
            throw new InvalidOperationException($"Cannot define range with 'min' {defaultMin} greater than 'max' {defaultMax}");
        
        this.defaultMin = defaultMin;
        this.defaultMax = defaultMax;
        this.parser = parser;
    }

    public (T, T) Parse(string input)
    {
        if (input is null)
            return (defaultMin, defaultMax);

        try
        {
            var (first, second) = SplitBoundaryArguments(input, delimiter);

            var start = ParserUtilities.ParseArgument(first, parser, defaultMin);
            var end = ParserUtilities.ParseArgument(second, parser, defaultMax);

            return (start, end);
        }
        catch (FormatException fe)
        {
            throw new FormatException($"Cannot parse input '{input}' using {nameof(DateRangeParser)} ", fe);
        }
    }
    
    private static (string first, string second) SplitBoundaryArguments(string arguments, string delimiter)
    {
        if (!arguments.Contains(delimiter))
            return (arguments, arguments);

        var tokens = arguments.Split(delimiter);

        if (tokens.Length != 2)
            throw new ArgumentException("Arguments cannot be parsed as boundaries", nameof(arguments));

        return (tokens[0], tokens[1]);
    }
}