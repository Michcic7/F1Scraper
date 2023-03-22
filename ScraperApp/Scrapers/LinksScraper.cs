using HtmlAgilityPack;
using ScraperApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScraperApp.Scrapers;

internal class LinksScraper
{
    internal List<string> ScrapeLinks(int year)
    {
        HtmlWeb web = new();
        web.OverrideEncoding = Encoding.UTF8;
        HtmlDocument document = web.Load($"https://www.formula1.com/en/results.html/{year}/races.html");

        HtmlNodeCollection unorderedList = document.DocumentNode.SelectNodes(
            "//div[@class='resultsarchive-filter-container']" +
            "/div[@class='resultsarchive-filter-wrap']" +
            "/ul[@class='resultsarchive-filter ResultFilterScrollable']" +
            "/li[@class='resultsarchive-filter-item']/a");

        List<string> links = new();

        foreach (var row in unorderedList)
        {
            string attribute = row.SelectSingleNode(".").Attributes["data-name"].Value;

            if (attribute == "meetingKey")
            {
                string link = row.SelectSingleNode(".").Attributes["href"].Value;

                if (!link.EndsWith("races.html"))
                {
                    links.Add(link);
                }
            }
        }

        return links;
    }
}
