namespace SFR.TemplateGenerator.Parsers;

public static class ParserUtilities
{
    public static T ParseArgument<T>(string arg, Func<string, T> parse, T defaultValue = default)
         => arg == string.Empty ? defaultValue : parse(arg);
}