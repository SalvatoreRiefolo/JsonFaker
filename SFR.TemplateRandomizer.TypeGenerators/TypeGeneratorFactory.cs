using SFR.TemplateRandomizer.TypeGenerators.Abstractions;
using SFR.TemplateRandomizer.TypeGenerators.Constants;
using SFR.TemplateRandomizer.TypeGenerators.Models;

namespace SFR.TemplateRandomizer.TypeGenerators;

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
        var tokens = keywordWithArguments.Split(this.argumentSeparator);

        var arguments = ParseArguments(tokens);

        return tokens[0] switch
        {
            Tokens.Integer => new IntegerGenerator(random, arguments),
            Tokens.String => new StringGenerator(random, arguments),
            Tokens.Double => new DoubleGenerator(random, arguments),
            Tokens.Date => new DateGenerator(random, arguments),
            _ => throw new NotImplementedException($"Type generator for type '{tokens[0]}' not implemented")
        };
    }

    private IGeneratorArgument ParseArguments(string[] tokens)
        => tokens.Any() ? new GeneratorArguments(tokens[0], tokens[1]) : new NullArguments();
}