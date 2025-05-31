using Moq;
using System.IO.Abstractions;
using System.Text.Json;
using ThirdDayToDoConsoleApp;
using ThirdDayToDoConsoleApp.Entities;

namespace ToDoConsoleApp.Tests
{

    public class FileSystemFixture : IDisposable
    {
        public IFileSystem FileManager { get; private set; }
        private string fileContent = "";

        public FileSystemFixture()
        {
            var fileMock = new Mock<IFile>();
            fileMock.Setup(f => f.Exists(It.IsAny<string>())).Returns(true);
            fileMock.Setup(f => f.ReadAllText(It.IsAny<string>())).Returns(() => fileContent);
            fileMock.Setup(f => f.WriteAllText(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((path, text) => fileContent = text);


            var fileSystemMock = new Mock<IFileSystem>();
            fileSystemMock.Setup(fs => fs.File).Returns(fileMock.Object);

            FileManager = fileSystemMock.Object;
        }

        public void Dispose()
        {
            fileContent = "";
        }
    }

    public class FileSystemTaskManagerTests : IClassFixture<FileSystemFixture>, IDisposable
    {
        private FileSystemFixture _fixture;
        private IFileSystem _fileSystem;

        public FileSystemTaskManagerTests(FileSystemFixture fixture)
        {
            _fileSystem = fixture.FileManager;
            _fixture = fixture;
        }

        [Fact]
        public async Task AddTask_FirstTaskAdded_TaskAdded()
        {
            var manager = new FileSystemTaskManager(_fileSystem);

            var task = new TaskItem { Id = Guid.NewGuid(), Title = "Test Task" };
            var toWrite = JsonSerializer.Serialize(new List<TaskItem> { task }, new JsonSerializerOptions { WriteIndented = true });

            await manager.AddTask(task);

            var allTasks = (await manager.GetAllTasks()).ToList();
            Assert.Single(allTasks);
            Assert.Equal(task.Id, allTasks[0].Id);
            Assert.Equal("Test Task", allTasks[0].Title);
        }

        [Fact]
        public async Task RemoveTask_TaskExists_TaskRemoved()
        {
            var manager = new FileSystemTaskManager(_fileSystem);
            var task = new TaskItem { Id = Guid.NewGuid(), Title = "Task to Remove" };
            await manager.AddTask(task);

            var result = await manager.RemoveTask(task.Id);

            Assert.True(result);
            Assert.Empty(await manager.GetAllTasks());
        }

        [Fact]
        public async Task RemoveTask_TaskDoesntExists_ReturnsFalse()
        {
            var manager = new FileSystemTaskManager(_fileSystem);

            var result = await manager.RemoveTask(Guid.NewGuid());

            Assert.False(result);
        }

        [Fact]
        public async Task UpdateTask_TaskExists_TaskUpdated()
        {
            var manager = new FileSystemTaskManager(_fileSystem);
            var task = new TaskItem { Id = Guid.NewGuid(), Title = "Old Title" };
            await manager.AddTask(task);

            var updatedTask = new TaskItem { Id = task.Id, Title = "New Title" };
            var result = await manager.UpdateTask(task.Id, updatedTask);

            Assert.True(result);
            var fetched = await manager.GetTaskById(task.Id);
            Assert.NotNull(fetched);
            Assert.Equal("New Title", fetched.Title);
        }

        [Fact]
        public async Task UpdateTask_TaskDoesntExist_ReturnsFalse()
        {
            var manager = new FileSystemTaskManager(_fileSystem);
            var updatedTask = new TaskItem { Id = Guid.NewGuid(), Title = "Doesn't Matter" };

            var result = await manager.UpdateTask(updatedTask.Id, updatedTask);

            Assert.False(result);
        }

        [Fact]
        public async Task GetAllTasks_TasksExists_ReturnsAllTasks()
        {
            var manager = new FileSystemTaskManager(_fileSystem)    ;
            var task1 = new TaskItem { Id = Guid.NewGuid(), Title = "Task 1" };
            var task2 = new TaskItem { Id = Guid.NewGuid(), Title = "Task 2" };
            await manager.AddTask(task1);
            await manager.AddTask(task2);

            var allTasks = (await manager.GetAllTasks()).ToList();

            Assert.Equal(2, allTasks.Count);
            Assert.Contains(allTasks, t => t.Id == task1.Id);
            Assert.Contains(allTasks, t => t.Id == task2.Id);
        }

        [Fact]
        public async Task GetTaskById_TaskExists_ReturnsTask()
        {
            var manager = new FileSystemTaskManager(_fileSystem)    ;
            var task = new TaskItem { Id = Guid.NewGuid(), Title = "Find Me" };
            await manager.AddTask(task);

            var found = await manager.GetTaskById(task.Id);

            Assert.NotNull(found);
            Assert.Equal(task.Id, found.Id);
        }

        [Fact]
        public async Task GetTaskById_TaskNotFound_ReturnsNull()
        {
            var manager = new FileSystemTaskManager(_fileSystem);

            var found = await manager.GetTaskById(Guid.NewGuid());

            Assert.Null(found);
        }

        public void Dispose()
        {
            _fixture.Dispose();
        }
    }
}
