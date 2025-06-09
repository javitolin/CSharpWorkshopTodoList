namespace FourthDayDemo.Entities
{
    public class TaskItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        public override string ToString()
        {
            return $"{Title} - {Description}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return this is null;

            return obj is TaskItem taskItem && taskItem.Id.Equals(Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Title, Description);
        }
    }

}
