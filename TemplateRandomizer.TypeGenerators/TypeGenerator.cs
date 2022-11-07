using SFR.TemplateRandomizer.TypeGenerators.Abstractions;

namespace SFR.TemplateRandomizer.TypeGenerators
{
    internal abstract class TypeGenerator : ITypeGenerator
    {
        protected readonly Random Random;

        protected TypeGenerator(Random random) => this.Random = random;

        public abstract object Execute();
    }
}
