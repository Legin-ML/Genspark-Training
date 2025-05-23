
class Program
{
    static void Main()
    {
        IFileFactory fileFactory = new FileIOFactory();
        IWriter writer = fileFactory.GetWriter();
        IReader reader = fileFactory.GetReader();

        writer.Write("Hello from the file!");
        writer.Write("Another line.");

        Console.WriteLine("------------------Written to file.---------------------");

        string content = reader.Read();
        Console.WriteLine("Read from file:\n" + content);

        FileManager.GetInstance().Close();
    }
}