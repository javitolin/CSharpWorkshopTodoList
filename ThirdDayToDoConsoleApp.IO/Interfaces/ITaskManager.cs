

using System.Threading;
using ThirdDayToDoConsoleApp.Entities;

namespace ThirdDayToDoConsoleApp.IO.Interfaces
{
    public delegate void TaskAddedHandler(TaskItem task);

    public interface ITaskManager
    {
        public event TaskAddedHandler? TaskAdded;

        Task AddTaskAsync(TaskItem task, CancellationToken cancellationToken);
        Task<IEnumerable<TaskItem>> GetAllTasksAsync(CancellationToken cancellationToken);
        Task<TaskItem?> GetTaskByIdAsync(Guid taskId, CancellationToken cancellationToken);
        Task<bool> RemoveTaskAsync(Guid taskId, CancellationToken cancellationToken);
        Task<bool> UpdateTask(Guid taskId, TaskItem updatedTask, CancellationToken cancellationToken);
    }
}