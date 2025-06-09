using FourthDayDemo.Entities;
using FourthDayDemo.IO;
using FourthDayDemo.IO.Interfaces;
using System.IO.Abstractions;

namespace FourthDayDemo
{
    class Program
    {
        static ITaskManager TaskManager = new FileSystemTaskManager(new FileSystem());

        static async Task Main(string[] args)
        {
            using var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            TaskManager.OnTaskAdded += TaskManagerOnTaskAdded;

            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                Console.WriteLine("Cancellation requested...");
                eventArgs.Cancel = true;
                cancellationTokenSource.Cancel();                
            };

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
                        await AddTaskAsync(cancellationToken);
                        break;
                    case "list":
                        await ListTasksAsync(cancellationToken);
                        break;
                    case "delete":
                        await DeleteTaskAsync(cancellationToken);
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

        private static void TaskManagerOnTaskAdded(TaskItem task)
        {
            Console.WriteLine($"New task added (OnTaskAdded): {task}");
        }

        private static void PrintMenu()
        {
            Console.Clear();
            Console.WriteLine("=== Task Tracker ===");
            Console.WriteLine("Add: Add new task");
            Console.WriteLine("List: List all tasks");
            Console.WriteLine("Delete: Delete a task");
            Console.WriteLine("Exit: Exit");
            Console.Write("Choose an option: ");
        }

        static async Task AddTaskAsync(CancellationToken cancellationToken)
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

            await TaskManager.AddTaskAsync(task, cancellationToken);

            Console.WriteLine("Task added! Press Enter to return to menu.");
            Console.ReadLine();
        }

        static async Task ListTasksAsync(CancellationToken cancellationToken)
        {
            var tasks = await TaskManager.GetAllTasksAsync(cancellationToken);
            if (tasks.Count() == 0)
            {
                Console.WriteLine("No tasks yet!");
            }
            else
            {
                foreach (var task in tasks)
                {
                    Console.WriteLine($"{task.Id} - {task.Title}");
                }
            }
            Console.WriteLine("\nPress Enter to return to menu.");
            Console.ReadLine();
        }

        static async Task DeleteTaskAsync(CancellationToken cancellationToken)
        {
            Console.Write("Enter task ID to delete: ");
            var input = Console.ReadLine();

            if (Guid.TryParse(input, out var id))
            {
                var success = await TaskManager.RemoveTaskAsync(id, cancellationToken);
                var message = success ? "Task deleted successfully!" : "Task not found.";
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
