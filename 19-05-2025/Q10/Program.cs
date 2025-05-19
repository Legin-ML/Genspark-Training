// 10) write a program that accepts a 9-element array representing a Sudoku row.

// Validates if the row:

// Has all numbers from 1 to 9.

// Has no duplicates.

// Displays if the row is valid or invalid.

class Program
{
    static bool IsValidSudokuRow(int[] row)
    {
        bool[] presentNumbers = new bool[10];
        int countUnique = 0;

        if (row.Length != 9)
        {
            return false;
        }

        foreach (var num in row)
        {
            if (num > 9 || num < 1)
            {
                return false;
            }
            if (presentNumbers[num])
            {
                return false;
            }
            else
            {
                presentNumbers[num] = true;
                countUnique += 1;
            }
        }

        return countUnique == 9;
    }

    static void ValidateRow(int[] row)
    {
        if (IsValidSudokuRow(row))
        {
            System.Console.WriteLine("Row is valid: " + string.Join(", ", row));
        }
        else
        {
            System.Console.WriteLine("Row is NOT valid: " + string.Join(", ", row));
        }
    }

    static void Main()
    {
        int[] validRow = { 5, 3, 1, 6, 7, 9, 2, 8, 4 };
        int[] duplicateRow = { 5, 3, 1, 6, 7, 9, 2, 3, 4 , 8, 4, 3, 2, 4, 3 };
        int[] duplicateRow2 = { 5, 3, 1, 6, 7, 9, 2, 4, 4 };
        int[] nullRow = { };
        ValidateRow(validRow);
        ValidateRow(duplicateRow);
        ValidateRow(duplicateRow2);
        ValidateRow(nullRow);
    }
}