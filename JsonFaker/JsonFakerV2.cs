using Newtonsoft.Json.Linq;
using JsonFaker.Models;
using JsonFaker.Parsers;
using JsonFaker.TypeGenerators;
using JsonFaker.TypeGenerators.Abstractions;
using JsonFaker.TypeGenerators.Constants;
using System.Numerics;


namespace JsonFaker;


public class JsonFakerV2
{
    private readonly JObject template;
    
    private readonly Random random = new(Environment.TickCount);
    private readonly IArgumentParser<(int, int)> rangeParser = new IntegerRangeParser();
    private readonly IDictionary<string, ITypeGenerator> generatorCache = new Dictionary<string, ITypeGenerator>();
    private readonly ITypeGeneratorFactory typeGeneratorFactory;


    private JsonFakerV2(JObject template)
    {
        this.template = template;
        typeGeneratorFactory = new TypeGeneratorFactory(random);
    }

    public static JsonFakerV2 CreateFor(JObject template)
        => new(template ?? throw new ArgumentNullException(nameof(template)));

    public static JsonFakerV2 CreateFor(string template)
        => CreateFor(JObject.Parse(template));

    public JObject Randomize()
        => RandomizePropertyValues(RepeatPropertyValues(RepeatPropertyNames(template)));

    private JObject RepeatPropertyNames(JObject input)
    {
        var result = new JObject();

        foreach (var prop in input.Properties())
        {
            if (prop.Name.Contains(Tokens.Repeat))
            {
                var (typeToken, repeatRange) = prop.Name.SplitOnToken(Tokens.Repeat);
                var range = RangeSegment.CreateFromToken(repeatRange);
                var (low, high) = rangeParser.Parse(range);

                for (var i = 0; i < random.Next(low, high); i++)
                {
                    var retryCount = 5;
                    var generatedValue = GenerateValue(typeToken);

                    while (!result.TryAdd(generatedValue.ToString(), prop.Value) && retryCount > 0)
                        retryCount--;

                    if (retryCount == 0)
                        result.Add($"{generatedValue}_{i}", prop.Value);
                }

                continue;
            }

            var current = new JProperty(prop.Name);
            current.Value = prop.Value;

            if (prop.Value.Type == JTokenType.Object)
                current.Value = RepeatPropertyNames((JObject)prop.Value);

            result.Add(current);
        }

        return result;
    }

    private JObject RepeatPropertyValues(JObject input)
    {
        //var repeated = input.Properties()
        //    .Select(p =>
        //    {
        //        return new JProperty(p.Name)
        //        {
        //            Value = p.Value.Type switch
        //            {
        //                JTokenType.Object => RepeatPropertyValues((JObject)p.Value),
        //                JTokenType.Array => RepeatArrayValues((JArray)p.Value),
        //                _ => p.Value
        //            }
        //        };
        //    });

        //return new JObject(repeated);

        var result = new JObject();

        foreach (var property in input.Properties())
        {
            var current = new JProperty(property.Name)
            {
                Value = property.Value.Type switch
                {
                    JTokenType.Object => RepeatPropertyValues((JObject)property.Value),
                    JTokenType.Array => RepeatArrayValues((JArray)property.Value),
                    _ => property.Value
                }
            };

            result.Add(current);
        }

        return result;
    }

    private JArray RepeatArrayValues(JArray array)
    {
        var arr = new JArray();
        foreach (var item in array)
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

    private JObject RandomizePropertyValues(JObject input, int sequenceCounter = 0)
    {
        var result = new JObject();

        foreach (var property in input.Properties())
        {
            var current = new JProperty(property.Name)
            {
                Value = property.Value.Type switch
                {
                    JTokenType.Object => RandomizePropertyValues((JObject)property.Value),
                    JTokenType.Array => RandomizeArrayValues((JArray)property.Value),
                    _ => property.Value.ToString() == Tokens.Sequence ? sequenceCounter : GenerateValue(property.Value)
                }
            };

            result.Add(current);
        }

        return result;
    }

    private JArray RandomizeArrayValues(JArray array)
    {
        var newArr = new JArray();

        for (var i = 0; i < array.Count; i++)
        {
            var item = array[i];
            newArr.Add(item.Type == JTokenType.Object
                ? RandomizePropertyValues((JObject)item, i)
                : GenerateValue(item, i));
        }

        return newArr;
    }

    private JToken GenerateValue(JToken token, int sequenceCounter = 0)
    {
        var tokenValue = token.ToString();

        if (tokenValue.StartsWith(Tokens.ReferenceIdentifier))
        {
            var referencedSection = template.GetValue(tokenValue) as JObject;
            return RandomizePropertyValues(RepeatPropertyValues(referencedSection!), sequenceCounter);
        }

        if (!tokenValue.StartsWith(Tokens.TokenIdentifier)) return token;

        var generator = generatorCache.GetOrCreate(tokenValue,
            () => typeGeneratorFactory.CreateTypeGenerator(tokenValue));

        return new JValue(generator.Execute());
    }
}
