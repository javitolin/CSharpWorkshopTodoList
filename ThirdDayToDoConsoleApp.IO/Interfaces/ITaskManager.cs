

using ThirdDayToDoConsoleApp.Entities;

namespace ThirdDayToDoConsoleApp.IO.Interfaces
{
    public delegate void TaskAddedHandler(TaskItem task);

    public interface ITaskManager
    {
        public event TaskAddedHandler? TaskAdded;

        Task AddTask(TaskItem task);
        Task<IEnumerable<TaskItem>> GetAllTasks();
        Task<TaskItem?> GetTaskById(Guid taskId);
        Task<bool> RemoveTask(Guid taskId);
        Task<bool> UpdateTask(Guid taskId, TaskItem updatedTask);
    }
}