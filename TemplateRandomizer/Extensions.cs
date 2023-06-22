namespace SFR.TemplateRandomizer;

internal static class Extensions
{
    public static T GetOrCreate<T>(this IDictionary<string, T> cache, string key, Func<T> generator)
    {
        if (cache.TryGetValue(key, out var value))
            return value;

        var result = generator();
        cache[key] = result;

        return result;
    }

    public static string[] Tokenize(this string input, char separator)
        => input.Split(separator, StringSplitOptions.TrimEntries);

    public static (string before, string after) SplitOnToken(this string input, string separator)
    {
        var res = input.Split(separator, StringSplitOptions.TrimEntries);
        if (res.Length != 2)
            throw new IndexOutOfRangeException(
                $"Cannot split string '{input}' in only two parts with separator '{separator}'");

        return (res[0], res[1]);
    }

}