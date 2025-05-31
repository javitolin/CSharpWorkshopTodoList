using TodoList.Entities;

namespace TodoList.IO.Interfaces
{
    public interface ITodoListDB
    {
        public IEnumerable<IEnumerable<TodoItem>> GetTodoListItems();
        public string AddTodoListItem(TodoItem item);
        public string UpdateTodoListItem(TodoItem item);
    }
}
