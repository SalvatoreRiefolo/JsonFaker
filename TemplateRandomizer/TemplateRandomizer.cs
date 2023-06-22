using Newtonsoft.Json.Linq;
using SFR.TemplateGenerator.Models;
using SFR.TemplateGenerator.Parsers;
using SFR.TemplateRandomizer.TypeGenerators;
using SFR.TemplateRandomizer.TypeGenerators.Abstractions;
using SFR.TemplateRandomizer.TypeGenerators.Constants;

namespace SFR.TemplateRandomizer;

public class TemplateRandomizer
{
    private readonly JObject template;

    private readonly Random random = new(12); //new(Environment.TickCount);
    private readonly IArgumentParser<(int, int)> rangeParser = new IntegerRangeParser();
    private readonly IDictionary<string, ITypeGenerator> generatorsCache = new Dictionary<string, ITypeGenerator>();
    private readonly ITypeGeneratorFactory typeGeneratorFactory;

    public TemplateRandomizer(JObject template)
    {
        this.template = template ?? throw new ArgumentNullException(nameof(template));
        typeGeneratorFactory = new TypeGeneratorFactory(random);
    }

    public JObject Randomize()
        => RandomizePropertyValues(RepeatPropertyValues(RepeatPropertyNames(template)));

    public JObject Randomize(int f)
    {
        var addRepeat = RepeatPropertyNames(template);
        // Console.WriteLine("Add Repeated");
        // Console.WriteLine(addRepeat.ToString());

        // Console.WriteLine();

        var repeat = RepeatPropertyValues(addRepeat);
        // Console.WriteLine("Repeat");
        // Console.WriteLine(repeat.ToString());

        // Console.WriteLine();

        return RandomizePropertyValues(repeat);
    }

    private JObject RepeatPropertyNames(JObject jobject)
    {
        var result = new JObject();

        foreach (var prop in jobject.Properties())
        {
            var current = new JProperty(prop.Name);

            // create + shuffle, pop
            // check if upper bound > repeat if property name is a token
            if (current.Name.Contains(Tokens.Repeat))
            {
                var (nameToken, repeatRange) = current.Name.SplitOnToken(Tokens.Repeat);
                var (low, high) = rangeParser.Parse(RangeSegment.CreateFromToken(repeatRange));

                for (var i = 0; i < random.Next(low, high); i++)
                {
                    var retryCount = 5;
                    var generatedValue = GenerateValue(nameToken);

                    while (!result.TryAdd(generatedValue.ToString(), prop.Value) && retryCount > 0)
                        retryCount--;

                    if (retryCount == 0)
                        result.Add($"{generatedValue}_{i}", prop.Value);
                }

                continue;
            }

            current.Value = prop.Value;

            if (prop.Value.Type == JTokenType.Object)
                current.Value = RepeatPropertyNames((JObject)prop.Value);

            result.Add(current);
        }

        return result;
    }

    private JObject RepeatPropertyValues(JObject jobject)
    {
        var result = new JObject();

        foreach (var property in jobject.Properties())
        {
            var current = new JProperty(property.Name)
            {
                Value = property.Value.Type switch
                {
                    JTokenType.Object => RepeatPropertyValues((JObject)property.Value),
                    JTokenType.Array => RepeatArrayValues(property),
                    _ => property.Value
                }
            };

            result.Add(current);
        }

        return result;
    }

    private JArray RepeatArrayValues(JProperty property)
    {
        var arr = new JArray();
        foreach (var item in (JArray)property.Value)
        {
            var itemValue = item.ToString();

            if (item.Type == JTokenType.Object)
            {
                arr.Add(RepeatPropertyValues((JObject)item));
            }
            else if (itemValue.Contains(Tokens.Repeat))
            {
                var tokens = itemValue.Split(Tokens.Repeat, StringSplitOptions.TrimEntries);
                var (low, high) = rangeParser.Parse(RangeSegment.CreateFromToken(tokens[1]));

                for (var i = 0; i < random.Next(low, high); i++)
                    arr.Add(tokens[0]);
            }
            else
            {
                arr.Add(item);
            }
        }

        return arr;
    }

    private JObject RandomizePropertyValues(JObject jobject, int sequenceCounter = 0)
    {
        var result = new JObject();

        foreach (var property in jobject.Properties())
        {
            result[property.Name] = property.Value.Type switch
            {
                JTokenType.Object => RandomizePropertyValues((JObject)property.Value),
                JTokenType.Array => RandomizeArrayValues((JArray)property.Value),
                _ => property.Value.ToString() == Tokens.Sequence ? sequenceCounter : GenerateValue(property.Value)
            };
        }

        return result;
    }

    private JArray RandomizeArrayValues(JArray currentArr)
    {
        var newArr = new JArray();

        for (var i = 0; i < currentArr.Count; i++)
        {
            var item = currentArr[i];
            newArr.Add(item.Type == JTokenType.Object
                ? RandomizePropertyValues((JObject)item, i)
                : GenerateValue(item, i));
        }

        return newArr;
    }

    private JToken GenerateValue(JToken token, int sequenceCounter = 0)
    {
        var tokenValue = token.ToString();

        if (tokenValue.StartsWith(Tokens.ReferenceSymbol))
        {
            var referencedSection = template.GetValue(tokenValue) as JObject;
            return RandomizePropertyValues(RepeatPropertyValues(referencedSection!), sequenceCounter);
        }

        if (!tokenValue.StartsWith(Tokens.TokenSymbol)) return token;

        var generator = generatorsCache.GetOrCreate(tokenValue,
            () => typeGeneratorFactory.CreateTypeGenerator(tokenValue));

        return new JValue(generator.Execute());
    }


}