using TemplateRandomizer.TypeGenerators.Abstractions;

namespace TemplateRandomizer.TypeGenerators;

internal class GuidGenerator : ITypeGenerator
{
    public GuidGenerator() { }

    public object Execute() => Guid.NewGuid();
}