using Newtonsoft.Json;
using ScraperApp.Models;
using ScraperApp.Scrapers;

namespace ScraperApp.Serialization;

internal class Serializer
{
    private readonly string _jsonFolder = Path.Combine(
        DirectoryHelper.GetProjectFolderPath(), "Json");

    internal void SerializeLinks()
    {
        LinksScraper scraper = new();

        List<string> links = scraper.ScrapeLinks(DateTime.Now.Year);

        string json = JsonConvert.SerializeObject(links, Formatting.Indented);

        string path = Path.Combine(_jsonFolder, "links.json");
        File.WriteAllText(path, json);
    }

    internal void SerializeCircuits()
    {
        CircuitScraper scraper = new();

        List<Circuit> circuits = scraper.ScrapeCircuits();

        string json = JsonConvert.SerializeObject(circuits, Formatting.Indented);

        string path = Path.Combine(_jsonFolder, "circuits.json");
        File.WriteAllText(path, json);
    }

    internal void SerializeTeams()
    {
        TeamScraper scraper = new();

        List<Team> teams = scraper.ScrapeTeams();

        string json = JsonConvert.SerializeObject(teams, Formatting.Indented);

        string path = Path.Combine(_jsonFolder, "teams.json");
        File.WriteAllText(path, json);
    }

    internal void SerializeDrivers()
    {
        DriverScraper scraper = new();

        List<Driver> drivers = scraper.ScrapeDrivers();

        string json = JsonConvert.SerializeObject(drivers, Formatting.Indented);

        string path = Path.Combine(_jsonFolder, "drivers.json");
        File.WriteAllText(path, json);
    }

    internal void SerializeTeamStandings(int startYear, int endYear)
    {
        TeamStandingScraper scraper = new();

        List<TeamStanding> teamStandings = scraper.ScrapeTeamStandings(startYear, endYear);

        string json = JsonConvert.SerializeObject(teamStandings, Formatting.Indented);

        string path = Path.Combine(_jsonFolder, "teamStandings.json");
        File.WriteAllText(path, json);
    }

    internal void SerializeDriverStandings(int startYear, int endYear)
    {
        DriverStandingScraper scraper = new();

        List<DriverStanding> driverStandings = scraper.ScrapeDriverStandings(startYear, endYear);

        string json = JsonConvert.SerializeObject(driverStandings, Formatting.Indented);

        string path = Path.Combine(_jsonFolder, "driverStandings.json");
        File.WriteAllText(path, json);
    }

    internal void SerializeRaceResults(int startYear, int endYear)
    {
        RaceResultScraper scraper = new();

        List<RaceResult> raceResults = scraper.ScrapeRaceResults(startYear, endYear);

        string json = JsonConvert.SerializeObject(raceResults, Formatting.Indented);

        string path = Path.Combine(_jsonFolder, "raceResults.json");
        File.WriteAllText(path, json);
    }
}
