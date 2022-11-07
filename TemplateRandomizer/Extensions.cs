namespace SFR.TemplateRandomizer;

public static class Extensions
{
    public static T GetOrCreate<T>(this IDictionary<string, T> cache, string key, Func<T> generator)
    {
        if (cache.TryGetValue(key, out var value))
            return value;

        var result = generator();
        cache[key] = result;

        return result;
    }
}