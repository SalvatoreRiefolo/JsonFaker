namespace SFR.TemplateRandomizer.TypeGenerators.Models;

public struct NullArguments : IGeneratorArgument
{
    public string Name => throw new InvalidOperationException("No argument was provided");
    public string Value => throw new InvalidOperationException("No argument was provided");
}