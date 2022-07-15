namespace SFR.TemplateRandomizer.Parsers
{
    internal interface IArgumentParser<out T>
    {
        T Parse(string input);
    }
}
