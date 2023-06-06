﻿using HtmlAgilityPack;
using ScraperApp.Interfaces;
using System.Text;

namespace ScraperApp.Scrapers;

internal class LinksScraper : ILinksScraper
{
    public IEnumerable<string> ScrapeLinks()
    {
        HtmlWeb web = new();
        web.OverrideEncoding = Encoding.UTF8;

        List<string> links = new();

        for (int year = 1950; year <= DateTime.Now.Year; year++)
        {
            HtmlDocument document = web.Load($"https://www.formula1.com/en/results.html/{year}/races.html");

            HtmlNodeCollection unorderedList = document.DocumentNode.SelectNodes(
                "//div[@class='resultsarchive-filter-container']" +
                "/div[@class='resultsarchive-filter-wrap']" +
                "/ul[@class='resultsarchive-filter ResultFilterScrollable']" +
                "/li[@class='resultsarchive-filter-item']/a");

            foreach (var row in unorderedList)
            {
                string attribute = row.SelectSingleNode(".").Attributes["data-name"].Value;

                if (attribute == "meetingKey")
                {
                    string link = row.SelectSingleNode(".").Attributes["href"].Value;

                    if (!link.EndsWith("races.html"))
                    {
                        string fullLink = $"https://www.formula1.com{link}";

                        links.Add(fullLink);
                        Console.WriteLine(fullLink);
                    }
                }
            }
        }

        return links;
    }
}
