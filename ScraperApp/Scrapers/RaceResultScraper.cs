using HtmlAgilityPack;
using ScraperApp.Json;
using ScraperApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ScraperApp.Scrapers;

internal class RaceResultScraper
{
    private int _index = 1;

    internal List<RaceResult> ScrapeRaceResults(List<string> links)
    {
        List<Driver> drivers = JsonDeserializer.Deserialize<Driver>("drivers");
        List<Team> teams = JsonDeserializer.Deserialize<Team>("teams");
        List<Circuit> circuits = JsonDeserializer.Deserialize<Circuit>("circuits");

        HtmlWeb web = new();
        web.OverrideEncoding = Encoding.UTF8;
        HtmlDocument document = new();
        HtmlDocument documentForDate = new();

        List<RaceResult> raceResults = new();

        // loop variables
        string yearString = string.Empty;
        int scrapedYear = 0;
        string scrapedCircuitFullName = string.Empty;
        string[] scrapedParts = new string[2];
        string scrapedCircuitName = string.Empty;
        string scrapedPositionString = string.Empty;
        int scrapedPosition = 0;
        string scrapedPointsString = string.Empty;
        float scrapedPoints = 0;
        string scrapedDateString = string.Empty;
        DateOnly scrapedDate = DateOnly.MinValue;
        string scrapedDriverFirstName = string.Empty;
        string scrapedDriverLastName = string.Empty;
        string scrapedDriverFullName = string.Empty;
        string scrapedTeamName = string.Empty;
        string scrapedLapsString = string.Empty;
        int scrapedLaps = 0;
        string scrapedTime = string.Empty;
        bool addedNewDriver = false;
        List<Driver> driversNotPresentInJson = new();
        int newDriverId = 500;
        int newDriverIndexInList = 0;
        HtmlNodeCollection twoSameNodes;

        foreach (var link in links)
        {
            document = web.Load("https://www.formula1.com" + link);

            scrapedCircuitFullName = document.DocumentNode.SelectSingleNode(
                "/html/body/div[1]/main/article/div/div[2]/div[2]/div[1]/p" +
                "/span[@class='circuit-info']").InnerText;

            scrapedParts = scrapedCircuitFullName.Split(',');
            scrapedCircuitName = scrapedParts[0].Trim();

            Circuit existingCircuit = circuits.FirstOrDefault(c =>
                c.Name == scrapedCircuitName);

            if (existingCircuit == null)
            {
                Console.WriteLine($"Circuit: {scrapedCircuitName} not found in circuits.json");
            }

            yearString = link[17..21];

            if (int.TryParse(yearString, out int year))
            {
                scrapedYear = year;
            }
            else
            {
                Console.WriteLine($"Year: {yearString} is invalid");
            }

            scrapedDateString = document.DocumentNode.SelectSingleNode(
                    "//div[@class='resultsarchive-content-header group']" +
                    "/p[@class='date']/span[@class='full-date']").InnerText;

            if (DateOnly.TryParseExact(scrapedDateString, "dd MMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly date))
            {
                scrapedDate = date;
            }
            else
            {
                Console.WriteLine($"Something's wrong: {link} \n" +
                    $"Date: {scrapedDateString}");
            }

            foreach (var row in document.DocumentNode.SelectNodes(
                "//table[@class='resultsarchive-table']/tbody/tr"))
            {
                scrapedPositionString = row.SelectSingleNode(
                    "./td[@class='dark']").InnerText;

                if (int.TryParse(scrapedPositionString, out int position))
                {
                    scrapedPosition = position;
                }
                else
                {
                    scrapedPosition = 0;
                }

                scrapedPointsString = row.SelectSingleNode(
                        "./td[@class='bold']").InnerText;

                if (float.TryParse(scrapedPointsString, out float points))
                {
                    scrapedPoints = points;
                }
                else
                {
                    scrapedPoints = 0;
                }

                scrapedDriverFirstName = row.SelectSingleNode(
                        "./td/span[@class='hide-for-tablet']").InnerText.Trim();
                scrapedDriverLastName = row.SelectSingleNode(
                    "./td/span[@class='hide-for-mobile']").InnerText.Trim();

                scrapedDriverFullName = scrapedDriverFirstName + " " + scrapedDriverLastName;

                Driver existingDriver = drivers.FirstOrDefault(d =>
                    d.FirstName + " " + d.LastName == scrapedDriverFullName);

                if (existingDriver == null)
                {
                    Console.WriteLine($"Driver: {scrapedDriverFullName} doesn't exist");

                    driversNotPresentInJson.Add(new Driver()
                    {
                        DriverId = newDriverId++,
                        FirstName = scrapedDriverFirstName,
                        LastName = scrapedDriverLastName
                    });

                    drivers.Add(driversNotPresentInJson.Last());

                    addedNewDriver = true;
                }

                scrapedTeamName = row.SelectSingleNode(
                    "./td[@class='semi-bold uppercase hide-for-tablet']").InnerText.Trim();

                Team existingTeam = teams.FirstOrDefault(t => t.Name == scrapedTeamName);

                if (existingTeam == null)
                {
                    Console.WriteLine(
                        $"Year: {scrapedYear} {scrapedTeamName} is missing in teams.json");
                }

                scrapedLapsString = row.SelectSingleNode(
                    "./td[@class='bold hide-for-mobile']").InnerText;

                if (int.TryParse(scrapedLapsString, out int laps))
                {
                    scrapedLaps = laps;
                }
                else
                {
                    scrapedLaps = 0;
                }

                twoSameNodes = row.SelectNodes("./td[@class='dark bold']");

                scrapedTime = twoSameNodes[1].InnerText;

                if (addedNewDriver)
                {
                    raceResults.Add(new RaceResult
                    {
                        RaceResultId = _index++,
                        Year = scrapedYear,
                        Position = scrapedPosition,
                        Circuit = existingCircuit,
                        Date = scrapedDate,
                        Driver = driversNotPresentInJson[newDriverIndexInList],
                        Team = existingTeam,
                        Laps = scrapedLaps,
                        Time = scrapedTime,
                        Points = scrapedPoints,
                    });

                    newDriverIndexInList++;
                    addedNewDriver = false;
                }
                else
                {
                    raceResults.Add(new RaceResult
                    {
                        RaceResultId = _index++,
                        Year = scrapedYear,
                        Position = scrapedPosition,
                        Circuit = existingCircuit,
                        Date = scrapedDate,
                        Driver = existingDriver,
                        Team = existingTeam,
                        Laps = scrapedLaps,
                        Time = scrapedTime,
                        Points = scrapedPoints,
                    });
                }
            }
        }

        return raceResults;
    }
}
