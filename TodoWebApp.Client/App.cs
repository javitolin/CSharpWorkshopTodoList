using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;
using ToDoWebApp.Entities;

namespace TodoWebApp.Client
{
    public class App
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        public App(ILogger<App> logger, 
            IConfiguration configuration, 
            IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            string? baseUrl = configuration["base_url"] ?? "";
            _httpClient.BaseAddress = new Uri(baseUrl);

            _logger = logger;

            _logger.LogInformation("Started!");
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                PrintMenu();

                var choice = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(choice))
                {
                    Console.WriteLine("Invalid option. Press Enter to continue.");
                    Console.ReadLine();
                    continue;
                }

                choice = choice.Trim().ToLowerInvariant();
                switch (choice)
                {
                    case "add":
                        await AddTask(cancellationToken);
                        break;
                    case "list":
                        await ListTasks(cancellationToken);
                        break;
                    case "delete":
                        await DeleteTask(cancellationToken);
                        break;
                    case "exit":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine($"Invalid option: [{choice}]. Press Enter to continue.");
                        Console.ReadLine();
                        break;
                }
            }
        }

        private static void PrintMenu()
        {
            Console.WriteLine("=== Task Tracker ===");
            Console.WriteLine("Add: Add new task");
            Console.WriteLine("List: List all tasks");
            Console.WriteLine("Delete: Delete a task");
            Console.WriteLine("Exit: Exit");
            Console.Write("Choose an option: ");
        }

        private async Task AddTask(CancellationToken cancellationToken)
        {
            Console.Write("Enter task title: ");
            var title = Console.ReadLine();

            Console.Write("Enter task description (optional): ");
            var description = Console.ReadLine();

            var task = new TaskItem
            {
                Title = title ?? "",
                Description = string.IsNullOrWhiteSpace(description) ? null : description
            };

            // Older
            using StringContent jsonContent = new(JsonSerializer.Serialize(task));
            using HttpResponseMessage response = await _httpClient.PostAsync("Tasks", jsonContent, cancellationToken);
            
            // Newer
            // using HttpResponseMessage response2 = await _httpClient.PostAsJsonAsync("TaskList", task, cancellationToken);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadFromJsonAsync<string>();


            Console.WriteLine("Task added! Press Enter to return to menu.");
            Console.ReadLine();
        }

        private async Task ListTasks(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await _httpClient.GetAsync("Tasks", cancellationToken);
            response.EnsureSuccessStatusCode();

            var tasks = await response.Content.ReadFromJsonAsync<List<TaskItem>>(cancellationToken);

            if (tasks?.Count() == 0)
            {
                Console.WriteLine("No tasks yet!");
                return;
            }

            foreach (var task in tasks ?? [])
            {
                Console.WriteLine($"{task.Id} - {task.Title}");
            }

            Console.WriteLine("\nPress Enter to return to menu.");
            Console.ReadLine();
        }

        private async Task DeleteTask(CancellationToken cancellationToken)
        {
            Console.Write("Enter task ID to delete: ");
            var input = Console.ReadLine();

            if (Guid.TryParse(input, out var id))
            {
                var success = await _httpClient.DeleteAsync($"Tasks/{id}");
                var message = success.IsSuccessStatusCode ? "Task deleted successfully!" : "Task not found.";
                Console.WriteLine(message);
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }

            Console.WriteLine("Press Enter to return to menu.");
            Console.ReadLine();
        }
    }
}

