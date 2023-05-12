using HtmlAgilityPack;
using ScraperApp.Models;
using ScraperApp.Serialization;
using System.Globalization;
using System.Text;

namespace ScraperApp.Scrapers;

internal class DriverStandingScraper
{
    private int _index = 1;

    internal List<DriverStanding> ScrapeDriverStandings(int startYear, int endYear)
    {
        List<Driver> drivers = JsonDeserializer.DeserializeCollection<Driver>("drivers");
        List<Team> teams = JsonDeserializer.DeserializeCollection<Team>("teams");

        HtmlWeb web = new();
        web.OverrideEncoding = Encoding.UTF8;

        List<DriverStanding> driverStandings = new();
        
        for (int year = startYear; year <= endYear; year++)
        {
            HtmlDocument document = web.Load(
                $"https://www.formula1.com/en/results.html/{year}/drivers.html");

            Console.WriteLine($"Year: {year}");

            foreach (var row in document.DocumentNode.SelectNodes(
                "//table[@class='resultsarchive-table']/tbody/tr"))
            {
                string scrapedFirstName = row.SelectSingleNode(
                        "./td/a/span[@class='hide-for-tablet']").InnerText.Trim();
                string scrapedLastName = row.SelectSingleNode(
                    "./td/a/span[@class='hide-for-mobile']").InnerText.Trim();

                string scrapedFullName = scrapedFirstName + " " + scrapedLastName;

                Driver existingDriver = drivers.FirstOrDefault(d =>
                    d.FirstName + " " + d.LastName == scrapedFullName);

                if (existingDriver == null)
                {
                    Console.WriteLine($"Driver: {scrapedFullName} is missing in drivers.json");
                    break;
                }

                string scrapedTeamName = row.SelectSingleNode(
                    "./td/a[@class='grey semi-bold uppercase ArchiveLink']").InnerText.Trim();

                Team existingTeam = teams.FirstOrDefault(t => t.Name == scrapedTeamName);

                if (existingTeam == null)
                {
                    Console.WriteLine($"Year: {year} {scrapedTeamName} is missing in teams.json");
                    break;
                }

                string scrapedPositionString = row.SelectSingleNode(
                    "./td[@class='dark']").InnerText;

                if (!int.TryParse(scrapedPositionString, out int position))
                {
                    position = 0;
                }

                string scrapedPointsString = row.SelectSingleNode(
                        "./td[@class='dark bold']").InnerText;

                if (!float.TryParse(scrapedPointsString, CultureInfo.InvariantCulture, out float points))
                {
                    points = 0;
                }

                driverStandings.Add(new DriverStanding()
                {
                    DriverStandingId = _index++,
                    Position = position,
                    Driver = existingDriver,
                    Team = existingTeam,
                    Points = points,
                    Year = year
                });

                Console.WriteLine($"\t{position}. {scrapedFullName}, {points}pts.");
            }
        }

        return driverStandings;
    }
}
