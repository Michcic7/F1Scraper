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

    internal List<Driver> ScrapeDrivers(List<string> links)
    {
        HtmlWeb web = new();
        web.OverrideEncoding = Encoding.UTF8;
        HtmlDocument document = new();

        List<Driver> drivers = ScrapeDriversWithNationality();

        string scrapedDriverFirstName = string.Empty;
        string scrapedDriverLastName = string.Empty;
        string scrapedDriverFullName = string.Empty;
        int newDriverId = 500;

        List<Driver> driversWithoutNationality = new();

        foreach (var link in links)
        {
            document = web.Load("https://www.formula1.com" + link);

            foreach (var row in document.DocumentNode.SelectNodes(
                "//table[@class='resultsarchive-table']/tbody/tr"))
            {
                scrapedDriverFirstName = row.SelectSingleNode(
                        "./td/span[@class='hide-for-tablet']").InnerText.Trim();
                scrapedDriverLastName = row.SelectSingleNode(
                    "./td/span[@class='hide-for-mobile']").InnerText.Trim();

                scrapedDriverFullName = scrapedDriverFirstName + " " + scrapedDriverLastName;

                Driver existingDriver = drivers.FirstOrDefault(d =>
                    d.FirstName + " " + d.LastName == scrapedDriverFullName);

                if (existingDriver == null)
                {
                    Console.WriteLine($"Driver: {scrapedDriverFullName} doesn't exist");

                    driversWithoutNationality.Add(new Driver()
                    {
                        DriverId = newDriverId++,
                        FirstName = scrapedDriverFirstName,
                        LastName = scrapedDriverLastName
                    });

                    drivers.Add(driversWithoutNationality.Last());
                }
            }
        }

        return drivers;
    }

    private List<Driver> ScrapeDriversWithNationality()
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
                    "./td/a/span[@class='hide-for-tablet']")?.InnerText.Trim() ?? string.Empty;

                // if there's no first name on the page - a season hasn't started yet
                if (string.IsNullOrEmpty(scrapedFirstName))
                {
                    break;
                }

                scrapedLastName = row.SelectSingleNode(
                    "./td/a/span[@class='hide-for-mobile']").InnerText.Trim();

                // if a driver has already been scraped - move to the next loop iteration
                if (drivers.Any(d => d.FirstName == scrapedFirstName &&
                                     d.LastName == scrapedLastName))
                {
                    continue;
                }

                scrapedNationality = row.SelectSingleNode(
                    "./td[@class='dark semi-bold uppercase']").InnerText.Trim();

                drivers.Add(new Driver
                {
                    DriverId = _index++,
                    FirstName = scrapedFirstName,
                    LastName = scrapedLastName,
                    Nationality = scrapedNationality

                });
            }
        }

        return drivers;
    }

    
}