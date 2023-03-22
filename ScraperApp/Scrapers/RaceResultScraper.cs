using HtmlAgilityPack;
using ScraperApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScraperApp.Scrapers;

internal class RaceResultScraper
{
    private int _index = 1;
    //private int _year = 1950;
    
    public List<RaceResult> ScrapeIndividualRaceResults(
        string url)
    {
        HtmlWeb web = new();
        web.OverrideEncoding = Encoding.UTF8;
        HtmlDocument document = web.Load($"https://www.formula1.com{url}");

        string yearString = url[17..21];

        List<RaceResult> raceResults = new();

        foreach (var row in document.DocumentNode.SelectNodes(
            "//table[@class='resultsarchive-table']/tbody/tr"))
        {
            RaceResult raceResult = new RaceResult();

            string positionString = row.SelectSingleNode(
                "./td[@class='dark']").InnerText;
            //string name = row.SelectSingleNode(
            //    "./td/a[@class='dark bold ArchiveLink']").InnerText.Trim();
            //string dateString = row.SelectSingleNode(
            //    "./td[@class='dark hide-for-mobile']").InnerText;
            string winnerFirstName = row.SelectSingleNode(
                "./td[@class='dark bold']/span[@class='hide-for-tablet']").InnerText;
            string winnerLastName = row.SelectSingleNode(
                "./td[@class='dark bold']/span[@class='hide-for-mobile']").InnerText;

            // nulluje!
            string lapsString = row.SelectSingleNode(
                "./td[@class='bold hide-for-mobile']").InnerText ?? "0";
            //string timeString = row.SelectSingleNode(
            //    "./td[@class='dark bold hide-for-tablet']").InnerText;

            raceResult.Id = _index++;


            if (int.TryParse(yearString, out int year))
            {
                raceResult.Year = year;
            }
            else
            {
                raceResult.Year = 0;
            }

            if (int.TryParse(positionString, out int position))
            {
                raceResult.Position = position;
            }
            else
            {
                raceResult.Position = 0;
            }

            //raceResult.Circuit = circuits.FirstOrDefault(c => c.Name == name);

            //if (DateOnly.TryParse(dateString, out DateOnly date))
            //{
            //    raceResult.Date = date;
            //}

            //raceResult.Driver = drivers.FirstOrDefault(d =>
            //    d.FirstName == winnerFirstName && d.LastName == winnerLastName);

            if (lapsString!= null)
            {
                if (int.TryParse(lapsString, out int laps))
                {
                    raceResult.Laps = laps;
                }
                else
                {
                    raceResult.Laps = 0;
                }
            }
            else
            {
                raceResult.Laps = 0;
            }

            //raceResult.Time = timeString;

            //if (TimeSpan.TryParse(timeString, out TimeSpan time))
            //{
            //    raceResult.Time = time;
            //}

            raceResults.Add(raceResult);
        }

        return raceResults;
    }



    //public List<RaceResult> ScrapeRaceResults(
    //    int year, List<Driver> drivers, List<Team> teams, List<Circuit> circuits)
    //{
    //    HtmlWeb web = new();
    //    web.OverrideEncoding = Encoding.UTF8;
    //    HtmlDocument document = web.Load($"https://www.formula1.com/en/results.html/{year}/races.html");

    //    List<RaceResult> raceResults = new();

    //    foreach (var row in document.DocumentNode.SelectNodes(
    //        "//table[@class='resultsarchive-table']/tbody/tr"))
    //    {
    //        RaceResult raceResult = new RaceResult();

    //        string positionString = row.SelectSingleNode(
    //            "./td[@class='dark']").InnerText;
    //        string name = row.SelectSingleNode(
    //            "./td/a[@class='dark bold ArchiveLink']").InnerText.Trim();
    //        string dateString = row.SelectSingleNode(
    //            "./td[@class='dark hide-for-mobile']").InnerText;
    //        string winnerFirstName = row.SelectSingleNode(
    //            "./td[@class='dark bold']/span[@class='hide-for-tablet']").InnerText;
    //        string winnerLastName = row.SelectSingleNode(
    //            "./td[@class='dark bold']/span[@class='hide-for-mobile']").InnerText;
    //        string lapsString = row.SelectSingleNode(
    //            "./td[@class='bold hide-for-mobile']").InnerText;
    //        string timeString = row.SelectSingleNode(
    //            "./td[@class='dark bold hide-for-tablet']").InnerText;

    //        raceResult.Id = _index++;

    //        raceResult.Year = year;

    //        if (int.TryParse(positionString, out int position))
    //        {
    //            raceResult.Position = position;
    //        }
    //        else
    //        {
    //            raceResult.Position = 0;
    //        }

    //        raceResult.Circuit = circuits.FirstOrDefault(c => c.Name == name);

    //        if (DateOnly.TryParse(dateString, out DateOnly date))
    //        {
    //            raceResult.Date = date;
    //        }
                        
    //        raceResult.Driver = drivers.FirstOrDefault(d => 
    //            d.FirstName == winnerFirstName && d.LastName == winnerLastName);

    //        if (int.TryParse(lapsString, out int laps))
    //        {
    //            raceResult.Laps = laps;
    //        }
    //        else
    //        {
    //            raceResult.Laps = 0;
    //        }
            
    //        if (TimeSpan.TryParse(timeString, out TimeSpan time))
    //        {
    //            raceResult.Time = time;
    //        }

    //        raceResults.Add(raceResult);
    //    }

    //    return raceResults;
    //}
}
