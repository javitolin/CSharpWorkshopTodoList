using SecondDayToDoConsoleApp.Entities;
using Xunit;

namespace ToDoConsoleApp.Tests
{
public class TaskManagerTests
     {

        [Fact]
        public void AddTask_OneCorrectTask_TaskAdded()
        {
            // Arrange
            var manager = new InMemoryTaskManager();
            var task = new TaskItem { Id = Guid.NewGuid(), Title = "Test Task" };

            // Act
            manager.AddTask(task);

            // Assert
            var allTasks = manager.GetAllTasks().ToList();
            Assert.Single(allTasks);
            Assert.Equal(task.Id, allTasks[0].Id);
            Assert.Equal("Test Task", allTasks[0].Title);
        }







        [Fact]
        public void RemoveTask_TaskExists_TaskRemoved()
        {
            var manager = new InMemoryTaskManager();
            var task = new TaskItem { Id = Guid.NewGuid(), Title = "Task to Remove" };
            manager.AddTask(task);

            var result = manager.RemoveTask(task.Id);

            Assert.True(result);
            Assert.Empty(manager.GetAllTasks());
        }

        [Fact]
        public void RemoveTask_TaskDoesntExists_ReturnsFalse()
        {
            var manager = new InMemoryTaskManager();

            var result = manager.RemoveTask(Guid.NewGuid());

            Assert.False(result);
        }

        [Fact]
        public void UpdateTask_TaskExists_TaskUpdated()
        {
            var manager = new InMemoryTaskManager();
            var task = new TaskItem { Id = Guid.NewGuid(), Title = "Old Title" };
            manager.AddTask(task);

            var updatedTask = new TaskItem { Id = task.Id, Title = "New Title" };
            var result = manager.UpdateTask(task.Id, updatedTask);

            Assert.True(result);
            var fetched = manager.GetTaskById(task.Id);
            Assert.NotNull(fetched);
            Assert.Equal("New Title", fetched.Title);
        }

        [Fact]
        public void UpdateTask_TaskDoesntExist_ReturnsFalse()
        {
            var manager = new InMemoryTaskManager();
            var updatedTask = new TaskItem { Id = Guid.NewGuid(), Title = "Doesn't Matter" };

            var result = manager.UpdateTask(updatedTask.Id, updatedTask);

            Assert.False(result);
        }

        [Fact]
        public void GetAllTasks_TasksExists_ReturnsAllTasks()
        {
            var manager = new InMemoryTaskManager();
            var task1 = new TaskItem { Id = Guid.NewGuid(), Title = "Task 1" };
            var task2 = new TaskItem { Id = Guid.NewGuid(), Title = "Task 2" };
            manager.AddTask(task1);
            manager.AddTask(task2);

            var allTasks = manager.GetAllTasks().ToList();

            Assert.Equal(2, allTasks.Count);
            Assert.Contains(allTasks, t => t.Id == task1.Id);
            Assert.Contains(allTasks, t => t.Id == task2.Id);
        }

        [Fact]
        public void GetTaskById_TaskExists_ReturnsTask()
        {
            var manager = new InMemoryTaskManager();
            var task = new TaskItem { Id = Guid.NewGuid(), Title = "Find Me" };
            manager.AddTask(task);

            var found = manager.GetTaskById(task.Id);

            Assert.NotNull(found);
            Assert.Equal(task.Id, found.Id);
        }

        [Fact]
        public void GetTaskById_TaskNotFound_ReturnsNull()
        {
            var manager = new InMemoryTaskManager();

            var found = manager.GetTaskById(Guid.NewGuid());

            Assert.Null(found);
        }
    }
}
