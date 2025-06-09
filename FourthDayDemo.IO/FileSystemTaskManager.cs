using FourthDayDemo.Entities;
using FourthDayDemo.IO.Interfaces;
using System.IO.Abstractions;
using System.Text.Json;

namespace FourthDayDemo.IO
{
    public class FileSystemTaskManager : ITaskManager
    {
        private readonly string _filePath = "tasks.json";
        private IFileSystem _fileSystem;

        public FileSystemTaskManager(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public event TaskItemHandler? OnTaskAdded;

        public async Task AddTaskAsync(TaskItem task, CancellationToken cancellationToken)
        {
            var tasks = await LoadTasksAsync(cancellationToken);
            if (task.Id == Guid.Empty)
                task.Id = Guid.NewGuid();

            tasks.Add(task);
            await SaveTasksAsync(tasks, cancellationToken);

            OnTaskAdded?.Invoke(task);
        }

        public async Task<IEnumerable<TaskItem>> GetAllTasksAsync(CancellationToken cancellationToken)
        {
            return await LoadTasksAsync(cancellationToken);
        }

        public async Task<TaskItem?> GetTaskByIdAsync(Guid taskId, CancellationToken cancellationToken)
        {
            var tasks = await LoadTasksAsync(cancellationToken);
            return tasks.FirstOrDefault(t => t.Id == taskId);
        }

        public async Task<bool> RemoveTaskAsync(Guid taskId, CancellationToken cancellationToken)
        {
            var tasks = await LoadTasksAsync(cancellationToken);
            var task = tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                return false;

            tasks.Remove(task);
            await SaveTasksAsync(tasks, cancellationToken);
            return true;
        }

        public async Task<bool> UpdateTask(Guid taskId, TaskItem updatedTask, CancellationToken cancellationToken)
        {
            var tasks = await LoadTasksAsync(cancellationToken);
            var index = tasks.FindIndex(t => t.Id == taskId);
            if (index == -1)
                return false;

            tasks[index] = updatedTask;
            await SaveTasksAsync(tasks, cancellationToken);
            return true;
        }

        private async Task<List<TaskItem>> LoadTasksAsync(CancellationToken cancellationToken)
        {
            if (!_fileSystem.File.Exists(_filePath))
                return new List<TaskItem>();

            var json = await _fileSystem.File.ReadAllTextAsync(_filePath, cancellationToken);
            if (string.IsNullOrWhiteSpace(json))
                return new List<TaskItem>();

            return JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
        }

        private async Task SaveTasksAsync(List<TaskItem> tasks, CancellationToken cancellationToken)
        {
            var json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
            await _fileSystem.File.WriteAllTextAsync(_filePath, json, cancellationToken);
        }
    }
}