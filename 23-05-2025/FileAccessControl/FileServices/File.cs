using System;
using System.IO;

public class File : IFile
{
    public string Read()
    {
        string output = "[Access Granted]\nWelcome Admin\n>Login Attempts\n";

        try
        {
            string fileContent = System.IO.File.ReadAllText("sensitive.txt");
            output += fileContent;
        }
        catch (FileNotFoundException)
        {
            output += "\n[Error] File 'sensitive.txt' not found.";
        }
        catch (Exception ex)
        {
            output += $"\n[Error] Unable to read file: {ex.Message}";
        }

        return output;
    }

    
}
