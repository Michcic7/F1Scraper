using Newtonsoft.Json;
using ScraperApp.Models;
using ScraperApp.Scrapers;
using System.Reflection.PortableExecutable;

namespace ScraperApp;

internal class Serializer
{
    internal void SerializeLinks()
    {
        LinksScraper scraper = new();
        List<string> links = new();
        List<string> linksToAdd = new();

        for (int year = 1950; year <= 2023; year++)
        {
            linksToAdd = scraper.ScrapeLinks(year);
            links.AddRange(linksToAdd);
        }

        string json = JsonConvert.SerializeObject(links, Formatting.Indented);

        File.WriteAllText(@"D:\C#\Formula1Project\Scraper\ScraperApp\Json\links.json", json);
    }

    internal void SerializeCircuits()
    {
        CircuitScraper scraper = new();

        List<string> links = GetLinks();

        List<Circuit> circuits = scraper.ScrapeCircuits(links);

        string json = JsonConvert.SerializeObject(circuits, Formatting.Indented);

        File.WriteAllText(@"D:\C#\Formula1Project\Scraper\ScraperApp\Json\circuits.json", json);
    }

    internal void SerializeTeams()
    {
        TeamScraper scraper = new();

        List<string> links = GetLinks();

        List<Team> teams = scraper.ScrapeTeams(links);

        string json = JsonConvert.SerializeObject(teams, Formatting.Indented);

        File.WriteAllText(@"D:\C#\Formula1Project\Scraper\ScraperApp\Json\teams.json", json);
    }

    internal void SerializeDrivers()
    {
        DriverScraper scraper = new();

        // drivers are scraped from driver standings, not race results - no argument
        List<Driver> drivers = scraper.ScrapeDrivers();

        string json = JsonConvert.SerializeObject(drivers, Formatting.Indented);

        File.WriteAllText(@"D:\C#\Formula1Project\Scraper\ScraperApp\Json\drivers.json", json);
    }

    private List<string> GetLinks()
    {
        using (StreamReader reader = new(
            @"D:\C#\Formula1Project\Scraper\ScraperApp\Json\links.json"))
        {
            string json = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<string>>(json);
        }
    }
}
