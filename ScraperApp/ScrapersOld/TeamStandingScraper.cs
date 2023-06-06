using HtmlAgilityPack;
using ScraperApp.Models;
using ScraperApp.Serialization;
using System.Globalization;
using System.Text;

namespace ScraperApp.Scrapers;

internal class TeamStandingScraper
{
    private int _index = 1;

    internal List<TeamStanding> ScrapeTeamStandings(int startYear, int endYear)
    {
        List<Team> teams = JsonDeserializer.DeserializeCollection<Team>("teams");
        
        HtmlWeb web = new();
        web.OverrideEncoding = Encoding.UTF8;

        List<TeamStanding> teamStandings = new();

        for (int year = startYear; year <= endYear; year++)
        {
            HtmlDocument document = web.Load(
                $"https://www.formula1.com/en/results.html/{year}/team.html");

            Console.WriteLine($"Year: {year}");

            foreach (var row in document.DocumentNode.SelectNodes(
                "//table[@class='resultsarchive-table']/tbody/tr"))
            {
                string scrapedTeamName = row.SelectSingleNode(
                    "./td/a[@class='dark bold uppercase ArchiveLink']")?.InnerText.Trim() ?? string.Empty;

                if (string.IsNullOrEmpty(scrapedTeamName))
                {
                    break;
                }

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

                teamStandings.Add(new TeamStanding
                {
                    TeamStandingId = _index++,
                    Position = position,
                    Team = existingTeam,
                    Points = points,
                    Year = year
                });
                Console.WriteLine($"\t{position}. {scrapedTeamName}, {points}pts.");
            }
        }    
        
        return teamStandings;
    }
}