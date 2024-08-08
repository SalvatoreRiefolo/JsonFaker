using JsonFaker.Models;
using JsonFaker.TypeGenerators.Abstractions;
using JsonFaker.TypeGenerators.Constants;

namespace JsonFaker.TypeGenerators;

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
            var range = RangeSegment.CreateFromToken(tokens.Length > 1 ? tokens[1] : string.Empty);
            
            Console.WriteLine($"Range for {tokens[0]}: {range}");

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
            Console.WriteLine($"Range: {tokens}, Message: {e}");
            throw;
        }
    }
}