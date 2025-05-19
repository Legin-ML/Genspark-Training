class Program
{

    static void CheckLargest(string n1, string n2) {
        if (double.TryParse(n1, out double num1) && double.TryParse(n2, out double num2))
        {
            double largest = num1 > num2 ? num1 : num2;
            Console.WriteLine($"The larger number is: {largest}");
        }
        else
        {
            Console.WriteLine("Please enter valid values");
        }
    }

    static void Main(string[] args)
    {
        Console.WriteLine("Largest calculator");
        Console.Write("Enter first number: ");
        string? number1 = Console.ReadLine();

        Console.Write("Enter second number: ");
        string? number2 = Console.ReadLine();

        CheckLargest(number1, number2);
    }
}