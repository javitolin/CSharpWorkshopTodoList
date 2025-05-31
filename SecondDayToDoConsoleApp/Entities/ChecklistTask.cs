using System.Text;

namespace SecondDayToDoConsoleApp.Entities
{
    public class ChecklistTask : TaskItem
    {
        public List<string> Subtasks { get; set; } = new();
        public HashSet<int> CompletedIndexes { get; set; } = new();

        public override string PrintSummary()
        {
            StringBuilder summary = new StringBuilder();
            summary.AppendLine($"📋 {Title} ({CompletedIndexes.Count}/{Subtasks.Count} done)");
            for (int i = 0; i < Subtasks.Count; i++)
            {
                var isDone = CompletedIndexes.Contains(i) ? "[x]" : "[ ]";
                summary.AppendLine($"\t{isDone} {Subtasks[i]}");
            }

            return summary.ToString();
        }
    }
}
