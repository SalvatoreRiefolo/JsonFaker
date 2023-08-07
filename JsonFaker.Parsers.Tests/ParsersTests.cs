using JsonFaker.Parsers;
using JsonFaker.Models;

namespace JsonFaker.Parsers.Tests;

public class ParsersTests
{
    private readonly Random rnd = new Random();

    [Theory]
    [InlineData("", int.MinValue, int.MaxValue)]
    [InlineData("..50", int.MinValue, 50)]
    [InlineData("50..", 50, int.MaxValue)]
    [InlineData("10..100", 10, 100)]
    public void IntegerRangeParser_ShouldParseRanges_InEveryAllowedFormat
        (string rangeToken, int expectedMin, int expectedMax)
    {
        var parser = new IntegerRangeParser();
        var (min, max) = parser.Parse(RangeSegment.CreateFromToken(rangeToken));

        min.Should().Be(expectedMin);
        max.Should().Be(expectedMax);
    }
}