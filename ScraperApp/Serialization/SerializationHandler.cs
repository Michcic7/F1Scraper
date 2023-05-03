using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace ScraperApp.Serialization;

internal class SerializationHandler
{
    private readonly Serializer _serializer = new();

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

        WriteLine("Team standings scraped and serialized.");

        bool IsYearValid(int input)
        {
            if (input >= 1950 && input <= 2023)
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

        WriteLine("Team standings scraped and serialized.");

        bool IsYearValid(int input)
        {
            if (input >= 1958 && input <= 2023)
            {
                return true;
            }

            return false;
        }
    }
}
