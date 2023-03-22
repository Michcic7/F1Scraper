using System.Data;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using ScraperApp.Models;

namespace ScraperApp.Old;

internal class DriverStandingsScraper
{
    private int index = 1;

    public List<Driver> GetDriverStandings(int year)
    {
        HtmlWeb web = new();
        web.OverrideEncoding = Encoding.UTF8;
        HtmlDocument document = web.Load($"https://www.formula1.com/en/results.html/{year}/drivers.html");

        List<Driver> drivers = new();

        foreach (var row in document.DocumentNode.SelectNodes(
            "//table[@class='resultsarchive-table']/tbody/tr"))
        {
            Driver driver = new();

            try
            {
                var position = row.SelectSingleNode(
                "./td[@class='dark']").InnerText;
                var firstName = row.SelectSingleNode(
                    "./td/a/span[@class='hide-for-tablet']").InnerText;
                var lastName = row.SelectSingleNode(
                    "./td/a/span[@class='hide-for-mobile']").InnerText;
                var nationality = row.SelectSingleNode(
                    "./td[@class='dark semi-bold uppercase']").InnerText;

                driver.Id = index++;
                driver.FirstName = WebUtility.HtmlDecode(firstName);
                driver.LastName = WebUtility.HtmlDecode(lastName);
                driver.Nationality = nationality;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            drivers.Add(driver);
        }

        return drivers;
    }
}

/*
 *public List<Driver> GetDriverStandings(int year)
    {
        HtmlWeb web = new();
        web.OverrideEncoding = Encoding.UTF8;
        HtmlDocument document = web.Load($"https://www.formula1.com/en/results.html/{year}/drivers.html");

        HtmlNodeCollection rows = document.DocumentNode.SelectNodes(
            "//table[@class='resultsarchive-table']/tbody/tr");

        List<Driver> drivers = new();

        foreach (var item in rows)
        {
            Driver driver = new();

            try
            {
                var position = item.SelectSingleNode(
                "./td[@class='dark']").InnerText;
                var firstName = item.SelectSingleNode(
                    "./td/a/span[@class='hide-for-tablet']").InnerText;
                var lastName = item.SelectSingleNode(
                    "./td/a/span[@class='hide-for-mobile']").InnerText;
                var nationality = item.SelectSingleNode(
                    "./td[@class='dark semi-bold uppercase']").InnerText;
                var team = item.SelectSingleNode(
                    "./td/a[@class='grey semi-bold uppercase ArchiveLink']").InnerText;
                var points = item.SelectSingleNode(
                    "./td[@class='dark bold']").InnerText;

                driver.DriverId = index++;
                driver.StandingsYearId = year;
                driver.Position = position;
                driver.FirstName = WebUtility.HtmlDecode(firstName);
                driver.LastName = WebUtility.HtmlDecode(lastName);
				driver.Nationality = nationality;
                driver.Team = team;
                if (points == "null")
                    points = "0";
                driver.Points = float.Parse(points);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            drivers.Add(driver);
        }

        return drivers;
    }
 *
 */