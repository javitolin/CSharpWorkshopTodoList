namespace LinqExcercise
{
    public class FileSystemDemo
    {
        public void PrintAllFiles(string rootDirectory)
        {
            var files = Directory.GetFiles(rootDirectory, "*.*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                Console.WriteLine($"File: [{file}]");
            }
        }

        public void EnumerateAllFiles(string rootDirectory)
        {
            foreach(var file in Directory.EnumerateFiles(rootDirectory, "*.*", SearchOption.AllDirectories))
            {
                Console.WriteLine($"File: [{file}]");
            }
        }
    }
}
