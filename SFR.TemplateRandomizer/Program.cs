﻿using BenchmarkDotNet.Running;
using Newtonsoft.Json.Linq;

namespace SFR.TemplateRandomizer
{
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

            var timer = new PeriodicTimer(TimeSpan.FromSeconds(1));

            while (await timer.WaitForNextTickAsync(cancellationToken) && !cancellationToken.IsCancellationRequested)
            {
                Console.Clear();
                var generated = gen.Randomize();
                Console.WriteLine(generated["main"]);
            }
        }
    }
}
