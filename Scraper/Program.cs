using HtmlAgilityPack;
using Newtonsoft.Json;
using ScraperApp;
using ScraperApp.Models;
using ScraperApp.Scrapers;
using System.Net;
using System.Text;
using System;
using System.Reflection;

TeamScraper teamScraper = new();
DriverScraper driverScraper = new();
List<Team> _teams = new();
List<Driver> _drivers = new();

List<Team> teamsToAdd = new();
for (int year = 1958; year <= 2022; year++)
{
    teamsToAdd = teamScraper.ScrapeTeams(year);
    _teams.AddRange(teamsToAdd);
}
_teams = _teams.DistinctBy(t => t.Name).ToList();

Serializer.SerializeDriverModel(_teams);


//void Method2()
//{
//    int index = 1;

//    TeamScraper teamScraper = new();
//    List<Team> _teams = new();

//    List<Team> teamsToAdd = new();
//    for (int year = 1958; year <= 2022; year++)
//    {
//        teamsToAdd = teamScraper.ScrapeTeams(year);
//        Console.WriteLine(_teams.Count);
//        _teams.AddRange(teamsToAdd);
//    }
//    _teams = _teams.DistinctBy(t => t.Name).ToList();
//    var teamsJson = JsonConvert.SerializeObject(_teams, Formatting.Indented);
//    File.WriteAllText(@"D:\C#\Formula1Project\Scraper\Scraper\Json\teams.json", teamsJson);

//    List<Driver> drivers = ScrapeD(1990, _teams);

//    drivers = drivers.DistinctBy(d => d.FirstName + " " + d.LastName).ToList();
//    var driversJson = JsonConvert.SerializeObject(drivers, Formatting.Indented);
//    File.WriteAllText(@"D:\C#\Formula1Project\Scraper\Scraper\Json\drivers.json", driversJson);
//}

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

        driver.Teams = teamVar;

        foreach (var item in teamVar)
        {
            Console.WriteLine(item.Name);
        }
    }

}
