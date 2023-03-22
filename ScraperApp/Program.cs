using HtmlAgilityPack;
using Newtonsoft.Json;
using ScraperApp;
using ScraperApp.Models;
using ScraperApp.Scrapers;
using System.Net;
using System.Text;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Reflection.Metadata;

//Serializer.SerializeLinks();

internal class Program
{
    private static void Main(string[] args)
    {
        Serializer serializer = new();

        serializer.SerializeDrivers();
    }

    //private List<Team> DeserializeTeams()
    //{
    //    List<Team> teams = new();

    //    using (StreamReader reader = new(@"D:\C#\Formula1Project\Scraper\ScraperApp\Json\teams.json"))
    //    {
    //        string json = reader.ReadToEnd();
    //        teams = JsonConvert.DeserializeObject<List<Team>>(json);
    //    }

    //    Serializer.SerializeCircuits(teams);
    //}

}



//RaceResultScraper _raceResultScraper = new();
//List<string> _links = new();
//List<RaceResult> _raceResults = new();




//using (StreamReader reader = new(@"D:\C#\Formula1Project\Scraper\Scraper\Json\links.json"))
//{
//    string json = reader.ReadToEnd();
//    _links = JsonConvert.DeserializeObject<List<string>>(json);
//}

//foreach (var link in _links)
//{
//    List<RaceResult> raceResultsToAdd = _raceResultScraper.ScrapeIndividualRaceResults(link);
//    _raceResults.AddRange(raceResultsToAdd);
//}

//Console.WriteLine($"Links: {_links.Count}");

//Console.WriteLine($"Race result: {_raceResults.Count}");



//DriverScraper driverScraper = new();
//TeamScraper TeamScraper = new();
//CircuitScraper circuitScraper = new();
//RaceResultScraper raceResultScraper = new();

//List<Driver> _drivers = new();
//List<Team> _teams = new();
//List<Circuit> _circuits = new();
//List<RaceResult> _raceResults = new();

//List<Driver> driversToAdd = new();
//List<Team> teamsToAdd = new();
//List<Circuit> circuitsToAdd = new();
//List<RaceResult> raceResultsToAdd = new();

//for (int year = 1950; year <= 2023; year++)
//{
//    driversToAdd = driverScraper.ScrapeDrivers(year);    
//    _drivers.AddRange(driversToAdd);

//    circuitsToAdd = circuitScraper.ScrapeCircuits(year);
//    _circuits.AddRange(circuitsToAdd);
//}
//_drivers = _drivers.DistinctBy(d => d.FirstName + " " + d.LastName).ToList();
//_circuits = _circuits.DistinctBy(c => c.Name).ToList();

//for (int year = 1958; year <= 2023; year++)
//{
//    teamsToAdd = TeamScraper.ScrapeTeams(year);
//    _teams.AddRange(teamsToAdd);
//}
//_teams = _teams.DistinctBy(t => t.Name).ToList();

//for (int year = 1950; year <= 2023; year++)
//{
//    raceResultsToAdd = raceResultScraper.ScrapeRaceResults(year, _drivers, _teams, _circuits);
//    _raceResults.AddRange(raceResultsToAdd);
//}

//foreach (var raceResult in _raceResults)
//{
//    Console.WriteLine();
//}
