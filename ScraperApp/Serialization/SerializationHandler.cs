using static System.Console;

namespace ScraperApp.Serialization;

internal class SerializationHandler
{
    private readonly Serializer _serializer = new();

    private readonly int _startYearDrivers = 1950;
    private readonly int _startYearTeams = 1958;
    private readonly int _currentYear = DateTime.Now.Year;

    internal void HandleDrivers()
    {
        WriteLine();
        WriteLine("Scraping drivers.");
        WriteLine("Note: Driver who didn't participate in Drivers' Championship " +
                  "don't have their nationality included.");
        WriteLine("Press any key to start.");
        ReadKey();

        _serializer.SerializeDrivers();

        WriteLine("Drivers have been scraped and serialised.");
    }

    internal void HandleTeams()
    {
        WriteLine();
        WriteLine("Scraping teams.");
        WriteLine("Press any key to start.");
        ReadKey();

        _serializer.SerializeTeams();

        WriteLine("Teams have been scraped and serialised.");
    }

    internal void HandleCircuits()
    {
        WriteLine();
        WriteLine("Scraping circuits.");
        WriteLine("Note: Some of the circuits had their location changed on " +
                  "the official Formula 1 website throughout years, so the same " +
                  "circuit might appear multiple times with a slight variation in " +
                  "name or location.");
        WriteLine("Press any key to start.");
        ReadKey();

        _serializer.SerializeCircuits();

        WriteLine("Circuits have been scraped and serialised.");
    }

    internal void HandleDriverStandings()
    {
        int startYear;
        int endYear;

        WriteLine();
        WriteLine("Scraping team standings.");
        WriteLine("Which years do you want to get? [1950 - 2023]");

        do
        {
            Write("From year: ");
            bool isStartYearInt = int.TryParse(ReadLine(), out startYear);
            if (!isStartYearInt || !IsYearValid(startYear))
            {
                WriteLine("Error: Input an integer from 1950 to 2023.");
                WriteLine();
            }
        }
        while (!IsYearValid(startYear));

        do
        {
            Write("Up to: ");
            bool isEndYearInt = int.TryParse(ReadLine(), out endYear);
            if (!isEndYearInt || !IsYearValid(endYear))
            {
                WriteLine("Error: Enter an integer from 1950 to 2023.");
                WriteLine();
            }
        }
        while (!IsYearValid(endYear));

        _serializer.SerializeDriverStandings(startYear, endYear);

        WriteLine("Team standings have been scraped and serialised.");

        bool IsYearValid(int input)
        {
            if (input >= _startYearDrivers && input <= _currentYear)
            {
                return true;
            }

            return false;
        }
    }

    internal void HandleTeamStandings()
    {
        int startYear;
        int endYear;

        WriteLine();
        WriteLine("Scraping team standings.");
        WriteLine("Which years do you want to get? [1958 - 2023]");

        do
        {
            Write("From year: ");
            bool isStartYearInt = int.TryParse(ReadLine(), out startYear);
            if (!isStartYearInt || !IsYearValid(startYear))
            {
                WriteLine("Error: Input an integer from 1958 to 2023.");
                WriteLine();
            }
        }
        while (!IsYearValid(startYear));

        do
        {
            Write("Up to: ");
            bool isEndYearInt = int.TryParse(ReadLine(), out endYear);
            if (!isEndYearInt || !IsYearValid(endYear))
            {
                WriteLine("Error: Enter an integer from 1958 to 2023.");
                WriteLine();
            }
        }
        while (!IsYearValid(endYear));

        _serializer.SerializeTeamStandings(startYear, endYear);

        WriteLine("Team standings have been scraped and serialised.");

        bool IsYearValid(int input)
        {
            if (input >= _startYearTeams && input <= _currentYear)
            {
                return true;
            }

            return false;
        }
    }

    internal void HandleRaceResults()
    {
        int startYear;
        int endYear;

        WriteLine();
        WriteLine("Scraping race results.");
        WriteLine("Which years do you want to get? [1950 - 2023]");

        do
        {
            Write("From year: ");
            bool isStartYearInt = int.TryParse(ReadLine(), out startYear);
            if (!isStartYearInt || !IsYearValid(startYear))
            {
                WriteLine("Error: Input an integer from 1950 to 2023.");
                WriteLine();
            }
        }
        while (!IsYearValid(startYear));

        do
        {
            Write("Up to: ");
            bool isEndYearInt = int.TryParse(ReadLine(), out endYear);
            if (!isEndYearInt || !IsYearValid(endYear))
            {
                WriteLine("Error: Enter an integer from 1950 to 2023.");
                WriteLine();
            }
        }
        while (!IsYearValid(endYear));

        _serializer.SerializeRaceResults(startYear, endYear);

        WriteLine("Race results have been scraped and serialised.");

        bool IsYearValid(int input)
        {
            if (input >= _startYearDrivers && input <= _currentYear)
            {
                return true;
            }

            return false;
        }
    }

    internal void HandleAllData()
    {
        WriteLine();
        WriteLine("Scraping all data.");
        WriteLine("Note: Driver who didn't participate in Drivers' Championship " +
                  "don't have their nationality included.");
        WriteLine();
        WriteLine("Note: Some of the circuits had their location changed on " +
                  "the official Formula 1 website throughout years, so the same " +
                  "circuit might appear multiple times with a slight variation in " +
                  "name or location.");
        WriteLine();

        bool isValidInput = false;
        bool includeCodes = false;

        do
        {
            WriteLine("Do you want to include nationality codes? (y/n)");

            string input = ReadLine()!;
            
            if (input.ToLower() == "y")
            {
                includeCodes = true;
                isValidInput = true;
            }
            else if (input.ToLower() == "n")
            {
                includeCodes = false;
                isValidInput = true;
            }
        } while (!isValidInput);

        WriteLine("Press any key to start.");

        ReadKey();

        _serializer.SerializeDrivers();
        _serializer.SerializeTeams();
        _serializer.SerializeCircuits();
        _serializer.SerializeDriverStandings(_startYearDrivers, DateTime.Now.Year);
        _serializer.SerializeTeamStandings(_startYearTeams, DateTime.Now.Year);
        _serializer.SerializeRaceResults(_startYearDrivers, DateTime.Now.Year);

        if (includeCodes)
        {
            _serializer.SerializeNationalityCodes();
        }

        WriteLine("All data have been scraped and serialised.");
    }

    internal void HandleNationalityCodes()
    {
        WriteLine();
        WriteLine("Scraping nationality codes");

        WriteLine("Press any key to start.");

        ReadKey();

        _serializer.SerializeNationalityCodes();
        WriteLine("Nationality codes have been scraped and serialised.");
    }
}
