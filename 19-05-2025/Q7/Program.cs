class Program
{
    static int[] RotateArray(int[] input)
    {
        int length = input.Length;
        int[] rotated = new int[length];
        for (int i = 1; i < length; i++)
        {
            rotated[i - 1] = input[i];
        }

        rotated[^1] = input[0];
        return rotated;
    }

    static void Main()
    {
        int[] input = { 10, 20, 30, 40, 50};
        Console.WriteLine("Input Array: " + string.Join(", ", input));
        int[] rotated = RotateArray(input);
        Console.WriteLine("Rotated Array: " + string.Join(", ", rotated));
    }
}