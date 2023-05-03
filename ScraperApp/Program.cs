using ScraperApp.Serialization;
using static System.Console;

SerializationHandler serializationHandler = new();
int input;

do
{
    WriteLine("What would you like to scrape? Enter the number.");
    WriteLine("1. Drivers");
    WriteLine("2. Teams");
    WriteLine("3. Circuits");
    WriteLine("4. Driver Standings");
    WriteLine("5. Team Standings");
    WriteLine("6. Race Results");
    Write("Scrape data about: ");

    bool isInputInt = int.TryParse(ReadLine(), out input);

    if (!IsInputValid(input) || !isInputInt)
    {
        WriteLine("Error: Enter an integer from 1 to 6.");
        WriteLine();
    }
}
while (!IsInputValid(input));

switch (input)
{
    case 1:
        //serializer.SerializeDrivers();
        WriteLine("Drivers scraped and serialized.");
        break;

    case 2:
        //serializer.SerializeTeams();
        WriteLine("Teams scraped and serialized.");
        break;

    case 3:
        //serializer.SerializeCircuits();
        WriteLine("Circuits scraped and serialized.");
        break;

    case 4:
        serializationHandler.HandleDriverStandings();
        break;

    case 5:
        serializationHandler.HandleTeamStandings();
        break;

    case 6:
        WriteLine("Which years do you want to get?");
        Write("Enter the year from 1950 to 2023");
        //serializer.SerializeRaceResults();
        WriteLine("Race results scraped and serialized.");
        break;
}

WriteLine("Press any key to exit.");
ReadKey();

bool IsInputValid(int input)
{
    if (input > 0 && input <= 6)
    {
        return true;
    }

    return false;
}
