namespace SFR.TemplateRandomizer;

public class RandomizerSettings
{
    public IntegerSettings Integer { get; set; }
    public DoubleSettings Double { get; set; }
    public StringSettings String { get; set; }
    public DateSettings Date { get; set; }
}

public class RangeSettings<T>
{
    public T Min { get; set; }
    public T Max { get; set; }
}

public class IntegerSettings : RangeSettings<int>
{
    
}

public class DoubleSettings : RangeSettings<double>
{
    public Range MaxDecimalDigits { get; set; }

}

public class StringSettings
{
    public StringCase Casing { get; set; }
    public bool IncludeDigits { get; set; }
}

public enum StringCase
{
    Uppercase,
    Lowercase
}

public class DateSettings : RangeSettings<DateTimeOffset>
{
    public string Format { get; set; }
}