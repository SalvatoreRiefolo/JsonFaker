using JsonFaker.TypeGenerators.Abstractions;

namespace JsonFaker.TypeGenerators;

internal class GuidGenerator : ITypeGenerator
{
    public GuidGenerator() { }

    public object Execute() => Guid.NewGuid();
}