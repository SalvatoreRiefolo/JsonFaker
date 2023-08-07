using BenchmarkDotNet.Running;
using Newtonsoft.Json.Linq;

namespace TemplateRandomizer.Tester;

// TODO increment, decrement and step for $seq keyword
// TODO datetime requires full date, possibility to use only part of date (year, year+month)
// TODO max decimal digits for double
// TODO replace CreateTypeGenerator with factory
// TODO create struct with token parts
// TODO ITypeGenerator ritorna string invece che object
// TODO double troppo grandi senza upper bound

public static class Program
{
    public static async Task Main()
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
        var path = Path.Combine(Directory.GetCurrentDirectory(), "template.dev.jsonc");
        var json = File.ReadAllText(path);
        var gen = new TemplateRandomizer(JObject.Parse(json)
        //, new Configuration
        // {
        //     Integer =
        //     {
        //         Min = 0,
        //         Max = 1000 
        //     },
        //     Double =
        //     {
        //         Min = -50,
        //         Max = +50,
        //         MaxDecimalDigits = new Range(2, 4) 
        //     },
        //     String =
        //     {
        //         Casing = StringCase.Uppercase,
        //         IncludeDigits = true
        //     },
        //     Date =
        //     {
        //         Min = DateTimeOffset.UnixEpoch,
        //         Max = DateTimeOffset.Parse("2020/12/31"),
        //         Format = "u",
        //     }
        // }
        );

        while (!cancellationToken.IsCancellationRequested)
        {
            Console.Clear();
            var generated = gen.Randomize(1);
            Console.WriteLine(generated["main"]);
            Console.ReadLine();
        }

        return Task.CompletedTask;
    }
}