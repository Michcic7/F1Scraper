using Newtonsoft.Json;
using ScraperApp.Models;
using ScraperApp.Scrapers;

namespace ScraperApp;

internal static class Serializer
{
    public static void SerializeDriverModel(List<Team> teams)
    {
        List<Driver> drivers = new();
        List<Driver> driversToAdd = new();
        DriverScraper scrapper = new();

        for (int year = 1950; year <= 2022; year++)
        {
            driversToAdd = scrapper.ScrapeDrivers(year, teams);

            drivers.AddRange(driversToAdd);
        }

        IEnumerable<Driver> distinctDrivers = drivers.DistinctBy(d => d.FirstName + " " + d.LastName);

        var json = JsonConvert.SerializeObject(distinctDrivers, Formatting.Indented);

        File.WriteAllText(@"D:\C#\Formula1Project\Scraper\Scraper\Json\drivers.json", json);
    }

    public static void SerializeCircuitModel()
    {
        List<Circuit> circuits = new();
        List<Circuit> circuitsToAdd = new();
        CircuitScraper scrapper = new();

        for (int year = 1950; year <= 2022; year++)
        {
            circuitsToAdd = scrapper.ScrapeCircuits(year);

            circuits.AddRange(circuitsToAdd);
        }

        var distinctCurcuits = circuits.DistinctBy(c => c.Name);

        var json = JsonConvert.SerializeObject(distinctCurcuits, Formatting.Indented);

        File.WriteAllText(@"D:\C#\Formula1Project\Scraper\Scraper\Json\circuits.json", json);
    }

    public static void SerializeTeamModel()
    {
        List<Team> teams = new();
        List<Team> teamsToAdd = new();
        TeamScraper scrapper = new();

        for (int year = 1958; year <= 2022; year++)
        {
            teamsToAdd = scrapper.ScrapeTeams(year);

            teams.AddRange(teamsToAdd);
        }

        var distinctTeams = teams.DistinctBy(t => t.Name);

        var json = JsonConvert.SerializeObject(distinctTeams, Formatting.Indented);

        File.WriteAllText(@"D:\C#\Formula1Project\Scraper\Scraper\Json\teams.json", json);
    }

    public static void SerializeRaceResultModel()
    {
        List<RaceResult> raceResults = new();
        List<RaceResult> raceResultsToAdd = new();
        RaceResultScraper scrapper = new();

        for (int year = 1958; year <= 2022; year++)
        {
            raceResultsToAdd = scrapper.ScrapeRaceResults(year);

            raceResults.AddRange(raceResultsToAdd);
        }

        var distinctRaceResults = raceResults.DistinctBy(t => t.Date);

        var json = JsonConvert.SerializeObject(distinctRaceResults, Formatting.Indented);

        File.WriteAllText(@"D:\C#\Formula1Project\Scraper\Scraper\Json\raceresults.json", json);
    }

    //public static void SerializeDrivers()
    //{
    //    List<Driver> drivers = new();
    //    List<Driver> driversToAdd = new();
    //    DriverStandingsScraper scrapper = new();

    //    for (int year = 1950; year <= 2022; year++)
    //    {
    //        driversToAdd = scrapper.GetDriverStandings(year);

    //        drivers.AddRange(driversToAdd);
    //    }

    //    var json = JsonConvert.SerializeObject(drivers, Formatting.Indented);

    //    File.WriteAllText(@"D:\C#\F1WebAPI\API\Data\Json\drivers.json", json);
    //}

    //public static void SerializeTeams()
    //{
    //    List<Team> teams = new();
    //    List<Team> teamsToAdd = new();
    //    TeamStandingsScraper scrapper = new();

    //    for (int year = 1958; year <= 2022; year++)
    //    {
    //        teamsToAdd = scrapper.GetTeamStandings(year);

    //        teams.AddRange(teamsToAdd);
    //    }

    //    var json = JsonConvert.SerializeObject(teams, Formatting.Indented);

    //    File.WriteAllText(@"D:\C#\F1WebAPI\API\Data\Json\teams.json", json);
    //}

    //public static void SerializeRaces()
    //{
    //    List<Circuit> races = new();
    //    List<Circuit> racesToAdd = new();
    //    RaceResultsScraper scrapper = new();

    //    for (int year = 1950; year <= 2022; year++)
    //    {
    //        racesToAdd = scrapper.GetRacesResults(year);

    //        races.AddRange(racesToAdd);
    //    }

    //    var json = JsonConvert.SerializeObject(races, Formatting.Indented);

    //    File.WriteAllText(@"D:\C#\F1WebAPI\API\Data\Json\races.json", json);
    //}
}
