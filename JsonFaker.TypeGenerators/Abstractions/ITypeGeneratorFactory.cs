namespace JsonFaker.TypeGenerators.Abstractions;

public interface ITypeGeneratorFactory
{
    ITypeGenerator CreateTypeGenerator(string keywordWithArguments);
}