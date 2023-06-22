using SFR.TemplateRandomizer.TypeGenerators.Abstractions;

namespace SFR.TemplateRandomizer.TypeGenerators;

internal class GuidGenerator : ITypeGenerator
{
    public GuidGenerator() { }

    public object Execute() => Guid.NewGuid();
}