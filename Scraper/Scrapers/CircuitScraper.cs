using HtmlAgilityPack;
using ScraperApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScraperApp.Scrapers;

internal class CircuitScraper
{
    private int index = 1;

    public List<Circuit> ScrapeCircuits(int year)
    {
        HtmlWeb web = new();
        web.OverrideEncoding = Encoding.UTF8;
        HtmlDocument document = web.Load($"https://www.formula1.com/en/results.html/{year}/races.html");

        List<Circuit> circuits = new();

        foreach (var row in document.DocumentNode.SelectNodes(
            "//table[@class='resultsarchive-table']/tbody/tr"))
        {
            Circuit circuit = new Circuit();

            try
            {
                circuit.Id = index++;
                circuit.Name = row.SelectSingleNode(
                "./td/a[@class='dark bold ArchiveLink']").InnerText.Trim();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            circuits.Add(circuit);
        }

        return circuits;
    }
}
