namespace ToDoWebApp.Entities
{
    public class DeadlineTask : TaskItem
    {
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; } = false;

        public override string PrintSummary()
        {
            var status = IsCompleted ? "✔️" : (DueDate < DateTime.Now ? "⚠️ Overdue" : "⏳");
            return $"{status} {Title} (due {DueDate:dd/MM/yyyy})";
        }
    }
}
