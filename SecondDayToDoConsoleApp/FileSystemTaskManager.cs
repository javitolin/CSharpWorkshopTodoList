using System.IO.Abstractions;
using System.Text.Json;
using SecondDayToDoConsoleApp.Entities;
using ToDoConsoleApp;

namespace SecondDayToDoConsoleApp
{
    public class FileSystemTaskManager : ITaskManager<TaskItem>
    {
        private readonly string _filePath = "tasks.json";
        private IFileSystem _fileSystem;

        public FileSystemTaskManager(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public void AddTask(TaskItem task)
        {
            var tasks = LoadTasks();
            if (task.Id == Guid.Empty)
                task.Id = Guid.NewGuid();

            tasks.Add(task);
            SaveTasks(tasks);
        }

        public IEnumerable<TaskItem> GetAllTasks()
        {
            return LoadTasks();
        }

        public TaskItem? GetTaskById(Guid taskId)
        {
            var tasks = LoadTasks();
            return tasks.FirstOrDefault(t => t.Id == taskId);
        }

        public bool RemoveTask(Guid taskId)
        {
            var tasks = LoadTasks();
            var task = tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                return false;

            tasks.Remove(task);
            SaveTasks(tasks);
            return true;
        }

        public bool UpdateTask(Guid taskId, TaskItem updatedTask)
        {
            var tasks = LoadTasks();
            var index = tasks.FindIndex(t => t.Id == taskId);
            if (index == -1)
                return false;

            tasks[index] = updatedTask;
            SaveTasks(tasks);
            return true;
        }

        private List<TaskItem> LoadTasks()
        {
            if (!_fileSystem.File.Exists(_filePath))
                return new List<TaskItem>();

            var json = _fileSystem.File.ReadAllText(_filePath);
            if (string.IsNullOrWhiteSpace(json)) 
                return new List<TaskItem>();

            return JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
        }

        private void SaveTasks(List<TaskItem> tasks)
        {
            var json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
            _fileSystem.File.WriteAllText(_filePath, json);
        }
    }
}