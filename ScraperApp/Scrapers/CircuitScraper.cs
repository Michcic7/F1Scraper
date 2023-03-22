using HtmlAgilityPack;
using Microsoft.VisualBasic;
using ScraperApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ScraperApp.Scrapers;

internal class CircuitScraper
{
    private int _index = 1;

    internal List<Circuit> ScrapeCircuits(List<string> links)
    {
        List<Circuit> circuits = new();

        HtmlWeb web = new();
        web.OverrideEncoding = Encoding.UTF8;
        HtmlDocument document = new();

        // loop variables
        string scrapedFullName = string.Empty;
        string[] scrapedParts = new string[2];
        string scrapedName = string.Empty;
        string scrapedCountry = string.Empty;

        foreach (var link in links)
        {
            document = web.Load("https://www.formula1.com" + link);

            scrapedFullName = document.DocumentNode.SelectSingleNode(
                "/html/body/div[1]/main/article/div/div[2]/div[2]/div[1]/p" +
            "/span[@class='circuit-info']").InnerText;

            // if there's no name on the page - a race from the link hasn't taken place yet
            if (string.IsNullOrEmpty(scrapedName))
            {
                break;
            }

            // if a circuit has already been scraped - move to the next loop iteration
            if (circuits.Any(c => c.Name + ", " + c.Country == scrapedFullName))
            {
                continue;
            }

            // split the scraped name into country and cicruit name
            scrapedParts = scrapedFullName.Split(',');
            scrapedName = scrapedParts[0].Trim();
            scrapedCountry = scrapedParts[1].Trim();
            
            // add a circuit to the returned list of circuits
            circuits.Add(new Circuit
            {
                Id = _index++,
                Name = scrapedName,
                Country = scrapedCountry
            });
        }

        return circuits;
    }
}
