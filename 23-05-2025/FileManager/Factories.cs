public interface IFileFactory
{
    IReader GetReader();
    IWriter GetWriter();
}

// ---------------------------------------

public class FileIOFactory : IFileFactory
{
    public IReader GetReader()
    {
        return new FileReader();
    }

    public IWriter GetWriter()
    {
        return new FileWriter();
    }
}