using HtmlAgilityPack;
using ScraperApp.Models;
using System.Text;

namespace ScraperApp.Scrapers;

internal class NationalityCodeScraper
{
    private int _index = 1;

    internal List<NationalityCode> ScrapeNationalityCodes()
    {
        HtmlWeb web = new();
        web.OverrideEncoding = Encoding.UTF8;
        HtmlDocument document = web.Load("https://www.iban.com/country-codes");

        List<NationalityCode> codes = new();

        foreach (var row in document.DocumentNode.SelectNodes(
            "//table[@class='table table-bordered downloads tablesorter']/tbody/tr"))
        {
            string scrapedCode = row.SelectSingleNode("./td[3]").InnerText.Trim();

            codes.Add(new NationalityCode
            {
                NationalityCodeId = _index++,
                Country = scrapedCode
            });

            Console.WriteLine($"{scrapedCode} scraped.");
        }

        return codes;
    }
}
