using Newtonsoft.Json.Linq;

namespace Drafts
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            //BenchmarkRunner.Run<Benchmark>();
            var cts = new CancellationTokenSource();

            var path = Path.Combine(Directory.GetCurrentDirectory(), "template.json");
            string json = File.ReadAllText(path);
            var gen = new TemplateRandomizer(JObject.Parse(json));

            var timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
            while(await timer.WaitForNextTickAsync(cts.Token) && !cts.Token.IsCancellationRequested)
            {
                Console.Clear();
                var generated = gen.Randomize();
                Console.WriteLine(generated["main"]);
            }
        }
    }
}
