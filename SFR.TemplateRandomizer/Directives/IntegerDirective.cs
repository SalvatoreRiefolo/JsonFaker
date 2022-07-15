using Drafts.Parsers;

namespace Drafts.Directives
{
    public class IntegerDirective : Directive
    {
        private readonly int min;
        private readonly int max;

        private readonly IArgumentParser<(int, int)> argumentParser = new IntegerRangeParser();

        public IntegerDirective(Random random, string args)
            : base(random)
        {
            (this.min, this.max) = this.argumentParser.Parse(args);
        }

        public override object Execute() => random.Next(this.min, this.max);
    }
}
