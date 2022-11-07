using Newtonsoft.Json.Linq;
using SFR.TemplateGenerator.Parsers;
using SFR.TemplateRandomizer.TypeGenerators;
using SFR.TemplateRandomizer.TypeGenerators.Abstractions;
using SFR.TemplateRandomizer.TypeGenerators.Constants;

namespace SFR.TemplateRandomizer
{
    public class TemplateRandomizer
    {
        private readonly JObject template;

        private readonly Random random = new(Environment.TickCount);
        private readonly IArgumentParser<(int, int)> rangeParser = new IntegerRangeParser();
        private readonly IDictionary<string, ITypeGenerator> typeGenerators = new Dictionary<string, ITypeGenerator>();
        private readonly ITypeGeneratorFactory typeGeneratorFactory;

        public TemplateRandomizer(JObject template, RandomizerSettings settings = null)
        {
            this.template = template ?? throw new ArgumentNullException(nameof(template));
            typeGeneratorFactory = new TypeGeneratorFactory(this.random);
        }

        public JObject Randomize()
            => RandomizeProperties(RepeatProperties(AddRepeatedProperties(template)));

        private JObject AddRepeatedProperties(JObject jobject)
        {
            var result = new JObject();

            foreach (var prop in jobject.Properties())
            {
                var current = new JProperty(prop.Name);

                if (current.Name.Contains(Tokens.Repeat))
                {
                    var tokens = current.Name.Split(Tokens.Repeat, StringSplitOptions.TrimEntries);
                    var (low, high) = rangeParser.Parse(tokens[1]);

                    for (int i = 0; i < random.Next(low, high); i++)
                    {
                        var retryCount = 5;
                        while (!result.TryAdd(SwapToken(tokens[0]).ToString(), prop.Value) && retryCount > 0)
                            retryCount--;

                        if (retryCount == 0)
                            result.Add($"{SwapToken(tokens[0])}_{i}", prop.Value);
                    }

                    continue;
                }
                else
                {
                    current.Value = prop.Value;
                }

                if (prop.Value.Type == JTokenType.Object)
                {
                    current.Value = AddRepeatedProperties((JObject)prop.Value);
                }

                result.Add(current);
            }

            return result;
        }

        private JObject RepeatProperties(JObject jobject)
        {
            var result = new JObject();

            foreach (var property in jobject.Properties())
            {
                var current = new JProperty(property.Name);

                switch (property.Value.Type)
                {
                    case JTokenType.Object:
                        current.Value = RepeatProperties((JObject)property.Value);
                        break;

                    case JTokenType.Array:
                        var arr = new JArray();
                        foreach (var arrItem in (JArray)property.Value)
                        {
                            if (arrItem.Type == JTokenType.Object)
                            {
                                arr.Add(RepeatProperties((JObject)arrItem));
                                continue;
                            }
                            
                            var itemValue = arrItem.ToString();

                            if (itemValue.Contains(Tokens.Repeat))
                            {
                                var tokens = itemValue.Split(Tokens.Repeat, StringSplitOptions.TrimEntries);
                                var (low, high) = rangeParser.Parse(tokens[1]);

                                for (int i = 0; i < random.Next(low, high); i++)
                                {
                                    arr.Add(tokens[0]);
                                }
                            }
                            else
                            {
                                arr.Add(arrItem);
                            }
                        }

                        current.Value = arr;
                        break;

                    default:
                        current.Value = property.Value;
                        break;
                }

                result.Add(current);
            }

            return result;
        }

        private JObject RandomizeProperties(JObject jobject, int seqCounter = -1)
        {
            var result = new JObject();

            foreach (var prop in jobject.Properties())
            {
                switch (prop.Value.Type)
                {
                    case JTokenType.Object:
                        result[prop.Name] = RandomizeProperties((JObject)prop.Value);
                        break;

                    case JTokenType.Array:
                        var arr = new JArray();

                        var currentArr = (JArray)prop.Value;
                        for (int i = 0; i < currentArr.Count; i++)
                        {
                            var item = currentArr[i];
                            if (item.Type == JTokenType.Object)
                                arr.Add(RandomizeProperties((JObject)item, i));
                            else
                                arr.Add(SwapToken(item, i));
                        }

                        result[prop.Name] = arr;
                        break;

                    default:
                        var value = prop.Value.ToString() == Tokens.Sequence ? seqCounter : SwapToken(prop.Value);
                        result[prop.Name] = value;
                        break;
                }
            }

            return result;
        }

        private JToken SwapToken(JToken token, int seqCounter = -1)
        {
            var tokenValue = token.ToString();
            if (tokenValue.StartsWith(Tokens.ReferenceSymbol))
                return RandomizeProperties(RepeatProperties(this.template.GetValue(tokenValue) as JObject), seqCounter);

            if (!tokenValue.StartsWith(Tokens.TokenSymbol))
                return token;

            var generator = typeGenerators.GetOrCreate(token.ToString(), () => typeGeneratorFactory.CreateTypeGenerator(token.ToString()));
            return new JValue(generator.Execute());
        }
    }
}
