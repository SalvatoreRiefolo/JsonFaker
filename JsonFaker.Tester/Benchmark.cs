using BenchmarkDotNet.Attributes;
using Newtonsoft.Json.Linq;

namespace JsonFaker.Tester;

[MemoryDiagnoser]
public class Benchmark
{
    private readonly JsonFaker gen;

    public Benchmark()
    {
        //var template = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "template.dev.json"));
        var jobj = JObject.Parse("{ \"main\": {\"array\":  [ \"100 $repeat 100\"]} }");
        gen = new JsonFaker(jobj);
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