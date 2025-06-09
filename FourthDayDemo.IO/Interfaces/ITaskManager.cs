using FourthDayDemo.Entities;

namespace FourthDayDemo.IO.Interfaces
{
    public delegate void TaskItemHandler(TaskItem task);

    public interface ITaskManager
    {
        public event TaskItemHandler? OnTaskAdded;

        Task AddTaskAsync(TaskItem task, CancellationToken cancellationToken);
        Task<IEnumerable<TaskItem>> GetAllTasksAsync(CancellationToken cancellationToken);
        Task<TaskItem?> GetTaskByIdAsync(Guid taskId, CancellationToken cancellationToken);
        Task<bool> RemoveTaskAsync(Guid taskId, CancellationToken cancellationToken);
        Task<bool> UpdateTask(Guid taskId, TaskItem updatedTask, CancellationToken cancellationToken);
    }
}