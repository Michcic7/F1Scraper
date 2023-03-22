using HtmlAgilityPack;
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

    public List<TeamStanding> ScrapeTeamStandings(int year, List<Team> teams)
    {
        HtmlWeb web = new();
        web.OverrideEncoding = Encoding.UTF8;

        HtmlDocument document = web.Load(
            $"https://www.formula1.com/en/results.html/{year}/team.html");

        List<TeamStanding> teamStandings = new();

        foreach (var row in document.DocumentNode.SelectNodes(
            "//table[@class='resultsarchive-table']/tbody/tr"))
        {
            TeamStanding teamStandingToAdd = new();

            string scrapedPoints = row.SelectSingleNode(
                "./td[@class='dark bold']").InnerText;

            string teamName = row.SelectSingleNode(
                "./td[@class='semi-bold uppercase']").InnerText;

            teamStandingToAdd.Id = _index++;

            if (int.TryParse(scrapedPoints, out int points))
            {
                teamStandingToAdd.Points = points;
            }
            else
            {
                teamStandingToAdd.Points = 0;
            }

            teamStandingToAdd.Team = teams.FirstOrDefault(t => t.Name == teamName);

            teamStandingToAdd.Year = year;

            teamStandings.Add(teamStandingToAdd);
        }

        return teamStandings;
    }
}

//     var position = item.SelectSingleNode(
//                "./td[@class='dark']").InnerText;
//                var name = item.SelectSingleNode(
//                    "./td/a[@class='dark bold uppercase ArchiveLink']").InnerText;
//                var points = item.SelectSingleNode(
//                    "./td[@class='dark bold']").InnerText;

//                team.TeamId = index++;
//                team.StandingsYearId = year;
//                team.Position = position;
//                team.Name = name;
//                if (points == "null")
//                    points = "0";
//                team.Points = float.Parse(points);

