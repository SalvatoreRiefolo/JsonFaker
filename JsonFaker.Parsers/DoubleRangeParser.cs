using System.Globalization;

namespace JsonFaker.Parsers;

public class DoubleRangeParser : RangeParser<double>
{
    public DoubleRangeParser(double defaultMin = double.MinValue, double defaultMax = double.MaxValue)
        : base(defaultMin, defaultMax, (item) => double.Parse(item, NumberStyles.Float))
    { }
}