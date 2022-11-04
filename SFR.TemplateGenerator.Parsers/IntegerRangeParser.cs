using System.Text.RegularExpressions;

namespace SFR.TemplateGenerator.Parsers
{
    public class IntegerRangeParser : BoundaryParser<int>
    {
        public IntegerRangeParser(int defaultMin = int.MinValue, int defaultMax = int.MaxValue) 
            : base(defaultMin, defaultMax, int.Parse)
        { }

    }
}
