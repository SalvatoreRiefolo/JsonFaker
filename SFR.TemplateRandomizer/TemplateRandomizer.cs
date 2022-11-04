using Newtonsoft.Json.Linq;
using SFR.TemplateGenerator.ArgumentParsers;
using SFR.TemplateGenerator.ArgumentParsers.RangeParsers;
using SFR.TemplateRandomizer.Constants;
using SFR.TemplateRandomizer.TypeGenerator;

namespace SFR.TemplateRandomizer
{
    public partial class TemplateRandomizer
    {
        public JObject Template
        {
            get
            {
                return new JObject(template);
            }
            private set
            {
                template = value;
            }
        }

        private JObject template;
        private readonly Random random = new(Environment.TickCount);
        private readonly IArgumentParser<int> rangeParser = new IntegerRangeParser();
        private readonly IDictionary<string, ITypeGenerator> typeGenerators = new Dictionary<string, ITypeGenerator>();

        public TemplateRandomizer(JObject template)
        {
            this.Template = template ?? throw new ArgumentNullException(nameof(template));
        }

        public JObject Randomize()
            => RandomizeProperties(RepeatProperties(AddRepeatedProperties(Template)));

        internal JObject AddRepeatedProperties(JObject jobject)
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

        // try to iterate on each property 2 times to perform repeat && replace
        internal JObject RepeatProperties(JObject jobject)
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
                            var itemValue = arrItem.ToString();

                            if (arrItem.Type == JTokenType.Object)
                            {
                                arr.Add(RepeatProperties((JObject)arrItem));
                                continue;
                            }

                            if (itemValue.Contains(Tokens.Repeat))
                            {
                                var tokens = itemValue.Split(Tokens.Repeat, StringSplitOptions.TrimEntries);
                                var (low, high) = rangeParser.Parse(tokens[1]);

                                for (int i = 0; i < random.Next(low, high); i++)
                                {
                                    arr.Add($"{tokens[0]}");
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

        internal JObject RandomizeProperties(JObject jobject, int seqCounter = -1)
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
            if (tokenValue.StartsWith('&'))
                return RandomizeProperties(RepeatProperties(this.Template.GetValue(tokenValue) as JObject), seqCounter);

            if (!tokenValue.StartsWith('$'))
                return token;

            ITypeGenerator generator = this.typeGenerators.GetOrCreate(tokenValue, () => CreateTypeGenerator(tokenValue));
            return new JValue(generator.Execute());
        }

        private ITypeGenerator CreateTypeGenerator(string token)
        {
            var tokens = token.Split(' ');

            var args = tokens.Length > 1 ? tokens[1] : null;

            return tokens[0] switch
            {
                Tokens.Integer => new IntegerGenerator(random, args),
                Tokens.String => new StringGenerator(random, args),
                Tokens.Double => new DoubleGenerator(random, args),
                Tokens.Date => new DateGenerator(random, args),
                _ => throw new NotImplementedException($"Type generator for type '{tokens[0]}' not implemented")
            };
        }
    }

    public static class Extensions
    {
        public static T GetOrCreate<T>(this IDictionary<string, T> cache, string key, Func<T> generator)
        {
            if (cache.TryGetValue(key, out T value))
                return value;

            T result = generator();
            cache[key] = result;

            return result;
        }
    }
}
