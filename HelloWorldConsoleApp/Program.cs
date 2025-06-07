namespace HelloWorldConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter your name: ");
            string? usersName = Console.ReadLine(); 
            if (string.IsNullOrWhiteSpace(usersName))
            {
                 Console.WriteLine("Name cannot be empty. Please enter a valid name.");
                 return;
            }

            Console.Write("Enter your age: ");
            string? usersAge = Console.ReadLine();
            if (!int.TryParse(usersAge, out int age))
            {     
                Console.WriteLine("Invalid age input. Please enter a number.");
                return;
            }

            var person = new Person(usersName, age);

            Console.WriteLine(person);
        }
    }
}
