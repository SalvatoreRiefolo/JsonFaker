using SFR.TemplateRandomizer.Parsers;

namespace SFR.TemplateRandomizer.TypeGenerator
{
    public class DoubleGenerator : TypeGenerator
    {
        private readonly double min;
        private readonly double max;

        private readonly IArgumentParser<(double, double)> argumentParser = new DoubleRangeParser();

        public DoubleGenerator(Random random, string args)
            : base(random)
        {
            (min, max) = this.argumentParser.Parse(args);
        }

        public override object Execute() => base.random.NextDouble() * (this.max - this.min) + this.min;
    }
}
