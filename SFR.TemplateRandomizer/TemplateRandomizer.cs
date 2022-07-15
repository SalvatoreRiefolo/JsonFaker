using Drafts.Constants;
using Drafts.Directives;
using Drafts.Parsers;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace Drafts
{
    public partial class TemplateRandomizer
    {
        private readonly Random random = new(Environment.TickCount);
        private readonly IArgumentParser<(int, int)> rangeParser = new IntegerRangeParser();

        private readonly IDictionary<string, IDirective> directives = new Dictionary<string, IDirective>();            
        private readonly JObject template;

        public TemplateRandomizer(JObject template)
        {
            this.template = template ?? throw new ArgumentNullException(nameof(template));
        }

        public JObject Randomize()
            => RandomizeProperties(RepeatProperties(this.template));

        public JObject RepeatProperties(JObject jobject)
        {
            var result = new JObject();

            foreach (var prop in jobject.Properties())
            {
                var current = new JProperty(prop.Name);

                switch (prop.Value.Type)
                {
                    case JTokenType.Object:
                        current.Value = RepeatProperties((JObject)prop.Value);
                        break;

                    case JTokenType.Array:
                        var arr = new JArray();
                        foreach (var arrItem in (JArray)prop.Value)
                        {
                            var itemValue = arrItem.ToString();

                            if (arrItem.Type == JTokenType.Object)
                            {
                                arr.Add(RepeatProperties((JObject)arrItem));
                                continue;
                            }

                            if (itemValue.Contains(Tokens.Repeat))
                            {
                                var tokens = Regex.Split(itemValue, Patterns.Repeat);
                                var (Low, High) = this.rangeParser.Parse(tokens[1]);

                                for (int i = 0; i < this.random.Next(Low, High); i++)
                                {
                                    arr.Add($"{tokens.First().Trim()}");
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
                        current.Value = prop.Value;
                        break;
                }

                if (current.Name.Contains(Tokens.Repeat))
                {
                    var tokens = Regex.Split(current.Name, Patterns.Repeat);
                    var (low, high) = this.rangeParser.Parse(tokens[1]);

                    for (int i = 0; i < this.random.Next(low, high); i++)
                    {
                        result.Add($"{tokens.First()}c_{i}", current.Value);
                    }
                }
                else
                {
                    result.Add(current);
                }
            }

            return result;
        }       
        
        public JObject RandomizeProperties(JObject jobject)
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
                        foreach (var item in (JArray)prop.Value)
                        {
                            if (item.Type == JTokenType.Object)
                                arr.Add(RandomizeProperties((JObject)item));
                            else
                                arr.Add(SwapToken(item));
                        }

                        result[prop.Name] = arr;
                        break;

                    default:
                        result[prop.Name] = SwapToken(prop.Value);
                        break;
                }
            }

            return result;
        }
       
        //private JToken SwapToken(JToken token)
        //{
        //    var tokenValue = token.ToString();
        //    var tokens = tokenValue.Split(' ');           

        //    var (low, high) = tokens.Length != 1 ? ParseRange(tokenValue, tokens[1]) : (0, 100);

        //    if (tokens[0].StartsWith('&')) return "TBD";

        //    return tokens[0] switch
        //    {
        //        Tokens.Integer => Integer(low, high),
        //        Tokens.String => NextRandomString(low, high),
        //        _ => token
        //    };
        //}

        //public (int low, int high) ParseRange(string token, string args)
        //{
        //    return this.cache.GetOrCreate(token, () =>
        //    {
        //        var match = this.rangeRegex.Match(args);

        //        var start = int.Parse(match.Groups["start"].Value);
        //        var end = match.Groups["end"].Value == string.Empty ? start : int.Parse(match.Groups["end"].Value) + 1;

        //        return (start, end);
        //    });
        //}       

        private JToken SwapToken(JToken token)
        {
            if (!token.ToString().StartsWith('$')) 
                return token;

            IDirective directive = this.directives.GetOrCreate(token.ToString(), () => CreateDirective(token));
            return new JValue(directive.Execute());
        }

        private IDirective CreateDirective(JToken token)
        {
            var tokens = token.ToString().Split(' ');

            var args = tokens.Length > 1 ? tokens[1] : null;

            return tokens[0] switch
            {
                Tokens.Integer => new IntegerDirective(this.random, args),
                Tokens.String => new StringDirective(this.random, args),
                _ => throw new NotImplementedException($"Directive '{tokens[0]}' not implemented")
            };
        }

        //private int Integer(int min = int.MinValue, int max = int.MaxValue) => this.random.Next(min, max);

        //private string NextRandomString(int minLen, int maxLen)
        //{
        //    var count = random.Next(minLen, maxLen);
        //    var res = new char[count];

        //    for (int i = 0; i < count; i++)
        //    {
        //        res[i] = AllowedChars[random.Next(0, AllowedChars.Length)];
        //    }

        //    return new string(res);
        //}
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
