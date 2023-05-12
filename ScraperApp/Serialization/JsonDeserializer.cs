using Newtonsoft.Json;

namespace ScraperApp.Serialization;

internal static class JsonDeserializer
{
    internal static List<T> DeserializeCollection<T>(string fileName)
    {
        string projectFolder = DirectoryHelper.GetProjectFolderPath();

        string filePath = Path.Combine(projectFolder, "Json", $"{fileName}.json");

        using (StreamReader reader = new(filePath))
        {
            string json = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<T>>(json);
        }
    }
}
