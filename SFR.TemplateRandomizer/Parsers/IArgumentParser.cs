using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drafts.Parsers
{
    internal interface IArgumentParser<out T>
    {
        T Parse(string input);
    }
}
