using System.Diagnostics;
using JsonFaker.Models.TokenTypes;
using JsonFaker.TypeGenerators.Constants;
using ValueType = JsonFaker.Models.TokenTypes.ValueType;

namespace JsonFaker.Models;

public record TypeSpecification(ValueType Type, string Value)
{
    public static TypeSpecification FromToken(string token)
        => token.Split(" ") switch
        {
            [Tokens.Integer, string r] => new TypeSpecification(ValueType.Integer, r),
            [Tokens.Double, string r] => new TypeSpecification(ValueType.Double, r),
            [Tokens.String, string r] => new TypeSpecification(ValueType.String, r),
            [Tokens.Date, string r] => new TypeSpecification(ValueType.Date, r),
            [Tokens.Guid] => new TypeSpecification(ValueType.GUID, string.Empty),
            [Tokens.Sequence] => new TypeSpecification(ValueType.Sequence, string.Empty),
            [['&', ..] s] => new TypeSpecification(ValueType.Reference, s),
            _ => throw new UnreachableException()
        };
};

public record PropertyTemplate(TypeSpecification Specification, IEnumerable<ModifierSegment>? Modifiers)
{
    public static PropertyTemplate FromJsonProperty(string jsonProperty)
    {
        char identifier = jsonProperty.Trim()[0];
        string[] tokens = jsonProperty.Split(new char[] { '$', '&', '-' },
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        TypeSpecification spec = identifier switch
        {
            Tokens.TokenIdentifier => TypeSpecification.FromToken(tokens[0]),
            Tokens.ReferenceIdentifier => new TypeSpecification(ValueType.Reference, tokens[0]),
            _ => throw new NotSupportedException($"'{identifier}' is not a valid token prefix.")
        };

        List<ModifierSegment> mods = ParseModifiersFromTokens(tokens.AsSpan(1));

        return new(spec, mods);
    }

    private static List<ModifierSegment> ParseModifiersFromTokens(Span<string> tokens)
    {
        var mods = new List<ModifierSegment>();

        foreach (var m in tokens)
        {
            string[] modTokens = m.Split(' ');

            var type = modTokens[0] switch
            {
                Tokens.Repeat => ModifierType.Repeat,
                _ => throw new NotSupportedException($"'{modTokens[0]}' is not a valid token modifier.")
            };

            var value = modTokens.Length > 1 ? modTokens[1] : string.Empty;
            mods.Add(new ModifierSegment(type, value));
        }

        return mods;
    }
}
