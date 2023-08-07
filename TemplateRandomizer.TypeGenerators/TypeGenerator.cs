using TemplateRandomizer.TypeGenerators.Abstractions;

namespace TemplateRandomizer.TypeGenerators;

internal abstract class TypeRandomGenerator : ITypeGenerator
{
    protected readonly Random Random;

    protected TypeRandomGenerator(Random random) => Random = random;

    public abstract object Execute();
}