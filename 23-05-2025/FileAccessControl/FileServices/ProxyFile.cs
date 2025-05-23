public class ProxyFile : IFile
{
    private File _file;
    private User _user;
    public ProxyFile(User user)
    {
        _user = user;
        _file = new File();
    }
    public string Read()
    {
        switch (_user.GetClearance().ToLower())
        {
            case "admin":
                return _file.Read();
            case "user":
                return "[Welcome User]\nThe file was last modified 1 minute ago";
            case "guest":
                return "[Access Denied]";
            default:
                return "UNKNOWN ACCESS LEVEL. THIS HAS BEEN LOGGED. EXITING......";
        }
    }
}