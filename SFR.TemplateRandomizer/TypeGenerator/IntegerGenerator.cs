using SFR.TemplateRandomizer.Parsers;

namespace SFR.TemplateRandomizer.TypeGenerator
{
    public class IntegerGenerator : TypeGenerator
    {
        private readonly int min;
        private readonly int max;

        private readonly IArgumentParser<(int, int)> argumentParser = new IntegerRangeParser();

        public IntegerGenerator(Random random, string args)
            : base(random)
        {
            (min, max) = argumentParser.Parse(args);
        }

        public override object Execute() => random.Next(min, max);
    }
}
