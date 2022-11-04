using System.Globalization;
using System.Text.RegularExpressions;

namespace SFR.TemplateGenerator.Parsers
{
    public class DoubleRangeParser : BoundaryParser<double>
    {
        public DoubleRangeParser(double defaultMin = double.MinValue, double defaultMax = double.MaxValue) 
            : base(defaultMin, defaultMax, double.Parse)
        { }

        // public (double start, double end) Parse(string input)
        // {
        //     if (input is null)
        //         return (defaultMin, defaultMax);
        //
        //     try
        //     {
        //         var match = rangeRegex.Match(input);
        //
        //         var start = double.Parse(match.Groups["start"].Value, NumberStyles.Float);
        //         var end = match.Groups["end"].Value == string.Empty ? this.defaultMax : double.Parse(match.Groups["end"].Value, NumberStyles.Float);
        //
        //         return (start, end);
        //     }
        //     catch (FormatException fe)
        //     {
        //         throw new FormatException($"Cannot parse input '{input}' using {nameof(DoubleRangeParser)} ", fe);
        //     }
        // }
    }
}
