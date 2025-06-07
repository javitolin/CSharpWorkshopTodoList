namespace ToDoConsoleApp
{
    using SecondDayToDoConsoleApp.Entities;
    using System;

    class Program
    {
        static ITaskManager<TaskItem> TaskManager = new InMemoryTaskManager();

        static void Main()
        {
            while (true)
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
                        AddTask();
                        break;
                    case "list":
                        ListTasks();
                        break;
                    case "delete":
                        DeleteTask();
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
            Console.Clear();
            Console.WriteLine("=== Task Tracker ===");
            Console.WriteLine("Add: Add new task");
            Console.WriteLine("List: List all tasks");
            Console.WriteLine("Delete: Delete a task");
            Console.WriteLine("Exit: Exit");
            Console.Write("Choose an option: ");
        }

        static void AddTask()
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

            TaskManager.AddTask(task);

            Console.WriteLine("Task added! Press Enter to return to menu.");
            Console.ReadLine();
        }

        static void ListTasks()
        {
            var tasks = TaskManager.GetAllTasks();
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

        static void DeleteTask()
        {
            Console.Write("Enter task ID to delete: ");
            var input = Console.ReadLine();

            if (Guid.TryParse(input, out var id))
            {
                var success = TaskManager.RemoveTask(id);
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
