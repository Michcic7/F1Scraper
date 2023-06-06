using ScraperApp.Models;

namespace ScraperApp.Interfaces;

internal interface IDriversScraper
{
    IEnumerable<Driver> ScrapeDrivers(int year, bool includeIndexing);
}
