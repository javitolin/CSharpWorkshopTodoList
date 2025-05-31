namespace ToDoWebApp.Entities
{
    public class TaskItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }


        public virtual string PrintSummary()
        {
            return $"{Title} - {Description}";
        }

        public override string ToString()
        {
            return PrintSummary();
        }
    }

}
