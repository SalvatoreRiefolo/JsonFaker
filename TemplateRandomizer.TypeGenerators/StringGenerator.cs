using SFR.TemplateGenerator.Models;
using SFR.TemplateGenerator.Parsers;

namespace SFR.TemplateRandomizer.TypeGenerators;

internal class StringGenerator : TypeRandomGenerator
{
    private readonly int min;
    private readonly int max;
    private const string AllowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

    private readonly IArgumentParser<(int, int)> argumentParser = new IntegerRangeParser(0, 255);

    public StringGenerator(Random random, RangeSegment range)
        : base(random)
    {
        (min, max) = argumentParser.Parse(range);
    }

    public override object Execute()
    {
        var count = Random.Next(min, max);
        var res = new char[count];

        for (var i = 0; i < count; i++) res[i] = AllowedChars[Random.Next(0, AllowedChars.Length)];

        return new string(res);
    }
}

internal class StringGeneratorOptions
{
    private const string LowerCaseChars = "abcdefghijklmnopqrstuvwxyz";
    private const string UpperCaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string Digits = "0123456789";

    public StringGeneratorOptions(
        int minLength,
        int maxLength,
        string alphabet,
        string prefix,
        string suffix,
        StringCase casing)
    {
        MinLength = minLength;
        MaxLength = maxLength;
        Alphabet = alphabet;
        Prefix = prefix;
        Suffix = suffix;
        Casing = casing;
    }

    public StringGeneratorOptions(
        int minLength,
        int maxLength,
        bool includeDigits,
        string prefix,
        string suffix,
        StringCase casing)
    {
        MinLength = minLength;
        MaxLength = maxLength;
        IncludeDigits = includeDigits;
        Casing = casing;

        Alphabet = casing switch
        {
            StringCase.Uppercase => includeDigits ? UpperCaseChars + Digits : UpperCaseChars,
            StringCase.Lowercase => includeDigits ? LowerCaseChars + Digits : LowerCaseChars,
            StringCase.Mixed => includeDigits
                ? UpperCaseChars + LowerCaseChars + Digits
                : UpperCaseChars + LowerCaseChars,
            _ => throw new NotSupportedException()
        };

        Prefix = prefix ?? "";
        Suffix = suffix ?? "";
    }


    public int MinLength { get; }
    public int MaxLength { get; }
    public string Alphabet { get; }
    public bool IncludeDigits { get; }
    public string Prefix { get; }
    public string Suffix { get; }
    public StringCase Casing { get; }

    public enum StringCase
    {
        Uppercase,
        Lowercase,
        Mixed
    }
}
