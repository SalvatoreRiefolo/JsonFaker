using JsonFaker.TypeGenerators.Abstractions;

namespace JsonFaker.TypeGenerators;

internal abstract class TypeRandomGenerator : ITypeGenerator
{
    protected readonly Random Random;

    protected TypeRandomGenerator(Random random) => Random = random;

    public abstract object Execute();
}