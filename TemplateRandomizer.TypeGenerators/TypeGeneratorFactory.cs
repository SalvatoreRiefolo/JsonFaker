using TemplateGenerator.Models;
using TemplateRandomizer.TypeGenerators.Abstractions;
using TemplateRandomizer.TypeGenerators.Constants;

namespace TemplateRandomizer.TypeGenerators;

public class TypeGeneratorFactory : ITypeGeneratorFactory
{
    private readonly Random random;
    private readonly char argumentSeparator;

    public TypeGeneratorFactory(Random random, char argumentSeparator = ' ')
    {
        this.random = random;
        this.argumentSeparator = argumentSeparator;
    }

    public ITypeGenerator CreateTypeGenerator(string keywordWithArguments)
    {
        var tokens = keywordWithArguments.Split(argumentSeparator);

        try
        {
            System.Console.WriteLine($"Range: {string.Join(',', tokens)}");
            var range = RangeSegment.CreateFromToken(tokens.Length > 1 ? tokens[1] : string.Empty);
            return tokens[0] switch
            {
                Tokens.Integer => new IntegerGenerator(random, range),
                Tokens.String => new StringGenerator(random, range),
                Tokens.Double => new DoubleGenerator(random, range),
                Tokens.Date => new DateGenerator(random, range),
                Tokens.Guid => new GuidGenerator(),
                _ => throw new NotImplementedException($"Type generator for type '{tokens[0]}' not implemented")
            };
        }
        catch (Exception e)
        {
            System.Console.WriteLine($"Range: {tokens}, Message: {e}");
            throw e;
        }

    }
}