using HtmlAgilityPack;
using Newtonsoft.Json;
using System;

Console.Write("Enter a year: ");
string year = Console.ReadLine();
Console.WriteLine();

var web = new HtmlWeb();
var doc = web.Load($"https://www.formula1.com/en/results.html/{year}/drivers.html");

var rows = doc.DocumentNode.SelectNodes(
    "//table[@class='resultsarchive-table']/tbody/tr");

foreach (var item in rows)
{
    var pos = item.SelectSingleNode(
        "./td[@class='dark']");
    var driverF = item.SelectSingleNode(
        "./td/a/span[@class='hide-for-tablet']");
    var driverL = item.SelectSingleNode(
        "./td/a/span[@class='hide-for-mobile']");
    var nationality = item.SelectSingleNode(
        "./td[@class='dark semi-bold uppercase']");
    var team = item.SelectSingleNode(
        "./td/a[@class='grey semi-bold uppercase ArchiveLink']");
    var points = item.SelectSingleNode(
        "./td[@class='dark bold']");

    Console.WriteLine(pos.InnerText + ".");
    Console.WriteLine(driverF.InnerText + " " + driverL.InnerText);
    Console.WriteLine(nationality.InnerText);
    Console.WriteLine(team.InnerText);
    Console.WriteLine(points.InnerText + " points");
    Console.WriteLine();

    //Console.WriteLine(JsonConvert.SerializeObject(_model, Formatting.Indented));
}