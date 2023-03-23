using Newtonsoft.Json;
using ScraperApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace ScraperApp.Json;

internal static class JsonDeserializer
{
    internal static List<T> Deserialize<T>(string fileName)
    {
        using (StreamReader reader = new(
                $@"D:\C#\Formula1Project\Scraper\ScraperApp\Json\{fileName}.json"))
        {
            string json = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<T>>(json);
        }
    }
}
