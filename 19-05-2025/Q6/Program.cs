class Program
{
    static void printFrequencies(int[] input)
    {
        Dictionary<int, int> frequency = new Dictionary<int, int>();
        foreach (var i in input)
        {
            if (frequency.ContainsKey(i))
            {
                frequency[i] += 1;
            }
            else
            {
                frequency.Add(i, 1);
            }
        }

        foreach (int key in frequency.Keys)
        {
            System.Console.WriteLine($"{key} occurs {frequency[key]} times");
        }
    }
    static void Main(string[] args)
    {
        int[] input = {1, 2, 2, 3, 4, 4, 4};
        printFrequencies(input);
    }
}