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
    private int _index = 1;

    internal List<Team> ScrapeTeams(List<string> links)
    {
        List<Team> teams = new();

        HtmlWeb web = new();
        web.OverrideEncoding = Encoding.UTF8;
        HtmlDocument document = new();

        // a loop variable
        string scrapedName;

        foreach (var link in links)
        {
            document = web.Load("https://www.formula1.com" + link);

            // using null-conditional operator to check the node,
            // then null coalescing operator to check the inner text of the node
            scrapedName = document.DocumentNode.SelectSingleNode(
                "/html/body/div[1]/main/article/div/div[2]/div[2]" +
                "/div[2]/div[2]/table/tbody/tr[1]" +
                "/td[@class='semi-bold uppercase hide-for-tablet']")?.InnerText ?? string.Empty;

            // if there's no name on the page - a race from the link hasn't taken place yet
            if (string.IsNullOrEmpty(scrapedName))
            {
                break;
            }

            // if a team has already been scraped - move to the next loop iteration
            if (teams.Any(t => t.Name == scrapedName))
            {
                continue;
            }

            // add a team to the returned list of teams
            teams.Add(new Team
            {
                Id = _index++,
                Name = scrapedName
            });
        }

        return teams;
    }
}
