namespace ScraperApp.Interfaces;

internal interface ILinksScraper
{
    IEnumerable<string> ScrapeLinks();
}