using SFR.TemplateGenerator.Models;

namespace SFR.TemplateGenerator.Parsers;

public interface IArgumentParser<out T>
{
    T Parse(RangeSegment input);
}