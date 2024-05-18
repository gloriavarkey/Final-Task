using System.IO;

public class FileHandler
{
    public static void SaveToFile(string filePath, string content)
    {
        File.WriteAllText(filePath, content);
    }

    public static string ReadFromFile(string filePath)
    {
        return File.ReadAllText(filePath);
    }
}
