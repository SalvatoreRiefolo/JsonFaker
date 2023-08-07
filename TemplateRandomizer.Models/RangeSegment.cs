using System.Diagnostics;

namespace TemplateGenerator.Models;

public record RangeSegment(string? LowerBound, string? UpperBound)
{
    private const string Delimiter = "..";

    public static RangeSegment CreateFromToken(string rangeToken)
    {
        var boundaryTokens = rangeToken.Split(Delimiter);

        return boundaryTokens switch
        {
            [""] => new RangeSegment(null, null),
            [string b] => new RangeSegment(b, b),
            [string lb, ""] => new RangeSegment(lb, null),
            ["", string ub] => new RangeSegment(null, ub),
            [string lb, string ub] => new RangeSegment(lb, ub),
            _ => throw new UnreachableException()
        };
    }
}
