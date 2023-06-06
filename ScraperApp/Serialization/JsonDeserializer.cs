using Newtonsoft.Json;

namespace ScraperApp.Serialization;

internal static class JsonDeserializer
{
    internal static List<T> DeserializeCollection<T>(string fileName)
    {
        string _jsonFolderPath = DirectoryHelper.GetJsonFolderPath();

        string filePath = Path.Combine(_jsonFolderPath, $"{fileName}.json");

        using (StreamReader reader = new(filePath))
        {
            string json = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<T>>(json);
        }
    }
}
