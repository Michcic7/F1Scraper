using HtmlAgilityPack;
using ScraperApp.Json;
using ScraperApp.Models;
using System.Globalization;
using System.Text;

namespace ScraperApp.Scrapers;

internal class RaceResultScraper
{
    private int _index = 1;
    private int _newDriverId = 500;

    internal List<RaceResult> ScrapeRaceResults(List<string> links)
    {
        List<Driver> drivers = JsonDeserializer.Deserialize<Driver>("drivers");
        List<Team> teams = JsonDeserializer.Deserialize<Team>("teams");
        List<Circuit> circuits = JsonDeserializer.Deserialize<Circuit>("circuits");

        HtmlWeb web = new();
        web.OverrideEncoding = Encoding.UTF8;

        List<RaceResult> raceResults = new();
        
        bool addedNewDriver = false;
        List<Driver> driversNotPresentInJson = new();
        int newDriverIndexInList = 0;

        foreach (var link in links)
        {
            HtmlDocument document = web.Load(link);

            string scrapedCircuitFullName = document.DocumentNode.SelectSingleNode(
                "/html/body/div[1]/main/article/div/div[2]/div[2]/div[1]/p" +
                "/span[@class='circuit-info']").InnerText;

            // Split a circuit's name and location.
            string[] scrapedParts = scrapedCircuitFullName.Split(',');
            string scrapedCircuitName = scrapedParts[0].Trim();

            Circuit existingCircuit = circuits.FirstOrDefault(c =>
                c.Name == scrapedCircuitName);

            if (existingCircuit == null)
            {
                Console.WriteLine($"Circuit: {scrapedCircuitName} not found in circuits.json");
            }

            string yearString = link[41..45];

            if (int.TryParse(yearString, out int year))
            {
            }
            else
            {
                Console.WriteLine($"Year: {yearString} is invalid");
            }

            string scrapedDateString = document.DocumentNode.SelectSingleNode(
                    "//div[@class='resultsarchive-content-header group']" +
                    "/p[@class='date']/span[@class='full-date']").InnerText;

            if (DateOnly.TryParseExact(scrapedDateString, "dd MMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly date))
            {
            }
            else
            {
                Console.WriteLine($"Something's wrong: {link} \n" +
                    $"Date: {scrapedDateString}");
            }

            // HTML nodes from the race results page.
            HtmlNodeCollection rows = document.DocumentNode.SelectNodes(
                "//table[@class='resultsarchive-table']/tbody/tr") ?? null;

            if (rows == null)
            {
                break;
            }

            Console.WriteLine();
            Console.WriteLine($"Year: {year}, {scrapedCircuitFullName}:");

            foreach (var row in rows)
            {                
                string scrapedPositionString = row.SelectSingleNode(
                    "./td[@class='dark']").InnerText;

                if (int.TryParse(scrapedPositionString, out int position))
                {
                }
                else
                {
                    position = 0;
                }

                string scrapedPointsString = row.SelectSingleNode(
                        "./td[@class='bold']").InnerText;

                if (float.TryParse(scrapedPointsString, CultureInfo.InvariantCulture, out float points))
                {
                }
                else
                {
                    points = 0;
                }

                string scrapedDriverFirstName = row.SelectSingleNode(
                        "./td/span[@class='hide-for-tablet']").InnerText.Trim();
                string scrapedDriverLastName = row.SelectSingleNode(
                    "./td/span[@class='hide-for-mobile']").InnerText.Trim();

                string scrapedDriverFullName = scrapedDriverFirstName + " " + scrapedDriverLastName;

                Driver existingDriver = drivers.FirstOrDefault(d =>
                    d.FirstName + " " + d.LastName == scrapedDriverFullName);

                if (existingDriver == null)
                {
                    throw new NullReferenceException($"Driver: {scrapedDriverFullName} not found in json");
                    
                    //Console.WriteLine($"Driver: {scrapedDriverFullName} doesn't exist");

                    //driversNotPresentInJson.Add(new Driver()
                    //{
                    //    DriverId = _newDriverId++,
                    //    FirstName = scrapedDriverFirstName,
                    //    LastName = scrapedDriverLastName
                    //});

                    //drivers.Add(driversNotPresentInJson.Last());

                    //addedNewDriver = true;
                }

                string scrapedTeamName = row.SelectSingleNode(
                    "./td[@class='semi-bold uppercase hide-for-tablet']").InnerText.Trim();

                Team existingTeam = teams.FirstOrDefault(t => t.Name == scrapedTeamName);

                if (existingTeam == null)
                {
                    Console.WriteLine(
                        $"Year: {year} {scrapedTeamName} is missing in teams.json");
                }

                string scrapedLapsString = row.SelectSingleNode(
                    "./td[@class='bold hide-for-mobile']").InnerText;

                if (int.TryParse(scrapedLapsString, out int laps))
                {
                }
                else
                {
                    laps = 0;
                }

                // There are two nodes with the same class, select the second.
                HtmlNodeCollection twoSameNodes = row.SelectNodes("./td[@class='dark bold']");
                string scrapedTime = twoSameNodes[1].InnerText;

                raceResults.Add(new RaceResult
                {
                    RaceResultId = _index++,
                    Year = year,
                    Position = position,
                    Circuit = existingCircuit,
                    Date = date,
                    Driver = existingDriver,
                    Team = existingTeam,
                    Laps = laps,
                    Time = scrapedTime,
                    Points = points,
                });

                Console.WriteLine($"\t{position}. {scrapedDriverFirstName} {scrapedDriverFullName}, {points}pts.");
            }
        }

        return raceResults;
    }
}
