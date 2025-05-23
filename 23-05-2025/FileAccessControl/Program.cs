using System.Runtime.InteropServices;

class Program
{
    static void Main()
    {
        User user = new User();
        ProxyFile file = new ProxyFile(user);

        Console.WriteLine(file.Read());
    }
}