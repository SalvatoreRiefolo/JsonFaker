using BenchmarkDotNet.Attributes;
using Newtonsoft.Json.Linq;

namespace Drafts
{
    [MemoryDiagnoser]
    public class Benchmark
    {
        private readonly JObject jobj;
        private readonly TemplateRandomizer gen;

        public Benchmark()
        {
            var template = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "template.json"));
            this.jobj = JObject.Parse(template);
            this.gen = new TemplateRandomizer(jobj);
        }

        [Benchmark(Baseline = true)]
        public JObject Complete()
        {
            return gen.Randomize();
        }
        
        [Benchmark]
        public JObject ReplaceRepeatOnly()
        {
            return gen.RepeatProperties(jobj);
        }
    }
}
