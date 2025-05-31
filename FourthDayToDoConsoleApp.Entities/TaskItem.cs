namespace FourthDayToDoConsoleApp.Entities
{
    public class TaskItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }


        public virtual string PrintSummary() // Virtual means it can be overridden in derived classes
        {
            return $"{Title} - {Description}";
        }

        public override string ToString()
        {
            return PrintSummary();
        }
    }

}
