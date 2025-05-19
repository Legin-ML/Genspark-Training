class Program
{
    static bool IsValidGroup(int[] group)
    {
        bool[] seen = new bool[10]; 
        foreach (int num in group)
        {
            if (num < 1 || num > 9 || seen[num])
                return false;
            seen[num] = true;
        }
        return true;
    }

    static bool ValidateRows(int[,] board)
    {
        for (int row = 0; row < 9; row++)
        {
            int[] currentRow = new int[9];
            for (int col = 0; col < 9; col++)
                currentRow[col] = board[row, col];

            if (!IsValidGroup(currentRow))
                return false;
        }
        return true;
    }

    static bool ValidateColumns(int[,] board)
    {
        for (int col = 0; col < 9; col++)
        {
            int[] currentCol = new int[9];
            for (int row = 0; row < 9; row++)
                currentCol[row] = board[row, col];

            if (!IsValidGroup(currentCol))
                return false;
        }
        return true;
    }

    static bool ValidateBoxes(int[,] board)
    {
        for (int boxRow = 0; boxRow < 3; boxRow++)
        {
            for (int boxCol = 0; boxCol < 3; boxCol++)
            {
                int[] box = new int[9];
                int index = 0;

                for (int row = boxRow * 3; row < boxRow * 3 + 3; row++)
                {
                    for (int col = boxCol * 3; col < boxCol * 3 + 3; col++)
                    {
                        box[index++] = board[row, col];
                    }
                }

                if (!IsValidGroup(box))
                    return false;
            }
        }
        return true;
    }

    static bool IsValidSudoku(int[,] board)
    {
        return ValidateRows(board) && ValidateColumns(board) && ValidateBoxes(board);
    }

    static void Main()
    {
        int[,] board = new int[9, 9]
        {
            { 5, 3, 4, 6, 7, 8, 9, 1, 2 },
            { 6, 7, 2, 1, 9, 5, 3, 4, 8 },
            { 1, 9, 8, 3, 4, 2, 5, 6, 7 },
            { 8, 5, 9, 7, 6, 1, 4, 2, 3 },
            { 4, 2, 6, 8, 5, 3, 7, 9, 1 },
            { 7, 1, 3, 9, 2, 4, 8, 5, 6 },
            { 9, 6, 1, 5, 3, 7, 2, 8, 4 },
            { 2, 8, 7, 4, 1, 9, 6, 3, 5 },
            { 3, 4, 5, 2, 8, 6, 1, 7, 9 }
        };

        Console.WriteLine(IsValidSudoku(board)
            ? "The Sudoku board is valid."
            : "The Sudoku board is invalid.");
    }
}
