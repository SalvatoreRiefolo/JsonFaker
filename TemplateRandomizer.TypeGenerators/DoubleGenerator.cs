using SFR.TemplateGenerator.Parsers;
using SFR.TemplateRandomizer.TypeGenerators.Models;

namespace SFR.TemplateRandomizer.TypeGenerators
{
    internal class DoubleGenerator : TypeGenerator
    {
        private readonly double min;
        private readonly double max;

        private readonly IArgumentParser<(double, double)> argumentParser = new DoubleRangeParser();

        public DoubleGenerator(Random random, IGeneratorArgument args)
            : base(random)
        {
            (min, max) = this.argumentParser.Parse(args.Value);
        }

        public override object Execute() => base.Random.NextDouble() * (this.max - this.min) + this.min;
    }
}
