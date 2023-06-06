using Microsoft.Extensions.DependencyInjection;
using ScraperApp;
using ScraperApp.Interfaces;
using ScraperApp.Scrapers;
using ScraperApp.Serialization;
using ScraperApp.Serializers;
using System.Diagnostics;
using static System.Console;

ServiceCollection services = new();

services
    .AddTransient<ILinksScraper, LinksScraper>()
    .AddTransient<ILinksSerializer, LinksSerializer>()
    .AddTransient<IDriversScraper, DriversScraper>()
    .AddTransient<IDriversSerializer, DriversSerializer>();

ServiceProvider serviceProvider = services.BuildServiceProvider();

string jsonFolderPath = DirectoryHelper.GetJsonFolderPath();

if (!File.Exists(Path.Combine(jsonFolderPath, "links.json")))
{
    serviceProvider.GetService<ILinksSerializer>()!.SerializeLinks();
}

serviceProvider.GetService<IDriversSerializer>()!.SerializeDrivers(2022, false);


//SerializationHandler serializationHandler = new();
//int input;
//bool toQuit = false;
//int numberOfOptions = 8;

//do
//{
//    do
//    {
//        WriteLine("What would you like to scrape? Enter the number.");
//        WriteLine("1. Drivers");
//        WriteLine("2. Teams");
//        WriteLine("3. Circuits");
//        WriteLine("4. Driver Standings");
//        WriteLine("5. Team Standings");
//        WriteLine("6. Race Results");
//        WriteLine("7. Nationality Three-letter Codes");
//        WriteLine("8. All Data");
//        WriteLine("Note: In order to have correct relationships between objects, " +
//                  "use 1 - 3 options to scrape the corresponding data " +
//                  "before proceeding to 4 - 6 options");
//        WriteLine();
//        Write("Scrape data about: ");

//        bool isInputInt = int.TryParse(ReadLine(), out input);

//        if (!IsInputValid(input) || !isInputInt)
//        {
//            WriteLine($"Error: Enter an integer from 1 to {numberOfOptions}.");
//            WriteLine();
//        }
//    }
//    while (!IsInputValid(input));

//    Stopwatch stopwatch = new();

//    switch (input)
//    {
//        case 1:
//            stopwatch.Start();
//            serializationHandler.HandleDrivers();
//            stopwatch.Stop();
//            PrintElapsedTime(stopwatch);
//            break;

//        case 2:
//            stopwatch.Start();
//            serializationHandler.HandleTeams();
//            stopwatch.Stop();
//            PrintElapsedTime(stopwatch);
//            break;

//        case 3:
//            stopwatch.Start();
//            serializationHandler.HandleCircuits();
//            stopwatch.Stop();
//            PrintElapsedTime(stopwatch);
//            break;

//        case 4:
//            stopwatch.Start();
//            serializationHandler.HandleDriverStandings();
//            stopwatch.Stop();
//            PrintElapsedTime(stopwatch);
//            break;

//        case 5:
//            stopwatch.Start();
//            serializationHandler.HandleTeamStandings();
//            stopwatch.Stop();
//            PrintElapsedTime(stopwatch);
//            break;

//        case 6:
//            stopwatch.Start();
//            serializationHandler.HandleRaceResults();
//            stopwatch.Stop();
//            PrintElapsedTime(stopwatch);
//            break;

//        case 7:
//            stopwatch.Start();
//            serializationHandler.HandleNationalityCodes();
//            stopwatch.Stop();
//            PrintElapsedTime(stopwatch);
//            break;

//        case 8:
//            stopwatch.Start();
//            serializationHandler.HandleAllData();
//            stopwatch.Stop();
//            PrintElapsedTime(stopwatch);
//            break;
//    }

//    WriteLine("Do you want to continue? (y/n)");

//    string quitInput = ReadLine();

//    if (quitInput.ToLower() == "y")
//    {
//        toQuit = false;
//        WriteLine();
//    }
//    else if (quitInput.ToLower() == "n")
//    {
//        toQuit = true;
//        WriteLine("Press any key to quit.");
//    }
//}
//while (!toQuit);

//ReadKey();

//bool IsInputValid(int input)
//{
//    if (input > 0 && input <= numberOfOptions)
//    {
//        return true;
//    }

//    return false;
//}

//void PrintElapsedTime(Stopwatch stopwatch)
//{
//    WriteLine($"It took {stopwatch.Elapsed.Minutes:00}:{stopwatch.Elapsed.Seconds:00}:{stopwatch.Elapsed.Milliseconds:00}.");
//}