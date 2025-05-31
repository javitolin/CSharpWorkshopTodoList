using System.IO.Abstractions;
using System.Text.Json;
using FourthDayToDoConsoleApp.Entities;
using FourthDayToDoConsoleApp.IO.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FourthDayToDoConsoleApp
{
    public class FileSystemTaskManager : ITaskManager
    {
        private string _filePath = "tasks.json";
        private ILogger<FileSystemTaskManager> _logger;
        private IFileSystem _fileSystem;

        public FileSystemTaskManager(IConfiguration configuration, ILogger<FileSystemTaskManager> logger, IFileSystem fileSystem)
        {
            _logger = logger;
            _fileSystem = fileSystem;

            _filePath = configuration["filename"] ?? "tasks.json";

            _logger.LogInformation("FileSystemTaskManager initialized with file path: {FilePath}", _filePath);
        }

        public event TaskAddedHandler? TaskAdded;

        public async Task AddTask(TaskItem task)
        {
            var tasks = await LoadTasks();
            if (task.Id == Guid.Empty)
                task.Id = Guid.NewGuid();

            tasks.Add(task);
            await SaveTasks(tasks);

            TaskAdded?.Invoke(task);
        }

        public async Task<IEnumerable<TaskItem>> GetAllTasks()
        {
            return await LoadTasks();
        }

        public async Task<TaskItem?> GetTaskById(Guid taskId)
        {
            var tasks = await LoadTasks();
            return tasks.FirstOrDefault(t => t.Id == taskId);
        }

        public async Task<bool> RemoveTask(Guid taskId)
        {
            var tasks = await LoadTasks();
            var task = tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                return false;

            tasks.Remove(task);
            await SaveTasks(tasks);
            return true;
        }

        public async Task<bool> UpdateTask(Guid taskId, TaskItem updatedTask)
        {
            var tasks = await LoadTasks();
            var index = tasks.FindIndex(t => t.Id == taskId);
            if (index == -1)
                return false;

            tasks[index] = updatedTask;
            await SaveTasks(tasks);
            return true;
        }

        private Task<List<TaskItem>> LoadTasks()
        {
            return Task.Run(() =>
            {
                if (!_fileSystem.File.Exists(_filePath))
                    return new List<TaskItem>();

                var json = _fileSystem.File.ReadAllText(_filePath);
                if (string.IsNullOrWhiteSpace(json))
                    return new List<TaskItem>();

                return JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
            });
        }

        private Task SaveTasks(List<TaskItem> tasks)
        {
            return Task.Run(() =>
            {
                var json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
                _fileSystem.File.WriteAllText(_filePath, json);
            });

        }
    }
}