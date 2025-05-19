class Program
{

    static string? GetInput(string query) {
        Console.Write($"{query}");
        return Console.ReadLine();
    }

    static void ComputeValue(string? num1, string? op, string? num2)
    {
        if (double.TryParse(num1, out double opd1) && double.TryParse(num2, out double opd2))
        {
            switch (op)
            {
                case "+":
                    Console.WriteLine($"The result is: {opd1 + opd2}");
                    break;
                case "-":
                    Console.WriteLine($"The result is: {opd1 - opd2}");
                    break;
                case "*":
                    Console.WriteLine($"The result is: {opd1 * opd2}");
                    break;
                case "/":
                    if (opd2 == 0)
                    {
                        Console.WriteLine("The denominator cannot be zero!");
                        break;
                    }
                    Console.WriteLine($"The result is: {opd1 / opd2}");
                    break;
                default:
                    Console.WriteLine("Invaild operator");
                    break;
            }
        }else
        Console.WriteLine("Enter valid values");
    }
    static void Main(string[] args)
    {
        Console.WriteLine("============CALCULATOR============");
        string? num1 = GetInput("Enter the first number:");
        string? op = GetInput("Enter the operation");
        string? num2 = GetInput("Enter the second number:");

        ComputeValue(num1, op, num2);
    }
}