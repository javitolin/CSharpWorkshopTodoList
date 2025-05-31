namespace FourthDayToDoConsoleApp.Entities
{
    public class MeetingTask : TaskItem
    {
        public string Location { get; set; } = string.Empty;
        public string[] Attendees { get; set; } = Array.Empty<string>();

        public override string PrintSummary()
        {
            return $"📅 {Title} at {Location} with {string.Join(", ", Attendees)}";
        }
    }
}
