using BenchmarkDotNet.Attributes;
using Newtonsoft.Json.Linq;

namespace SFR.TemplateRandomizer.Tester
{
    [MemoryDiagnoser]
    public class Benchmark
    {
        private readonly JObject jobj;
        private readonly TemplateRandomizer gen;

        public Benchmark()
        {
            var template = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "template.dev.json"));
            jobj = JObject.Parse(template);
            gen = new TemplateRandomizer(jobj);
        }

        [Benchmark(Baseline = true)]
        public JObject Complete()
        {
            return gen.Randomize();
        }

        // [Benchmark]
        // public JObject ReplacePropertiesRepeatOnly()
        // {
        //     return gen.RepeatProperties(jobj);
        // }
        //
        // [Benchmark]
        // public JObject AddRepeatedPropertiesOnly()
        // {
        //     return gen.AddRepeatedProperties(jobj);
        // }
    }
}
