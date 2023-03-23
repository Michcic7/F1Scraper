using HtmlAgilityPack;
using ScraperApp.Json;
using ScraperApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScraperApp.Scrapers;

internal class DriverStandingScraper
{
    private int _index = 1;

    internal List<DriverStanding> ScrapeDriverStandings()
    {
        List<Driver> drivers = JsonDeserializer.Deserialize<Driver>("drivers");
        List<Team> teams = JsonDeserializer.Deserialize<Team>("teams");

        HtmlWeb web = new();
        web.OverrideEncoding = Encoding.UTF8;
        HtmlDocument document = new();

        List<DriverStanding> driverStandings = new();
        
        // loop variables
        string scrapedPositionString = string.Empty;
        int scrapedPosition = 0;
        string scrapedPointsString = string.Empty;
        float scrapedPoints = 0;
        string scrapedFirstName = string.Empty;
        string scrapedLastName = string.Empty;
        string scrapedFullName = string.Empty;
        string scrapedTeamName = string.Empty;

        for (int year = 1950; year < 2023; year++)
        {
            document = web.Load(
                $"https://www.formula1.com/en/results.html/{year}/drivers.html");

            foreach (var row in document.DocumentNode.SelectNodes(
                "//table[@class='resultsarchive-table']/tbody/tr"))
            {
                scrapedFirstName = row.SelectSingleNode(
                        "./td/a/span[@class='hide-for-tablet']").InnerText;
                scrapedLastName = row.SelectSingleNode(
                    "./td/a/span[@class='hide-for-mobile']").InnerText;

                scrapedFullName = scrapedFirstName + " " + scrapedLastName;

                Driver existingDriver = drivers.FirstOrDefault(d =>
                    d.FirstName + " " + d.LastName == scrapedFullName);

                if (existingDriver == null)
                {
                    Console.WriteLine($"Driver: {scrapedFullName} doesn't exist");
                    break;
                }

                scrapedTeamName = row.SelectSingleNode(
                    "./td/a[@class='grey semi-bold uppercase ArchiveLink']").InnerText;

                Team existingTeam = teams.FirstOrDefault(t => t.Name == scrapedTeamName);

                if (existingTeam == null)
                {
                    Console.WriteLine($"Year: {year} {scrapedTeamName} is missing in teams.json");
                    break;
                }

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
                        "./td[@class='dark bold']").InnerText;

                if (float.TryParse(scrapedPointsString, out float points))
                {
                    scrapedPoints = points;
                }
                else
                {
                    scrapedPoints = 0;
                }

                driverStandings.Add(new DriverStanding()
                {
                    Id = _index++,
                    Position = scrapedPosition,
                    Driver = existingDriver,
                    Team = existingTeam,
                    Points = scrapedPoints,
                    Year = year
                });
            }
        }

        return driverStandings;
    }
}
