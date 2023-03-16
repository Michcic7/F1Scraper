using HtmlAgilityPack;
using ScraperApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ScraperApp.Scrapers;

internal class DriverScraper
{
    private int index = 1;

    public List<Driver> ScrapeDrivers(int year, List<Team> teams)
    {
        HtmlWeb web = new();
        web.OverrideEncoding = Encoding.UTF8;
        HtmlDocument document = web.Load($"https://www.formula1.com/en/results.html/{year}/drivers.html");

        //List<Team> distinctTeams = new();

        //if (year > 1957)
        //{
        //    TeamScraper teamScraper = new();
        //    List<Team> teams = teamScraper.ScrapeTeams(year); 
        //    distinctTeams = teams.DistinctBy(t => t.Name).ToList();
        //}
        
        List<Driver> drivers = new();

        foreach (var row in document.DocumentNode.SelectNodes(
            "//table[@class='resultsarchive-table']/tbody/tr"))
        {
            Driver driver = new();

            try
            {
                var firstName = row.SelectSingleNode(
                        "./td/a/span[@class='hide-for-tablet']").InnerText;
                var lastName = row.SelectSingleNode(
                    "./td/a/span[@class='hide-for-mobile']").InnerText;
                var nationality = row.SelectSingleNode(
                    "./td[@class='dark semi-bold uppercase']").InnerText;
                var teamString = row.SelectSingleNode(
                    "./td/a[@class='grey semi-bold uppercase ArchiveLink']").InnerText;

                driver.Id = index++;
                driver.FirstName = WebUtility.HtmlDecode(firstName);
                driver.LastName = WebUtility.HtmlDecode(lastName);
                driver.Nationality = nationality;


                driver.Team.AddRange(teams);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            drivers.Add(driver);
        }

        return drivers;
    }
}
