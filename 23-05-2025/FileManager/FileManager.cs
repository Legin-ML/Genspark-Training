class FileManager
{
    private static FileManager instance = null;

    private static readonly object lockThread = new object();
    private FileStream fileStream;
    private StreamWriter writer;
    private StreamReader reader;

    private string filePath = "test.txt";

    private FileManager()
    {
        fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        writer = new StreamWriter(fileStream);
        reader = new StreamReader(fileStream);
    }

    public static FileManager GetInstance()
    {
        lock (lockThread)
        {
            if (instance == null)
            {
                instance = new FileManager();
            }
            return instance;
        }
    }

    public StreamWriter GetWriter()
    {
        return writer;
    }

    public StreamReader GetReader()
    {
        return reader;
    }

    public FileStream GetStream()
    {
        return fileStream;
    }

    public void Close()
    {
        fileStream.Close();
        // writer.Close();
        // reader.Close();
    }
}