namespace ToDoConsoleApp
{
    public class TaskManager : ITaskManager
    {
        private List<TaskItem> tasks = new List<TaskItem>();
        public void AddTask(TaskItem task)
        {
            tasks.Add(task);
        }

        public bool RemoveTask(Guid taskId)
        {
            var task = tasks.FirstOrDefault(t => t.Id == taskId);
            if (task is null)
                return false;

            return tasks.Remove(task);
        }

        public bool UpdateTask(Guid taskId, TaskItem updatedTask)
        {
            var taskIndex = tasks.FindIndex(t => t.Id == taskId);
            if (taskIndex < 0)
                return false;

            tasks[taskIndex] = updatedTask;
            return true;
        }

        public List<TaskItem> GetAllTasks()
        {
            return tasks;
        }

        public TaskItem? GetTaskById(Guid taskId)
        {
            return tasks.FirstOrDefault(t => t.Id == taskId);
        }
    }
}
