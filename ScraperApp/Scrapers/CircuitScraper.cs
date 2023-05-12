using HtmlAgilityPack;
using ScraperApp.Models;
using ScraperApp.Serialization;
using System.Text;

namespace ScraperApp.Scrapers;

internal class CircuitScraper
{
    private int _index = 1;

    internal List<Circuit> ScrapeCircuits()
    {
        List<Circuit> circuits = new();

        HtmlWeb web = new();
        web.OverrideEncoding = Encoding.UTF8;

        List<string> links = JsonDeserializer.DeserializeCollection<string>("links");

        foreach (var link in links)
        {
            HtmlDocument document = web.Load(link);

            string scrapedFullName = document.DocumentNode.SelectSingleNode(
                "/html/body/div[1]/main/article/div/div[2]/div[2]/div[1]/p" +
                "/span[@class='circuit-info']").InnerText;

            // If there's no name on the page - a race from the link hasn't taken place yet.
            if (string.IsNullOrEmpty(scrapedFullName))
            {
                break;
            }

            // If a circuit has already been scraped - move to the next loop iteration.
            if (circuits.Any(c => c.Name + ", " + c.Location == scrapedFullName))
            {
                continue;
            }

            // Split the scraped name into location and cicruit name.
            string[] scrapedParts = scrapedFullName.Split(',');
            string scrapedName = scrapedParts[0].Trim();
            string scrapedLocation = scrapedParts[1].Trim();
            
            // Add everything, even duplicates.
            circuits.Add(new Circuit
            {
                CircuitId = _index++,
                Name = scrapedName,
                Location = scrapedLocation
            });
            Console.WriteLine($"Circuit: {scrapedFullName} added.");
        }

        // Remove duplicates separately for fewer queries - increased performance.
        List<Circuit> distinctCircuits = circuits.DistinctBy(c => c.Name + c.Location).ToList();

        // Add IDs.
        distinctCircuits.ForEach(c => c.CircuitId = _index++);

        return circuits;
    }
}
