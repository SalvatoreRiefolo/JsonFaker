using JsonFaker.TypeGenerators.Abstractions;

namespace JsonFaker.TypeGenerators;

internal abstract class TypeGenerator : ITypeGenerator
{
    protected readonly Random Random;

    protected TypeGenerator(Random random) => Random = random;

    public abstract object Execute();
}