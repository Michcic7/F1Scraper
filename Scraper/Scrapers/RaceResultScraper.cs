using HtmlAgilityPack;
using ScraperApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScraperApp.Scrapers;

internal class RaceResultScraper
{
    private int index = 1;
    public List<RaceResult> ScrapeRaceResults(int year)
    {
        HtmlWeb web = new();
        web.OverrideEncoding = Encoding.UTF8;
        HtmlDocument document = web.Load($"https://www.formula1.com/en/results.html/{year}/races.html");

        List<RaceResult> raceResults = new();

        foreach (var row in document.DocumentNode.SelectNodes(
            "//table[@class='resultsarchive-table']/tbody/tr"))
        {
            RaceResult raceResult = new RaceResult();

            try
            {
                var date = row.SelectSingleNode(
                    "./td[@class='dark hide-for-mobile']").InnerText;
                var laps = row.SelectSingleNode(
                    "./td[@class='bold hide-for-mobile']").InnerText;
                var time = row.SelectSingleNode(
                    "./td[@class='dark bold hide-for-tablet']").InnerText;

                raceResult.Id = index++;
                raceResult.Date = date;
                if (laps == "null")
                    laps = "0";
                raceResult.Laps = int.Parse(laps);
                raceResult.Time = time;                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            raceResults.Add(raceResult);
        }

        return raceResults;
    }
}
