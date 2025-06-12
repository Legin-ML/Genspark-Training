namespace TrueFeedback.Security;

public class PasswordUtils
{
    public static string HashPassword(string password)
    {
        string encrypted = BCrypt.Net.BCrypt.HashPassword(password);
        return encrypted;
    }

    public static bool VerifyPassword(string password, string encrypted)
    {
        return BCrypt.Net.BCrypt.Verify(password, encrypted);
    }
}