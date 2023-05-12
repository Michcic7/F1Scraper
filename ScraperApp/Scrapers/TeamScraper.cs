using HtmlAgilityPack;
using ScraperApp.Models;
using ScraperApp.Serialization;
using System.Text;

namespace ScraperApp.Scrapers;

internal class TeamScraper
{
    private int _index = 1;

    internal List<Team> ScrapeTeams()
    {
        List<Team> teams = new();

        HtmlWeb web = new();
        web.OverrideEncoding = Encoding.UTF8;

        List<string> links = JsonDeserializer.DeserializeCollection<string>("links");

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
                // Using null-conditional operator to check the node,
                // then null coalescing operator to check the inner text of the node.
                string scrapedName = row.SelectSingleNode(
                    "./td[@class='semi-bold uppercase hide-for-tablet']")?.InnerText.Trim() ?? string.Empty;

                // If there's no name on the page - a race from the link hasn't taken place yet.
                if (string.IsNullOrEmpty(scrapedName))
                {
                    break;
                }

                // Add everything, even duplicates.
                teams.Add(new Team
                {
                    Name = scrapedName
                });

                Console.WriteLine($"Team: {scrapedName} scraped.");
            }
        }

        // Remove duplicates separately for fewer queries - increased performance.
        List<Team> distinctTeams = teams.DistinctBy(t => t.Name).ToList();
        
        // Add IDs.
        distinctTeams.ForEach(t => t.TeamId = _index++);

        return distinctTeams;
    }
}
