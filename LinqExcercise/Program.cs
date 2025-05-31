using SecondDayToDoConsoleApp.Entities;

namespace LinqExcercise
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<TaskItem> tasks = new()
            {
                new TaskItem { Title = "Fix television" },
                new TaskItem { Title = "fix computer" },
                new TaskItem { Title = "Fix fridge", Description = "This is urgent!" },
                new TaskItem { Title = "This is a very long title to demonstrate something. What? I don't know", Description = "[Urgent]" },
                new TaskItem { Title = "Water the plants" },
                new TaskItem { Title = "Take out the trash" },
            };

            // 1. Return all tasks where the title starts with "Fix".
            var tasksStartingWithFix = tasks.Where(task => task.Title.ToLower().StartsWith("fix")).ToList();
            PrintList(tasksStartingWithFix, "Tasks starting with fix");
            Console.WriteLine($"------------------------------");


            // 2. Get all titles of tasks that have a non-empty description.
            var titlesWithDescription = tasks
                .Where(task => !string.IsNullOrEmpty(task.Description))
                .Select(task => task.Title)
                .ToList();
            PrintList(titlesWithDescription, "Titles of tasks with description");
            Console.WriteLine($"------------------------------");

            // 3. Count how many tasks have the word "urgent" in the description.
            var urgentCount = tasks
                .Count(task => task.Description != null && task.Description.ToLower().Contains("urgent"));
            Console.WriteLine($"Number of tasks with 'urgent' in the description: {urgentCount}");
            Console.WriteLine($"------------------------------");

            // 4. Return a list of IDs of tasks whose title is longer than 20 characters.
            var longTitleTaskIds = tasks
                .Where(task => task.Title.Length > 20)
                .Select(task => task.Id)
                .ToList();
            PrintList(longTitleTaskIds, "IDs of long title tasks");
            Console.WriteLine($"------------------------------");

            // 5. Return the task with the longest description.
            var taskWithLongestDescription = tasks
                .Where(task => task.Description != null)
                .OrderByDescending(task => task.Description?.Length)
                .FirstOrDefault();
            Console.WriteLine($"Task with the longest description: [{taskWithLongestDescription}]");
            Console.WriteLine($"------------------------------");

            // 6. Group tasks by the first letter of the title.
            var groupedTasks = tasks
                .GroupBy(task => task.Title[0].ToString().ToLower())
                .ToDictionary(group => group.Key, group => group.ToList());
            foreach (var group in groupedTasks)
            {
                PrintList(group.Value, $"Tasks starting with '{group.Key}'");
            }
            Console.WriteLine($"------------------------------");

        }

        static void PrintList<T>(List<T> items, string title)
        {
            Console.WriteLine($"{title}:");
            foreach (var item in items)
            {
                Console.WriteLine($"\t{item}");
            }
        }
    }
}
