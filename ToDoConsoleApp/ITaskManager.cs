﻿
namespace ToDoConsoleApp
{
    public interface ITaskManager
    {
        void AddTask(TaskItem task);
        List<TaskItem> GetAllTasks();
        TaskItem? GetTaskById(Guid taskId);
        bool RemoveTask(Guid taskId);
        bool UpdateTask(Guid taskId, TaskItem updatedTask);
    }
}