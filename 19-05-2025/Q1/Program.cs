class Program {
    static void Main(string[] args) {

        Console.Write("Enter your name: ");
        string? name = Console.ReadLine();
        
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Hello Stranger!");
        }
        else
        {
            Console.WriteLine($"Hello {name}!");
        }
    }
}