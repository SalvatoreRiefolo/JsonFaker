using TemplateGenerator.Models;

namespace TemplateGenerator.Parsers;

public interface IArgumentParser<out T>
{
    T Parse(RangeSegment input);
}