using Newtonsoft.Json;
using ScraperApp.Interfaces;

namespace ScraperApp.Serializers;

internal class LinksSerializer : ILinksSerializer
{
    private readonly ILinksScraper _scraper;

    public LinksSerializer(ILinksScraper scraper)
    {
        _scraper = scraper;
    }

    public void SerializeLinks()
    {
        IEnumerable<string> links = _scraper.ScrapeLinks();

        string json = JsonConvert.SerializeObject(links, Formatting.Indented);

        string _jsonFolderPath = DirectoryHelper.GetJsonFolderPath();

        string path = Path.Combine(_jsonFolderPath, "links.json");

        File.WriteAllText(path, json);
    }
}
