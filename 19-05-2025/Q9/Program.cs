// 9) Write a program that:

// Has a predefined secret word (e.g., "GAME").

// Accepts user input as a 4-letter word guess.

// Compares the guess to the secret word and outputs:

// X Bulls: number of letters in the correct position.

// Y Cows: number of correct letters in the wrong position.

// Continues until the user gets 4 Bulls (i.e., correct guess).

// Displays the number of attempts.

// Bull = Correct letter in correct position.

// Cow = Correct letter in wrong position.

// Secret Word	User Guess	Output	Explanation
// GAME	GAME	4 Bulls, 0 Cows	Exact match
// GAME	MAGE	1 Bull, 3 Cows	A in correct position, MGE misplaced
// GAME	GUYS	1 Bull, 0 Cows	G in correct place, rest wrong
// GAME	AMGE	2 Bulls, 2 Cows	A, E right; M, G misplaced
// NOTE	TONE	2 Bulls, 2 Cows	O, E right; T, N misplaced

class Program
{
    const string WORD = "GAME";

    static int CheckCows(string input)
    {

        int cows = 0;

        bool[] visitedInput = new bool[WORD.Length];
        bool[] visitedWord = new bool[WORD.Length];

        for (int i = 0; i < WORD.Length; i++)
        {
            if (input[i] == WORD[i])
            {
                visitedInput[i] = true;
                visitedWord[i] = true;
            }
        }

        for (int i = 0; i < WORD.Length; i++)
        {
            if (visitedInput[i]) { continue; }

            for (int j = 0; j < WORD.Length; j++)
            {
                if (!visitedWord[j] && input[i] == WORD[j])
                {
                    cows += 1;
                    visitedWord[j] = true;
                    break;
                }
            }
        }

        return cows;

    }

    static int CheckBulls(string input)
    {
        int bulls = 0;
        for (int i = 0; i < WORD.Length; i++)
        {
            if (input[i] == WORD[i])
            {
                bulls += 1;
            }
        }

        return bulls;

    }

    static bool IsValid(string? input)
    {
        return !string.IsNullOrEmpty(input) && input?.Length == WORD.Length;
    }

    static string? GetInput()
    {
        Console.Write("Enter your guess: ");
        return Console.ReadLine();
    }
    static void GameLoop()
    {
        Console.WriteLine($"The word has {WORD.Length} letters. Good Luck!");

        int attempts = 1;

        while (true)
        {
            string? input = GetInput();

            if (IsValid(input))
            {
                string fixedInput = input.ToUpper();
                int bulls = CheckBulls(fixedInput);

                if (bulls == WORD.Length)
                {
                    Console.WriteLine($"Congratulations, You guessed correctly in {attempts} Attempt(s)!");
                    break;
                }
                int cows = CheckCows(fixedInput);

                Console.WriteLine($"You guess has {bulls} bulls and {cows} cows");

                attempts += 1;

            }
            else
            {
                System.Console.WriteLine("Please enter a valid guess");
            }
        }
    }
    static void Main()
    {
        Console.WriteLine("==========Guess The Word==========");

        GameLoop();

    }
}