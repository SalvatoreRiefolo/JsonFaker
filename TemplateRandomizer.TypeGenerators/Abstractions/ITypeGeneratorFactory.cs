namespace SFR.TemplateRandomizer.TypeGenerators.Abstractions;

public interface ITypeGeneratorFactory
{
    ITypeGenerator CreateTypeGenerator(string keywordWithArguments);
}