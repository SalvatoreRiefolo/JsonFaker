using Drafts.Parsers;

namespace Drafts.Directives
{
    public class IntegerDirective : Directive
    {
        private readonly int min;
        private readonly int max;

        private readonly IArgumentParser<(int, int)> argumentParser;

        public IntegerDirective(Random random, string args)
            : base(random)
        {
            min = int.MinValue;
            max = int.MaxValue;
        }

        public override object Execute() => random.Next(min, max);
    }
}
