using System;

class Program
{
    static void Main()
    {
        const string validUsername = "Admin";
        const string validPassword = "pass";
        const int maxAttempts = 3;

        if (AuthenticateUser(validUsername, validPassword, maxAttempts))
        {
            Console.WriteLine("Login successful! Welcome, Admin.");
        }
        else
        {
            Console.WriteLine("Invalid attempts for 3 times. Exiting....");
        }
    }

    static bool AuthenticateUser(string validUsername, string validPassword, int maxAttempts)
    {
        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            string username = PromptInput("Enter username: ");
            string password = PromptInput("Enter password: ");

            if (IsValidCredentials(username, password, validUsername, validPassword))
            {
                return true;
            }

            Console.WriteLine($"Invalid credentials. Attempt {attempt} of {maxAttempts}.");
        }

        return false;
    }

    static string PromptInput(string message)
    {
        Console.Write(message);
        return Console.ReadLine() ?? string.Empty;
    }

    static bool IsValidCredentials(string username, string password, string validUsername, string validPassword)
    {
        return username == validUsername && password == validPassword;
    }
}
