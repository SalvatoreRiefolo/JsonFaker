namespace SFR.TemplateRandomizer;

public class Configuration
{
    public IntegerConfiguration? Integer { get; set; }
    public DoubleConfiguration? Double { get; set; }
    public StringConfiguration? String { get; set; }
    public DateConfiguration? Date { get; set; }
}

public abstract class RangeConfiguration<T>
{
    public T? Min { get; set; }
    public T? Max { get; set; }
}

public class IntegerConfiguration : RangeConfiguration<int>
{ }

public class DoubleConfiguration : RangeConfiguration<double>
{
    public Range MaxDecimalDigits { get; set; }
}

public class StringConfiguration
{
    public StringCase Casing { get; set; }
    public bool IncludeDigits { get; set; }
    public string? Prefix { get; set; }
    public string? Suffix { get; set; }
}

public enum StringCase
{
    Uppercase,
    Lowercase,
    Mixed
}

public class DateConfiguration : RangeConfiguration<DateTimeOffset>
{
    public string? Format { get; set; }
}