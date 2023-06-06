using Newtonsoft.Json;
using ScraperApp.Interfaces;
using ScraperApp.Models;
using ScraperApp.Scrapers;

namespace ScraperApp.Serializers;

internal class DriversSerializer : IDriversSerializer
{
    private readonly IDriversScraper _scraper;

    public DriversSerializer(IDriversScraper scraper)
    {
        _scraper = scraper;
    }

    public void SerializeDrivers(int year, bool includeIndexing)
    {
        IEnumerable<Driver> drivers = _scraper.ScrapeDrivers(year, false);

        string json = JsonConvert.SerializeObject(drivers, Formatting.Indented);

        string _jsonFolderPath = DirectoryHelper.GetJsonFolderPath();

        string path = Path.Combine(_jsonFolderPath, "drivers.json");

        File.WriteAllText(path, json);
    }
}
