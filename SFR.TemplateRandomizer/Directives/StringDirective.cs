using Drafts.Parsers;

namespace Drafts.Directives
{
    internal class StringDirective : Directive
    {
        private readonly int min;
        private readonly int max;
        private readonly string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        private readonly IArgumentParser<(int, int)> argumentParser;

        public StringDirective(Random random, string args)
            : base(random)
        {
            this.min = int.MinValue;
            this.max = int.MaxValue;
        }

        public override object Execute()
        {
            var count = base.random.Next(min, max);
            var res = new char[count];

            for (int i = 0; i < count; i++)
            {
                res[i] = this.allowedChars[base.random.Next(0, this.allowedChars.Length)];
            }

            return new string(res);
        }
    }
}
