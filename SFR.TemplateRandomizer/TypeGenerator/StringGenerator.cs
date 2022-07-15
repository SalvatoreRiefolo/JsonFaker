using SFR.TemplateRandomizer.Parsers;

namespace SFR.TemplateRandomizer.TypeGenerator
{
    internal class StringGenerator : TypeGenerator
    {
        private readonly int min;
        private readonly int max;
        private readonly string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        private readonly IArgumentParser<(int, int)> argumentParser = new IntegerRangeParser(0, 255);

        public StringGenerator(Random random, string args)
            : base(random)
        {
            (min, max) = argumentParser.Parse(args);
        }

        public override object Execute()
        {
            var count = random.Next(min, max);
            var res = new char[count];

            for (int i = 0; i < count; i++)
            {
                res[i] = allowedChars[random.Next(0, allowedChars.Length)];
            }

            return new string(res);
        }
    }
}
