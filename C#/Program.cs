namespace MyProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            string? name = null;

            do
            {
                Console.WriteLine("What is your name?");
                name = Console.ReadLine();
            } while (name == null || name.Trim().Length == 0);

            Console.WriteLine("Hello, " + name + "!");
        }
    }
}
