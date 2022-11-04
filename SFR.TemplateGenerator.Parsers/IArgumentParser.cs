namespace SFR.TemplateGenerator.Parsers
{
    public interface IArgumentParser<out T>
    {
        T Parse(string input);
    }
}
