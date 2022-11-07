﻿using Newtonsoft.Json.Linq;

namespace SFR.TemplateRandomizer.Tester
{
    // TODO increment, decrement e step in token $seq
    // TODO datetime richiede data completa, possibilità di usare solo parte della data (anno, anno+mese)
    // TODO max cifre decimali per double
    // TODO defaults globali (max/min per tipo, max cifre decimali, datetime format...)
    // TODO sostituire CreateTypeGenerator con factory

    public static class Program
    {
        public static async Task Main(string[] args)
        {
#if DEBUG
            var cts = new CancellationTokenSource();
            await PrintResultAsync(cts.Token);
#endif
#if (!DEBUG)
            BenchmarkRunner.Run<Benchmark>();
#endif
        }

        private static Task PrintResultAsync(CancellationToken cancellationToken)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "template.dev.json");
            string json = File.ReadAllText(path);
            var gen = new TemplateRandomizer(JObject.Parse(json));

            while (!cancellationToken.IsCancellationRequested)
            {
                Console.Clear();
                var generated = gen.Randomize();
                //var generated = gen.ReplaceAuxiliarySections(gen.Template);
                Console.WriteLine(generated["main"]);
                Console.ReadLine();
            }

            return Task.CompletedTask;
        }
    }
}