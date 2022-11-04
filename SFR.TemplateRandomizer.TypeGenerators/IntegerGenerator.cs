using SFR.TemplateGenerator.Parsers;
using SFR.TemplateRandomizer.TypeGenerators.Models;

namespace SFR.TemplateRandomizer.TypeGenerators
{
    internal class IntegerGenerator : TypeGenerator
    {
        private readonly int min;
        private readonly int max;

        private readonly IArgumentParser<(int, int)> argumentParser = new IntegerRangeParser();

        public IntegerGenerator(Random random, IGeneratorArgument args)
            : base(random)
        {
            (this.min, this.max) = this.argumentParser.Parse(args.Value);
        }

        public override object Execute() => base.Random.Next(this.min, this.max);
    }
}
