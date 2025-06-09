using System.IO.Abstractions;
using System.Text.Json;
using System.Threading;
using ThirdDayToDoConsoleApp.Entities;
using ThirdDayToDoConsoleApp.IO.Interfaces;

namespace ThirdDayToDoConsoleApp
{
    public class FileSystemTaskManager : ITaskManager
    {
        private readonly string _filePath = "tasks.json";
        private IFileSystem _fileSystem;

        public FileSystemTaskManager(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public event TaskAddedHandler? TaskAdded;

        public async Task AddTaskAsync(TaskItem task, CancellationToken cancellationToken)
        {
            var tasks = await LoadTasksAsync(cancellationToken);
            if (task.Id == Guid.Empty)
                task.Id = Guid.NewGuid();

            tasks.Add(task);
            await SaveTasksAsync(tasks, cancellationToken);

            TaskAdded?.Invoke(task);
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

        private Task<List<TaskItem>> LoadTasksAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                if (!_fileSystem.File.Exists(_filePath))
                    return new List<TaskItem>();

                var json = _fileSystem.File.ReadAllText(_filePath);
                if (string.IsNullOrWhiteSpace(json))
                    return new List<TaskItem>();

                return JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
            }, cancellationToken);
        }

        private Task SaveTasksAsync(List<TaskItem> tasks, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
                _fileSystem.File.WriteAllText(_filePath, json);
            }, cancellationToken);

        }
    }
}