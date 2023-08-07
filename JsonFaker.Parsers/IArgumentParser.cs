using JsonFaker.Models;

namespace JsonFaker.Parsers;

public interface IArgumentParser<out T>
{
    T Parse(RangeSegment input);
}