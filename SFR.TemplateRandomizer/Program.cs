using BenchmarkDotNet.Running;
using Newtonsoft.Json.Linq;

namespace SFR.TemplateRandomizer
{
    // TODO datetime richiede data completa, possibilità di usare solo parte della data (anno, anno+mese)
    // TODO max cifre decimali per double
    // TODO regex componibile per argomenti aggiuntivi 
    // TODO check se max < min
    // TODO possibilità di definire solo min o solo max (10.. | ..100)
    // TODO defaults globali

    public static class Program
    {
        public static async Task Main(string[] args)
        {
#if DEBUG
            var cts = new CancellationTokenSource();
            await PrintResult(cts.Token);
#endif
#if (!DEBUG)
            BenchmarkRunner.Run<Benchmark>();
#endif
        }

        public static async Task PrintResult(CancellationToken cancellationToken)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "template.json");
            string json = File.ReadAllText(path);
            var gen = new TemplateRandomizer(JObject.Parse(json));

            while (!cancellationToken.IsCancellationRequested)
            {
                Console.Clear();
                var generated = gen.Randomize();
                Console.WriteLine(generated["main"]);
                Console.ReadLine();
            }
        }
    }
}
