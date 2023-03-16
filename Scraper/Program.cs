using HtmlAgilityPack;
using Newtonsoft.Json;
using ScraperApp;
using ScraperApp.Models;
using ScraperApp.Scrapers;
using System.Net;
using System.Text;
using System;
using System.Reflection;

int index = 1;

TeamScraper teamScraper = new();
List<Team> _teams = new();

List<Team> teamsToAdd = new();
for (int year = 1958; year <= 2022; year++)
{
    teamsToAdd = teamScraper.ScrapeTeams(year);
    Console.WriteLine(_teams.Count);
    _teams.AddRange(teamsToAdd);
}
_teams = _teams.DistinctBy(t => t.Name).ToList();
var teamsJson = JsonConvert.SerializeObject(_teams, Formatting.Indented);
File.WriteAllText(@"D:\C#\Formula1Project\Scraper\Scraper\Json\teams.json", teamsJson);

List<Driver> drivers = ScrapeD(1990, _teams);

drivers = drivers.DistinctBy(d => d.FirstName + " " + d.LastName).ToList();
var driversJson = JsonConvert.SerializeObject(drivers, Formatting.Indented);
File.WriteAllText(@"D:\C#\Formula1Project\Scraper\Scraper\Json\drivers.json", driversJson);



List<Driver> ScrapeD(int year, List<Team> teams)
{
    HtmlWeb web = new();
    web.OverrideEncoding = Encoding.UTF8;
    HtmlDocument document = web.Load($"https://www.formula1.com/en/results.html/{year}/drivers.html");

    List<Driver> drivers = new();

    foreach (var row in document.DocumentNode.SelectNodes(
        "//table[@class='resultsarchive-table']/tbody/tr"))
    {
        Driver driver = new();

        try
        {
            var firstName = row.SelectSingleNode(
                    "./td/a/span[@class='hide-for-tablet']").InnerText;
            var lastName = row.SelectSingleNode(
                "./td/a/span[@class='hide-for-mobile']").InnerText;
            var nationality = row.SelectSingleNode(
                "./td[@class='dark semi-bold uppercase']").InnerText;
            var teamString = row.SelectSingleNode(
                "./td/a[@class='grey semi-bold uppercase ArchiveLink']").InnerText;

            driver.Id = index++;
            driver.FirstName = WebUtility.HtmlDecode(firstName);
            driver.LastName = WebUtility.HtmlDecode(lastName);
            driver.Nationality = nationality;
            driver.Team = new List<Team>();

            driver.Team.AddRange(teams);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        drivers.Add(driver);
    }

    return drivers;
}




void Method2()
{
    TeamScraper teamScraper = new();
    DriverScraper driverScraper = new();
    List<Team> teamsToAdd = new();
    List<Driver> drivers = new();
    List<Driver> driversToAdd = new();

    List<Team> _teams = new();

    for (int year = 1958; year <= 2022; year++)
    {
        teamsToAdd = teamScraper.ScrapeTeams(year);
        Console.WriteLine(_teams.Count);
        _teams.AddRange(teamsToAdd);
    }

    _teams = _teams.DistinctBy(t => t.Name).ToList();

    var teamsJson = JsonConvert.SerializeObject(_teams, Formatting.Indented);
    File.WriteAllText(@"D:\C#\Formula1Project\Scraper\Scraper\Json\teams.json", teamsJson);

    for (int year = 1950; year <= 2022; year++)
    {
        driversToAdd = driverScraper.ScrapeDrivers(year, _teams);

        drivers.AddRange(driversToAdd);
    }

    drivers = drivers.DistinctBy(d => d.FirstName + " " + d.LastName).ToList();

    var json = JsonConvert.SerializeObject(drivers, Formatting.Indented);

    File.WriteAllText(@"D:\C#\Formula1Project\Scraper\Scraper\Json\drivers.json", json);

    //Serializer.SerializeDriverModel();
    //Serializer.SerializeCircuitModel();
    //Serializer.SerializeTeamModel();
    //Serializer.SerializeRaceResultModel();
}

void Meth()
{

    List<Team> teams = new()
{
    new Team()
    {
        Id = 1,
        Name = "Aston Martin"
    },
    new Team()
    {
        Id = 2,
        Name = "Ferrari"
    },
    new Team()
    {
        Id = 3,
        Name = "Mercedes"
    }
};


    List<Driver> drivers = new()
{
    new Driver()
    {
        Id = 1,
        FirstName = "Fernando",
        LastName = "Alonso",
        Nationality = "Spain"
    },
    new Driver()
    {
        Id = 2,
        FirstName = "Charles",
        LastName = "Leclerc",
        Nationality = "Monaco"
    }
};

    string alonsoString = "Aston Martin";
    string leclercString = "Ferari";

    string teamString = "Aston Martin";



    foreach (var driver in drivers)
    {
        Console.WriteLine(driver.FirstName);

        var teamVar = teams.Where(t => t.Name == teamString).ToList();

        driver.Team = teamVar;

        foreach (var item in teamVar)
        {
            Console.WriteLine(item.Name);
        }
    }

}
