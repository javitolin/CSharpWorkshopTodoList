using TodoList.Entities;
using TodoList.IO.Interfaces;

namespace TodoList.IO
{
    public class InMemoryTodoList : ITodoListDB
    {
        private readonly Dictionary<string, List<TodoItem>> _todoList = new();

        public string AddTodoListItem(TodoItem item)
        {
            if (!_todoList.ContainsKey(item.Id))
            {
                _todoList[item.Id] = new List<TodoItem>();
            }
            _todoList[item.Id].Add(item);

            return item.Id;
        }

        public IEnumerable<IEnumerable<TodoItem>> GetTodoListItems()
        {
            return _todoList.Values.Select(items => items.AsEnumerable()).ToList();
        }

        public string UpdateTodoListItem(TodoItem item)
        {
            if (_todoList.ContainsKey(item.Id))
            {
                var existingItem = _todoList[item.Id].FirstOrDefault(i => i.Id == item.Id);
                if (existingItem != null)
                {
                    _todoList[item.Id].Remove(existingItem);
                    _todoList[item.Id].Add(item);
                }
            }
            return item.Id;
        }
    }
}
