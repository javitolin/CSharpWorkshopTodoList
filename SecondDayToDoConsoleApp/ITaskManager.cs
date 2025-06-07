
using SecondDayToDoConsoleApp.Entities;

namespace ToDoConsoleApp
{
    public interface ITaskManager<T> where T : TaskItem
    {
        void AddTask(T task);
        IEnumerable<T> GetAllTasks();
        T? GetTaskById(Guid taskId);
        bool RemoveTask(Guid taskId);
        bool UpdateTask(Guid taskId, T updatedTask);
    }
}