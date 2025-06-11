using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TodoWebApp.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // 1. Set up DI container
            var host = Host.CreateDefaultBuilder(args)
                 .ConfigureAppConfiguration((context, config) =>
                 {
                     // 3. Optional: Add extra config sources here
                     config.AddJsonFile("appsettings.json", optional: true);
                     config.AddEnvironmentVariables();
                     config.AddCommandLine(args);
                 })
                // 2. Configure services
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<App>();
                    services.AddHttpClient();
                }).Build();

            var cancellationToken = host.Services
                .GetRequiredService<IHostApplicationLifetime>()
                .ApplicationStopping;

            // 4. Run your app
            var app = host.Services.GetRequiredService<App>();

            await app.RunAsync(cancellationToken);
        }
    }
}
