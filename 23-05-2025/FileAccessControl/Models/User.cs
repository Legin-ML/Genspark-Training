using System;

public class User
{
    private string Username { get; set; }

    private string Clearance { get; set; }

    public User()
    {
        GetDetails();
    }
    public User(string name, string clearance)
    {
        Username = name;
        Clearance = clearance;
    }

    public string GetUsername()
    {
        return Username;
    }

    public string GetClearance()
    {
        return Clearance;
    }

    public void GetDetails()
    {
        Username = GetValidInput("Enter username: ");
        Clearance = GetValidInput("Enter clearance level: ");
    }

    private string GetValidInput(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(input))
            {
                return input.Trim();
            }

            Console.WriteLine("Input cannot be empty. Please try again.");
        }
    }
}
