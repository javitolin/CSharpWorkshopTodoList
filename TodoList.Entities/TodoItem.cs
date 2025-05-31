namespace TodoList.Entities
{
    public record TodoItem
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;

    }

    public record TodoItemRequest
    {
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
    }
}
