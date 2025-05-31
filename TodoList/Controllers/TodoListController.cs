using Microsoft.AspNetCore.Mvc;
using TodoList.Entities;
using TodoList.IO.Interfaces;

namespace TodoList.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodoListController : ControllerBase
    {

        private readonly ILogger<TodoListController> _logger;
        private readonly ITodoListDB _todoListRetriever;

        public TodoListController(ILogger<TodoListController> logger, ITodoListDB todoListRetriever)
        {
            _logger = logger;
            _todoListRetriever = todoListRetriever;
        }

        [HttpGet]
        public IEnumerable<IEnumerable<TodoItem>> Get()
        {
            _logger.LogInformation("Getting TodoList items ids");
            return _todoListRetriever.GetTodoListItems();
        }

        [HttpPost]
        public string InsertTodoItem([FromBody] TodoItem item)
        {
            _logger.LogInformation("Adding TodoList item with id {id}", item.Id);
            return _todoListRetriever.AddTodoListItem(item);
        }

        [HttpPut]
        public string UpdateTodoItem([FromBody] TodoItem item)
        {
            _logger.LogInformation("Updating TodoList item with id {id}", item.Id);
            return _todoListRetriever.UpdateTodoListItem(item);
        }
    }
}
