using HtmlAgilityPack;
using ScraperApp.Models;
using System.Text;

namespace ScraperApp.Scrapers;

internal class TeamScraper
{
    private int _index = 1;

    internal List<Team> ScrapeTeams(List<string> links)
    {
        List<Team> teams = new();

        HtmlWeb web = new();
        web.OverrideEncoding = Encoding.UTF8;

        foreach (var link in links)
        {
            HtmlDocument document = web.Load(link);

            var rows = document.DocumentNode.SelectNodes(
                "//table[@class='resultsarchive-table']/tbody/tr") ?? null;

            // If there's no row node on the page - the race hasn't taken place yet.
            if (rows == null)
            {
                break;
            }

            foreach (var row in rows)
            {
                // using null-conditional operator to check the node,
                // then null coalescing operator to check the inner text of the node
                string scrapedName = row.SelectSingleNode(
                    "./td[@class='semi-bold uppercase hide-for-tablet']")?.InnerText.Trim() ?? string.Empty;

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

                teams.Add(new Team
                {
                    TeamId = _index++,
                    Name = scrapedName
                });

                Console.WriteLine($"Team: {scrapedName} added.");
            }
        }

        return teams;
    }
}
