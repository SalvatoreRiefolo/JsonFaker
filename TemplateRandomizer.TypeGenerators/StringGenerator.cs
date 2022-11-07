using SFR.TemplateGenerator.Parsers;
using SFR.TemplateRandomizer.TypeGenerators.Models;

namespace SFR.TemplateRandomizer.TypeGenerators
{
    internal class StringGenerator : TypeGenerator
    {
        private readonly int min;
        private readonly int max;
        private readonly string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        private readonly IArgumentParser<(int, int)> argumentParser = new IntegerRangeParser(0, 255);

        public StringGenerator(Random random, IGeneratorArgument args)
            : base(random)
        {
            (this.min, this.max) = this.argumentParser.Parse(args.Value);
        }

        public override object Execute()
        {
            var count = base.Random.Next(this.min, this.max);
            var res = new char[count];

            for (int i = 0; i < count; i++)
            {
                res[i] = this.allowedChars[base.Random.Next(0, this.allowedChars.Length)];
            }

            return new string(res);
        }
    }
}
