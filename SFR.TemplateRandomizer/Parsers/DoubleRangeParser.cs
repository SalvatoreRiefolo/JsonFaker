using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drafts.Parsers
{
    internal class DoubleRangeParser : IArgumentParser<(double start, double end)>
    {
        public (double start, double end) Parse(string input)
        {
            throw new NotImplementedException();
        }
    }
}
