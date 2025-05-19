using System;

class Program
{
    static void Main()
    {
        int count = CountDivisibleBy7(10);
        Console.WriteLine($"Total numbers divisible by 7: {count}");
    }

    static int CountDivisibleBy7(int totalInputs)
    {
        int count = 0;

        for (int i = 1; i <= totalInputs; i++)
        {
            int number = GetValidInteger(i);
            if (IsDivisibleBy7(number))
            {
                count++;
            }
        }

        return count;
    }

    static int GetValidInteger(int index)
    {
        while (true)
        {
            Console.Write($"Enter number {index}: ");
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int number))
            {
                return number;
            }

            Console.WriteLine("Invalid input. Please enter an integer.");
        }
    }

    static bool IsDivisibleBy7(int number)
    {
        return number % 7 == 0;
    }
}
