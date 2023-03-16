using HtmlAgilityPack;
using ScraperApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScraperApp.Scrapers;

internal class TeamScraper
{
    private int index = 1;

    public List<Team> ScrapeTeams(int year)
    {
        HtmlWeb web = new();
        web.OverrideEncoding = Encoding.UTF8;
        HtmlDocument document = web.Load($"https://www.formula1.com/en/results.html/{year}/team.html");

        List<Team> teams = new();

        foreach (var row in document.DocumentNode.SelectNodes(
            "//table[@class='resultsarchive-table']/tbody/tr"))
        {
            Team team = new Team();

            try
            {
                team.Id = index++;

                team.Name = row.SelectSingleNode(
                    "./td/a[@class='dark bold uppercase ArchiveLink']").InnerText;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            teams.Add(team);
        }

        return teams;
    }
}
