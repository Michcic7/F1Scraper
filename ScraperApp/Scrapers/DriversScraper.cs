using HtmlAgilityPack;
using ScraperApp.Interfaces;
using ScraperApp.Models;
using ScraperApp.Serialization;
using System.Text;

namespace ScraperApp.Scrapers;

internal class DriversScraper : IDriversScraper
{
    private int _index = 1;

    public IEnumerable<Driver> ScrapeDrivers(int year, bool includeIndexing)
    {
        HtmlWeb web = new();
        web.OverrideEncoding = Encoding.UTF8;

        // Get links to races from a requested year.
        IEnumerable<string> links = GetLinks(year);

        List<Driver> scrapedDrivers = new();
        HtmlDocument document = new();

        foreach (var link in links)
        {
            document = web.Load(link);

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

                // Add every driver, even duplicates.
                scrapedDrivers.Add(new Driver()
                {
                    FirstName = scrapedFirstName,
                    LastName = scrapedLastName
                });

                Console.WriteLine($"Driver: {scrapedFirstName} {scrapedLastName} scraped.");
            }
        }

        // Remove duplicates separately for fewer queries - increased performance.
        List<Driver> distinctDrivers = scrapedDrivers
            .DistinctBy(d => d.FirstName + d.LastName)
            .ToList();

        // Add IDs.
        if (includeIndexing)
        {
            distinctDrivers.ForEach(d => d.DriverId = _index++);
        }

        IEnumerable<Driver> drivers = AddNationalityToDrivers(year, distinctDrivers);

        return drivers;
    }

    private static IEnumerable<string> GetLinks(int year)
    {
        IEnumerable<string> allLinks = JsonDeserializer.DeserializeCollection<string>("links");
        IEnumerable<string> links = allLinks
            .Where(l => int.Parse(l[41..45]) == year)
            .Select(l => l)
            .AsEnumerable();
        return links;
    }

    private IEnumerable<Driver> AddNationalityToDrivers(int year, List<Driver> drivers)
    {
        HtmlWeb web = new();
        web.OverrideEncoding = Encoding.UTF8;

        HtmlDocument document = web.Load(
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

            Driver? scrapedDriver = drivers.FirstOrDefault(d =>
                d.FirstName == scrapedFirstName && d.LastName == scrapedLastName);

            if (scrapedDriver == null)
            {
                continue;
            }

            string scrapedNationality = row.SelectSingleNode(
                "./td[@class='dark semi-bold uppercase']").InnerText.Trim();

            scrapedDriver.Nationality = scrapedNationality;

            Console.WriteLine($"Nationality: {scrapedNationality} added to {scrapedFirstName} {scrapedLastName}.");
        }

        return drivers;
    }

    //internal List<Driver> ScrapeDrivers()
    //{
    //    List<Driver> drivers = ScrapeDriversWithoutNationality();

    //    HtmlWeb web = new();
    //    web.OverrideEncoding = Encoding.UTF8;
    //    HtmlDocument document = new();

    //    for (int year = _startYear; year <= DateTime.Now.Year; year++)
    //    {
    //        document = web.Load(
    //            $"https://www.formula1.com/en/results.html/{year}/drivers.html");

    //        foreach (var row in document.DocumentNode.SelectNodes(
    //            "//table[@class='resultsarchive-table']/tbody/tr"))
    //        {
    //            string scrapedFirstName = row.SelectSingleNode(
    //                "./td/a/span[@class='hide-for-tablet']")?.InnerText.Trim() ?? string.Empty;

    //            // If there's no first name on the page - a season hasn't started yet.
    //            if (string.IsNullOrEmpty(scrapedFirstName))
    //            {
    //                break;
    //            }

    //            string scrapedLastName = row.SelectSingleNode(
    //                "./td/a/span[@class='hide-for-mobile']").InnerText.Trim();

    //            Driver existingDriver = drivers.FirstOrDefault(d =>
    //                d.FirstName == scrapedFirstName && d.LastName == scrapedLastName);

    //            if (existingDriver == null)
    //            {
    //                continue;
    //            }

    //            if (existingDriver.Nationality == null)
    //            {
    //                string scrapedNationality = row.SelectSingleNode(
    //                    "./td[@class='dark semi-bold uppercase']").InnerText.Trim();

    //                existingDriver.Nationality = scrapedNationality;

    //                Console.WriteLine($"Nationality: {scrapedNationality} added to {scrapedFirstName} {scrapedLastName}.");
    //            }
    //        }
    //    }

    //    return drivers;
    //}

    //private List<Driver> ScrapeDriversWithoutNationality()
    //{
    //    HtmlWeb web = new();
    //    web.OverrideEncoding = Encoding.UTF8;

    //    List<string> raceResultsLinks = JsonDeserializer.DeserializeCollection<string>("links");

    //    List<Driver> drivers = new();

    //    foreach (var link in raceResultsLinks)
    //    {
    //        HtmlDocument document = web.Load(link);

    //        var rows = document.DocumentNode.SelectNodes(
    //            "//table[@class='resultsarchive-table']/tbody/tr") ?? null;

    //        // If there's no row node on the page - the race hasn't taken place yet.
    //        if (rows == null)
    //        {
    //            break;
    //        }

    //        foreach (var row in rows)
    //        {
    //            string scrapedFirstName = row.SelectSingleNode(
    //                    "./td/span[@class='hide-for-tablet']").InnerText.Trim();

    //            string scrapedLastName = row.SelectSingleNode(
    //                "./td/span[@class='hide-for-mobile']").InnerText.Trim();

    //            // Add everything, even duplicates.
    //            drivers.Add(new Driver()
    //            {
    //                FirstName = scrapedFirstName,
    //                LastName = scrapedLastName
    //            });

    //            Console.WriteLine($"Driver: {scrapedFirstName} {scrapedLastName} scraped.");
    //        }
    //    }

    //    // Remove duplicates separately for fewer queries - increased performance.
    //    List<Driver> distinctDrivers = drivers.DistinctBy(d => d.FirstName + d.LastName).ToList();

    //    // Add IDs.
    //    distinctDrivers.ForEach(d => d.DriverId = _index++);

    //    return distinctDrivers;
    //}
}
