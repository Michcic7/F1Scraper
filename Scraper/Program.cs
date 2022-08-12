using HtmlAgilityPack;
using Newtonsoft.Json;
using Scraper.Services;

DriverService _service = new DriverService();

var infos = _service.GetDriverInfo("https://en.wikipedia.org/wiki/Mick_Schumacher");

Console.WriteLine(infos);


//var url = "https://en.wikipedia.org/wiki/Fernando_Alonso";
//var web = new HtmlWeb();
//var doc = web.Load(url);

//var rows = doc.DocumentNode.SelectNodes(
//    "//table[@class='infobox biography vcard']//tr");

//var thInfo = doc.DocumentNode.SelectSingleNode(
//    "//table[@class='infobox biography vcard']//tr//th[@class='infobox-header']");

//foreach (var item in rows)
//{
//    var th = item.SelectSingleNode(".//th");
//    var td = item.SelectSingleNode(".//td");

//    if (th != null && th.HasClass("infobox-header"))
//    {
//        if (th.InnerText != "Formula One World Championship career")
//            break;
//    }

//    if (th != null && td != null)
//    {
//        Console.Write(th.InnerText + " : ");
//        Console.WriteLine(td.InnerText);
//    }
//    else if (th != null && td == null)
//    {
//        Console.WriteLine("---");
//        Console.WriteLine(th.InnerText);
//        Console.WriteLine("---");        
//    }
//}