public interface IReader
{
    string Read();
}

public interface IWriter
{
    void Write(string data);
}

// -------------------------------------

public class FileReader : IReader
{
    public string Read()
    {
        var fm = FileManager.GetInstance();
        var stream = fm.GetStream();
        stream.Seek(0, SeekOrigin.Begin); 

        var reader = fm.GetReader();
        return reader.ReadToEnd();
    }
}

public class FileWriter : IWriter
{
    public void Write(string data)
    {
        var writer = FileManager.GetInstance().GetWriter();
        writer.WriteLine(data);
        writer.Flush();
    }
}