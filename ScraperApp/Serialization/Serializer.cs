using Newtonsoft.Json;
using ScraperApp.Models;
using ScraperApp.Scrapers;

namespace ScraperApp.Serialization;

internal class Serializer
{
    // Path to 'ScraperApp\Json'.
    private readonly string basePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + @"\Json";

    internal void SerializeLinks()
    {
        LinksScraper scraper = new();

        List<string> links = scraper.ScrapeLinks(DateTime.Now.Year);

        string json = JsonConvert.SerializeObject(links, Formatting.Indented);

        string path = Path.Combine(basePath, "links.json");
        File.WriteAllText(path, json);
    }

    internal void SerializeCircuits()
    {
        List<string> links = GetLinks();

        CircuitScraper scraper = new();

        List<Circuit> circuits = scraper.ScrapeCircuits(links);

        string json = JsonConvert.SerializeObject(circuits, Formatting.Indented);

        string path = Path.Combine(basePath, "circuits.json");
        File.WriteAllText(path, json);
    }

    internal void SerializeTeams()
    {
        List<string> links = GetLinks();

        TeamScraper scraper = new();

        List<Team> teams = scraper.ScrapeTeams(links);

        string json = JsonConvert.SerializeObject(teams, Formatting.Indented);

        string path = Path.Combine(basePath, "teams.json");
        File.WriteAllText(path, json);
    }

    internal void SerializeDrivers()
    {
        List<string> links = GetLinks();

        DriverScraper scraper = new();

        List<Driver> drivers = scraper.ScrapeDrivers(links);

        string json = JsonConvert.SerializeObject(drivers, Formatting.Indented);

        string path = Path.Combine(basePath, "drivers.json");
        File.WriteAllText(path, json);
    }

    internal void SerializeTeamStandings(int startYear, int endYear)
    {
        TeamStandingScraper scraper = new();

        List<TeamStanding> teamStandings = scraper.ScrapeTeamStandings(startYear, endYear);

        string json = JsonConvert.SerializeObject(teamStandings, Formatting.Indented);

        string path = Path.Combine(basePath, "teamStandings.json");
        File.WriteAllText(path, json);
    }

    internal void SerializeDriverStandings(int startYear, int endYear)
    {
        DriverStandingScraper scraper = new();

        List<DriverStanding> driverStandings = scraper.ScrapeDriverStandings(startYear, endYear);

        string json = JsonConvert.SerializeObject(driverStandings, Formatting.Indented);

        string path = Path.Combine(basePath, "driverStandings.json");
        File.WriteAllText(path, json);
    }

    internal void SerializeRaceResults()
    {
        List<string> links = GetLinks();

        RaceResultScraper scraper = new();

        List<RaceResult> raceResults = scraper.ScrapeRaceResults(links);

        string json = JsonConvert.SerializeObject(raceResults, Formatting.Indented);

        string path = Path.Combine(basePath, "raceResults.json");
        File.WriteAllText(path, json);
    }

    private List<string> GetLinks()
    {
        string path = Path.Combine(basePath, "links.json");

        using (StreamReader reader = new(path))
        {
            string json = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<string>>(json);
        }
    }
}
