using System;

class Program
{
    static int[] CombineArrays(int[] arr1, int[] arr2)
    {
        int[] result = new int[arr1.Length + arr2.Length];

        for (int i = 0; i < arr1.Length; i++)
        {
            result[i] = arr1[i];
        }

        for (int i = 0; i < arr2.Length; i++)
        {
            result[arr1.Length + i] = arr2[i];
        }

        return result;
    }

    static void Main()
    {
        int[] arr1 = { 1, 3, 5 };
        int[] arr2 = { 2, 4, 6 };

        int[] combined = CombineArrays(arr1, arr2);

        Console.WriteLine("Combined array: " + string.Join(", ", combined));
    }
}
