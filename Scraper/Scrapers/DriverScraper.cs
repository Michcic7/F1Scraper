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
    private List<Driver> _drivers = new();
    
    private int index = 1;

    public List<Driver> ScrapeDrivers(int year, List<Team> teams)
    {
        HtmlWeb web = new();
        web.OverrideEncoding = Encoding.UTF8;
        HtmlDocument document = web.Load($"https://www.formula1.com/en/results.html/{year}/drivers.html");
                
        List<Team> teamsToAdd = new();

        foreach (var row in document.DocumentNode.SelectNodes(
            "//table[@class='resultsarchive-table']/tbody/tr"))
        {
            Driver driver = new();

            try
            {
                string firstName = row.SelectSingleNode(
                        "./td/a/span[@class='hide-for-tablet']").InnerText;
                string lastName = row.SelectSingleNode(
                    "./td/a/span[@class='hide-for-mobile']").InnerText;
                string teamString = row.SelectSingleNode(
                        "./td/a[@class='grey semi-bold uppercase ArchiveLink']").InnerText;

                if (_drivers.Any(d => d.FirstName == firstName && d.LastName == lastName))
                {
                    driver = _drivers.FirstOrDefault(d => d.FirstName == firstName && d.LastName == lastName);

                    teamsToAdd = teams.Where(t => t.Name == teamString).ToList();

                    List<Team> newElements = teamsToAdd.Except(_drivers.SelectMany(d => d.Teams)).ToList();

                    if (newElements.Any())
                    {
                        driver.Teams.AddRange(newElements);
                    }
;
                    //driver.Teams.AddRange(teamsToAdd);                    
                }
                else
                {
                    string nationality = row.SelectSingleNode(
                    "./td[@class='dark semi-bold uppercase']").InnerText;

                    driver.Id = index++;
                    driver.FirstName = WebUtility.HtmlDecode(firstName);
                    driver.LastName = WebUtility.HtmlDecode(lastName);
                    driver.Teams = new List<Team>();
                    driver.Nationality = nationality;
                    driver.Teams.Add(teams.FirstOrDefault(t => t.Name == teamString));
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            _drivers.Add(driver);
        }

        return _drivers;
    }
}

//public List<Driver> ScrapeDrivers(int year, List<Team> teams)
//{
//    HtmlWeb web = new();
//    web.OverrideEncoding = Encoding.UTF8;
//    HtmlDocument document = web.Load($"https://www.formula1.com/en/results.html/{year}/drivers.html");

//    List<Driver> drivers = new();
//    List<Team> teamsToAdd = new();

//    foreach (var row in document.DocumentNode.SelectNodes(
//        "//table[@class='resultsarchive-table']/tbody/tr"))
//    {
//        Driver driver = new();

//        try
//        {
//            var firstName = row.SelectSingleNode(
//                    "./td/a/span[@class='hide-for-tablet']").InnerText;
//            var lastName = row.SelectSingleNode(
//                "./td/a/span[@class='hide-for-mobile']").InnerText;
//            var nationality = row.SelectSingleNode(
//                "./td[@class='dark semi-bold uppercase']").InnerText;
//            var teamString = row.SelectSingleNode(
//                "./td/a[@class='grey semi-bold uppercase ArchiveLink']").InnerText;

//            driver.Id = index++;
//            driver.FirstName = WebUtility.HtmlDecode(firstName);
//            driver.LastName = WebUtility.HtmlDecode(lastName);
//            driver.Nationality = nationality;
//            driver.Team = new List<Team>();

//            teamsToAdd = teams.Where(t => t.Name == teamString).ToList();

//            driver.Team.AddRange(teamsToAdd);
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine(ex.Message);
//        }

//        drivers.Add(driver);
//    }

//    return drivers;
//}
