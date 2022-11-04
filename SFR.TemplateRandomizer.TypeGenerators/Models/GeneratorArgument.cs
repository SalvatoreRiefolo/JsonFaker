namespace SFR.TemplateRandomizer.TypeGenerators.Models;

public struct GeneratorArguments : IGeneratorArgument
{
    public GeneratorArguments(string name, string value)
    {
        Name = name;
        Value = value;
    }

    public string Name { get; }
    public string Value { get; }
}