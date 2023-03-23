using HtmlAgilityPack;
using ScraperApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ScraperApp.Scrapers;

internal class DriverScraper
{
    private int _index = 1;

    internal List<Driver> ScrapeDrivers()
    {
        List<Driver> drivers = new();

        HtmlWeb web = new();
        web.OverrideEncoding = Encoding.UTF8;
        HtmlDocument document = new();

        string scrapedFirstName = string.Empty;
        string scrapedLastName = string.Empty;
        string scrapedNationality = string.Empty;

        for (int year = 1950; year < 2023; year++)
        {
            document = web.Load(
                $"https://www.formula1.com/en/results.html/{year}/drivers.html");

            foreach (var row in document.DocumentNode.SelectNodes(
                "//table[@class='resultsarchive-table']/tbody/tr"))
            {
                scrapedFirstName = row.SelectSingleNode(
                    "./td/a/span[@class='hide-for-tablet']")?.InnerText ?? string.Empty;

                // if there's no first name on the page - a season hasn't started yet
                if (string.IsNullOrEmpty(scrapedFirstName))
                {
                    break;
                }

                scrapedLastName = row.SelectSingleNode(
                    "./td/a/span[@class='hide-for-mobile']").InnerText;

                // if a driver has already been scraped - move to the next loop iteration
                if (drivers.Any(d => d.FirstName == scrapedFirstName &&
                                     d.LastName == scrapedLastName))
                {
                    continue;
                }

                scrapedNationality = row.SelectSingleNode(
                    "./td[@class='dark semi-bold uppercase']").InnerText;

                drivers.Add(new Driver
                {
                    Id = _index++,
                    FirstName = scrapedFirstName,
                    LastName = scrapedLastName,
                    Nationality = scrapedNationality

                });
            }
        }

        return drivers;
    }
}