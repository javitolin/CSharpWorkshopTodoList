using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.IO.Abstractions;
using ToDoWebApp;
using Xunit;

namespace ToDoWebAppTests.Controllers
{
    public class  CustomWebApplicationFactory:WebApplicationFactory<Program>
    {
        private string fileContent = "";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var fileMock = new Mock<IFile>();
            fileMock.Setup(f => f.Exists(It.IsAny<string>())).Returns(true);
            fileMock.Setup(f => f.ReadAllText(It.IsAny<string>())).Returns(() => fileContent);
            fileMock.Setup(f => f.WriteAllText(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((path, text) => fileContent = text);


            var fileSystemMock = new Mock<IFileSystem>();
            fileSystemMock.Setup(fs => fs.File).Returns(fileMock.Object);

            builder.ConfigureServices(services =>
            {
                // Remove the real service if it exists
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IFileSystem));

                if (descriptor != null)
                    services.Remove(descriptor);

                // Add your mocked version
                services.AddSingleton(fileSystemMock.Object);
            });
        }


    }

    public class TodoApiTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public TodoApiTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetTasks_NoTasksExist_ReturnsOk()
        {
            var response = await _client.GetAsync("/Tasks");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            Assert.NotNull(content);
        }
    }
}