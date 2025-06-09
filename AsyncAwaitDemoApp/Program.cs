class Program
{

    // Add cancellation token

    static async Task Main(string[] args)
    {
        Console.WriteLine("Main started");

        var result = DoWorkAsync();

        while (!result.IsCompleted)
        {
            Console.WriteLine("Showing nice loading graphic");
            await Task.Delay(1000);
        }

        Console.WriteLine($"Result: {result.Result}");
        Console.WriteLine("Main finished");
    }

    static async Task<string> DoWorkAsync()
    {
        Console.WriteLine("DoWorkAsync started");

        var data = await SimulateLongRunningJob();
        Console.WriteLine("Back from SimulateLongRunningJob");

        return data.ToUpper();
    }

    static async Task<string> SimulateLongRunningJob()
    {
        Console.WriteLine("Simulating long running IO operation...");
        await Task.Delay(5000);
        Console.WriteLine("Finished simulating operation");
        return "hello async world";
    }
}
