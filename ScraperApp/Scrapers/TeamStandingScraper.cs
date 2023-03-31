using HtmlAgilityPack;
using Newtonsoft.Json;
using ScraperApp.Json;
using ScraperApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScraperApp.Scrapers;

internal class TeamStandingScraper
{
    private int _index = 1;

    internal List<TeamStanding> ScrapeTeamStandings()
    {
        List<Team> teams = JsonDeserializer.Deserialize<Team>("teams");
        
        HtmlWeb web = new();
        web.OverrideEncoding = Encoding.UTF8;
        HtmlDocument document = new();

        List<TeamStanding> teamStandings = new();

        string scrapedPositionString = string.Empty;
        int scrapedPosition = 0;
        string scrapedTeamName = string.Empty;
        string scrapedPointsString = string.Empty;
        float scrapedPoints = 0;

        for (int year = 1958; year < 2023; year++)
        {
            document = web.Load(
                $"https://www.formula1.com/en/results.html/{year}/team.html");

            foreach (var row in document.DocumentNode.SelectNodes(
                "//table[@class='resultsarchive-table']/tbody/tr"))
            {
                scrapedTeamName = row.SelectSingleNode(
                    "./td/a[@class='dark bold uppercase ArchiveLink']")?.InnerText.Trim() ?? string.Empty;

                if (string.IsNullOrEmpty(scrapedTeamName))
                {
                    break;
                }

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

                teamStandings.Add(new TeamStanding
                {
                    TeamStandingId = _index++,
                    Position = scrapedPosition,
                    Team = existingTeam,
                    Points = scrapedPoints,
                    Year = year
                });
            }
        }    
        
        return teamStandings;
    }
}