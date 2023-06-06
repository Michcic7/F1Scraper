using HtmlAgilityPack;
using ScraperApp.Models;
using ScraperApp.Serialization;
using System.Text;

namespace ScraperApp.Scrapers;

internal class DriverScraperOld
{
    private int _index = 1;
    private readonly int _startYear = 1950;
        
    internal List<Driver> ScrapeDrivers()
    {
        List<Driver> drivers = ScrapeDriversWithoutNationality();

        HtmlWeb web = new();
        web.OverrideEncoding = Encoding.UTF8;
        HtmlDocument document = new();

        for (int year = _startYear; year <= DateTime.Now.Year; year++)
        {
            document = web.Load(
                $"https://www.formula1.com/en/results.html/{year}/drivers.html");

            foreach (var row in document.DocumentNode.SelectNodes(
                "//table[@class='resultsarchive-table']/tbody/tr"))
            {
                string scrapedFirstName = row.SelectSingleNode(
                    "./td/a/span[@class='hide-for-tablet']")?.InnerText.Trim() ?? string.Empty;

                // If there's no first name on the page - a season hasn't started yet.
                if (string.IsNullOrEmpty(scrapedFirstName))
                {
                    break;
                }

                string scrapedLastName = row.SelectSingleNode(
                    "./td/a/span[@class='hide-for-mobile']").InnerText.Trim();

                Driver existingDriver = drivers.FirstOrDefault(d =>
                    d.FirstName == scrapedFirstName && d.LastName == scrapedLastName);

                if (existingDriver == null)
                {
                    continue;
                }

                if (existingDriver.Nationality == null)
                {
                    string scrapedNationality = row.SelectSingleNode(
                        "./td[@class='dark semi-bold uppercase']").InnerText.Trim();

                    existingDriver.Nationality = scrapedNationality;

                    Console.WriteLine($"Nationality: {scrapedNationality} added to {scrapedFirstName} {scrapedLastName}.");
                }                
            }
        }

        return drivers;
    }

    private List<Driver> ScrapeDriversWithoutNationality()
    {
        HtmlWeb web = new();
        web.OverrideEncoding = Encoding.UTF8;

        List<string> raceResultsLinks = JsonDeserializer.DeserializeCollection<string>("links");

        List<Driver> drivers = new();

        foreach (var link in raceResultsLinks)
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
                string scrapedFirstName = row.SelectSingleNode(
                        "./td/span[@class='hide-for-tablet']").InnerText.Trim();

                string scrapedLastName = row.SelectSingleNode(
                    "./td/span[@class='hide-for-mobile']").InnerText.Trim();

                // Add everything, even duplicates.
                drivers.Add(new Driver()
                {
                    FirstName = scrapedFirstName,
                    LastName = scrapedLastName
                });

                Console.WriteLine($"Driver: {scrapedFirstName} {scrapedLastName} scraped.");
            }
        }

        // Remove duplicates separately for fewer queries - increased performance.
        List<Driver> distinctDrivers = drivers.DistinctBy(d => d.FirstName + d.LastName).ToList();

        // Add IDs.
        distinctDrivers.ForEach(d => d.DriverId = _index++);

        return distinctDrivers;
    }
}

/*using HtmlAgilityPack;
using ScraperApp.Models;
using ScraperApp.Serialization;
using System.Text;

namespace ScraperApp.Scrapers;

internal class DriverScraper
{
    private int _index = 1;
    private int _newDriverWithoutNationalityId = 500;
    private readonly int _startYear = 1950;    

    internal List<Driver> ScrapeDrivers()
    {
        HtmlWeb web = new();
        web.OverrideEncoding = Encoding.UTF8;

        List<string> links = JsonDeserializer.DeserializeCollection<string>("links");

        List<Driver> drivers = ScrapeDriversWithNationality();

        List<Driver> driversWithoutNationality = new();

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
                string scrapedFirstName = row.SelectSingleNode(
                        "./td/span[@class='hide-for-tablet']").InnerText.Trim();

                string scrapedLastName = row.SelectSingleNode(
                    "./td/span[@class='hide-for-mobile']").InnerText.Trim();

                string scrapedFullName = scrapedFirstName + " " + scrapedLastName;

                Driver existingDriver = drivers.FirstOrDefault(d =>
                    d.FirstName + " " + d.LastName == scrapedFullName);

                // If there's no such driver scraped from standings - add them
                if (existingDriver == null)
                {
                    driversWithoutNationality.Add(new Driver()
                    {
                        DriverId = _newDriverWithoutNationalityId++,
                        FirstName = scrapedFirstName,
                        LastName = scrapedLastName
                    });

                    drivers.Add(driversWithoutNationality.Last());
                    Console.WriteLine($"Driver: {scrapedFirstName} {scrapedLastName} added.");
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

        for (int year = _startYear; year <= DateTime.Now.Year; year++)
        {
            document = web.Load(
                $"https://www.formula1.com/en/results.html/{year}/drivers.html");

            foreach (var row in document.DocumentNode.SelectNodes(
                "//table[@class='resultsarchive-table']/tbody/tr"))
            {
                string scrapedFirstName = row.SelectSingleNode(
                    "./td/a/span[@class='hide-for-tablet']")?.InnerText.Trim() ?? string.Empty;

                // If there's no first name on the page - a season hasn't started yet.
                if (string.IsNullOrEmpty(scrapedFirstName))
                {
                    break;
                }

                string scrapedLastName = row.SelectSingleNode(
                    "./td/a/span[@class='hide-for-mobile']").InnerText.Trim();

                // If a driver has already been scraped - move to the next loop iteration.
                if (drivers.Any(d => d.FirstName == scrapedFirstName &&
                                     d.LastName == scrapedLastName))
                {
                    continue;
                }

                string scrapedNationality = row.SelectSingleNode(
                    "./td[@class='dark semi-bold uppercase']").InnerText.Trim();

                drivers.Add(new Driver
                {
                    DriverId = _index++,
                    FirstName = scrapedFirstName,
                    LastName = scrapedLastName,
                    Nationality = scrapedNationality

                });
                Console.WriteLine($"Driver: {scrapedFirstName} {scrapedLastName} added.");
            }
        }

        return drivers;
    }    
}*/