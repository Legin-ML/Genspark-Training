using System;

class Program
{
    static string Encrypt(string message, int shift)
    {
        message = message.ToLower();
        char[] encrypted = new char[message.Length];

        for (int i = 0; i < message.Length; i++)
        {
            char c = message[i];
            encrypted[i] = (char)('a' + (c - 'a' + shift) % 26);
        }

        return new string(encrypted);
    }

    static string Decrypt(string encrypted, int shift)
    {
        encrypted = encrypted.ToLower();
        char[] decrypted = new char[encrypted.Length];

        for (int i = 0; i < encrypted.Length; i++)
        {
            char c = encrypted[i];
            decrypted[i] = (char)('a' + (c - 'a' - shift + 26) % 26);
        }

        return new string(decrypted);
    }

    static void RunTest(string input, int shift)
    {
        string encrypted = Encrypt(input, shift);
        string decrypted = Decrypt(encrypted, shift);

        Console.WriteLine($"Input     : {input}");
        Console.WriteLine($"Encrypted : {encrypted}");
        Console.WriteLine($"Decrypted : {decrypted}");
        Console.WriteLine(new string('-', 30));
    }

    static void Main()
    {
        RunTest("hello", 3);
        RunTest("world", 3);
        RunTest("xyz", 3);
        RunTest("apple", 1);
    }
}
