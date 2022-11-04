namespace SFR.TemplateGenerator.Parsers;

public static class ParserUtilities
{
    public static (string first, string second) SplitBoundaryArguments(string arguments, string delimiter)
    {
        if (!arguments.Contains(delimiter))
            return (arguments, arguments);

        var tokenized = arguments.Split(delimiter);

        if (tokenized.Length != 2)
            throw new ArgumentException("Arguments cannot be parsed as boundaries", nameof(arguments));

        return (tokenized[0], tokenized[1]);
    }

    public static T ParseArgument<T>(string arg, Func<string, T> parse, T defaultValue = default)
         => arg == string.Empty ? defaultValue : parse(arg);
}