using System.Runtime.CompilerServices;
using System.Threading;

class Program
{

    // Add cancellation token

    static async Task Main(string[] args)
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        Console.CancelKeyPress += (sender, eventArgs) =>
        {
            Console.WriteLine("Cancellation requested...");
            eventArgs.Cancel = true;
            cancellationTokenSource.Cancel();
        };

        Console.WriteLine("Main started");
        await GetRandomWords();

        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(200000);
            Console.WriteLine("HERE");
        }

        Console.WriteLine("Closing gracefuly");
    }

    static Task<string> DoWorkAsync()
    {
        Console.WriteLine("DoWorkAsync started");

        Task.WhenAll(SimulateLongRunningJob());

        Console.WriteLine("Back from SimulateLongRunningJob");
        return Task.FromResult("Done!");
    }

    static async Task<string> SimulateLongRunningJob()
    {
        Console.WriteLine("Simulating long running IO operation...");
        await Task.Delay(1000);
        throw new Exception("Throwing inside SimluateLongRunningJob");

        Console.WriteLine("Finished simulating operation");
        return "hello async world";
    }


    static async Task GetRandomWords()
    {
        var words = ReturnRandomWords(CancellationToken.None);
        await foreach (var word in words)
        {
            Console.WriteLine(word);
        }
    }

    static async IAsyncEnumerable<string> ReturnRandomWords([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        Random random = new Random();
        for (int i = 0; i < 10; i++)
        {
            if (cancellationToken.IsCancellationRequested)
                break;

            await Task.Delay(random.Next(10, 1000), cancellationToken);
            yield return $"Hello - {i}";
        }
    }
}
